using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum EnemyStates { PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private EnemyStates enemyStates;

    NavMeshAgent agent;
    Animator animator;
    CharacterStats characterStats;
    Collider _collider;

    private GameObject attackTarget; //player����m

    private float speed;

    public float LookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Basic Settings")]
    public float sighRadius;//FoundPlayer

    [Header("Patrol State�]���ު��A�]�m�^")]
    public float potralRange;//���ް骺�b�|�j�p
    Vector3 wayPoint;
    Vector3 guardPos;

    public bool isPatrol;
    public bool isDead;
    public bool playerDead;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        _collider = GetComponent<Collider>();
        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = LookAtTime;
    }

    private void Start()
    {
        if (isPatrol)
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }

        characterStats.CurrentHealth = 2;

        //TODO ����������קﱼ
        GameManager.Instance.AddObserver(this);
    }
    /*
    //TODO ���������ɱҥ�
    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
    }
    */
    void OnDisable()
    {
        if (!GameManager.IsInitalized) return;
        GameManager.Instance.RemoveObserver(this);
    }

    void Update()
    {
        if (characterStats.CurrentHealth == 0)
        {
            animator.SetBool("Death", true);
            isDead = true;
        }
        if (!playerDead)
        {
            SwitchStates();
            lastAttackTime -= Time.deltaTime;
        }
    }    

    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;

        else if (FoundPlayer())
            enemyStates = EnemyStates.CHASE;            
             
        switch (enemyStates)
        {
            case EnemyStates.PATROL:
                animator.SetBool("Chase", false);
                agent.speed = speed * 0.5f;

                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    animator.SetBool("Walk", false);
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    animator.SetBool("Walk", true);

                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                animator.SetBool("Walk", false);
                animator.SetBool("Chase", true);

                agent.speed = speed;

                if (!FoundPlayer())
                {
                    animator.SetBool("Run", false);

                    if (remainLookAtTime > 0)
                    {                        
                        agent.destination = transform.position; //�Ԧ��^��@�Ӫ��A
                        remainLookAtTime -= Time.deltaTime;
                    }

                    else if (isPatrol)
                    {
                        enemyStates = EnemyStates.PATROL;
                    }
                }
                else
                {                    
                    animator.SetBool("Run", true);

                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    animator.SetBool("Run", false);
                    agent.isStopped = true;

                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        
                        //�z���P�_(�ڷQ�ϥΪ��ۦ��A�ܦ����l������)
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        //�������
                        Attack();
                    }
                }                
                break;
            case EnemyStates.DEAD:
                _collider.enabled = false;
                agent.enabled = false;
                Destroy(gameObject, 5f);
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);

        if (TargetInAttackRange())
        {
            animator.SetTrigger("Attack");
            animator.SetBool("Critical", characterStats.isCritical==false);
        }

        if (TargetInSkillRange())
        {
            animator.SetTrigger("Skill");
            animator.SetBool("Critical", characterStats.isCritical==true);
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sighRadius);

        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else
            return false;
    }
    void GetNewWayPoint()
    {
        remainLookAtTime = LookAtTime;
        //����U�@�Ө��ޱo�H���ؼ��I        
        //��������d��[-potralRange,potralRange]
        float randomX = Random.Range(-potralRange, potralRange);
        float randomZ = Random.Range(-potralRange, potralRange);

        //�b�ĤH�ۤv�������y���I�W�i����H���I
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, potralRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sighRadius);
    }

    //�����ƥ�
    public void Hit()
    {
        if (attackTarget !=null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        playerDead = true;
        //TODO ��q�k�s��^�쭫���I �g����
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //�����
        animator.SetBool("Walk", false);
        animator.SetBool("Chase", false);

        //����Agent
        attackTarget = null;        
    }
}
