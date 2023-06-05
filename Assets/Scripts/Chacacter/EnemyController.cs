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

    private GameObject attackTarget; //player的位置

    private float speed;

    public float LookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Basic Settings")]
    public float sighRadius;//FoundPlayer

    [Header("Patrol State（巡邏狀態設置）")]
    public float potralRange;//巡邏圈的半徑大小
    Vector3 wayPoint;
    Vector3 guardPos;

    public bool isPatrol;
    public bool isDead;
    public bool playerDead;
    public CoreManager coreManager;

    public int currentInt;  //用來判斷當前敵人編號
    public Transform point; //用來生成核心位置

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        _collider = GetComponent<Collider>();
        coreManager = GameObject.Find("CoreManager").GetComponent<CoreManager>();
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

        characterStats.CurrentHealth = 1;

        //TODO 場景切換後修改掉
        GameManager.Instance.AddObserver(this);
    }
    /*
    //TODO 切換場景時啟用
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
        if (characterStats.CurrentHealth == 0 && !isDead)
        {
            EnemyDeath();

        }
        if (!playerDead)
        {
            SwitchStates();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void EnemyDeath()
    {
        animator.SetBool("Death", true);
        isDead = true;

        //TODO 有多位敵人後 如何正確生成核心  
        //coreManager.TureOnCore(coreManager.corePoolCount);
        coreManager.TureOnCore(point,currentInt);
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
                        agent.destination = transform.position; //拉托回到一個狀態
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
                        
                        //爆擊判斷(我想使用的招式，變成輪子之類的)
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        //執行攻擊
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
        //獲取下一個巡邏得隨機目標點        
        //獲取的的範圍[-potralRange,potralRange]
        float randomX = Random.Range(-potralRange, potralRange);
        float randomZ = Random.Range(-potralRange, potralRange);

        //在敵人自己本身的座標點上進行取隨機點
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, potralRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sighRadius);
    }

    //攻擊事件
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
        //TODO 血量歸零後回到重生點 寫延遲
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //停止移動
        animator.SetBool("Walk", false);
        animator.SetBool("Chase", false);

        //停止Agent
        attackTarget = null;        
    }   
}
