using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public enum EnemyStates { PATROL, CHASE, DEAD,GUARD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    public static EnemyController instance;

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
    public float sighRadius;   //FoundPlayer
    private Quaternion guardRotation;

    [Header("Patrol State（巡邏狀態設置）")]
    public float potralRange;   //巡邏圈的半徑大小
    Vector3 wayPoint;           //用於存儲巡邏的下一個目標位置點
    Vector3 guardPos;           //Enemy的當前位置

    [Header("Disintegrate&&Dissolve")]
    public SkinnedMeshRenderer skinnedMesh;
    public Material skinnedMaterial;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    [Space(10)]
    public float delayTurnOnCoreTime;

    [Space(10)]
    public bool isGuard;
    public bool isPatrol;
    public bool isDead;
    public bool playerDead;
    public CoreManager coreManager;

    public int currentInt;  //用來判斷當前敵人編號
    public Transform point; //用來生成核心位置  

    void Awake()
    {      
        instance = this;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        _collider = GetComponent<Collider>();
        coreManager = GameObject.Find("CoreManager").GetComponent<CoreManager>();

        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = LookAtTime;
        guardRotation = transform.rotation;
    }

    private void Start()
    {
        //if (isGuard)
        //{
        //    enemyStates = EnemyStates.GUARD;
        //}
        //else
        //{
        //    enemyStates = EnemyStates.PATROL;
        //    GetNewWayPoint();
        //}

        if (isPatrol)
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }

        //characterStats.CurrentHealth = 20;

        //TODO 場景切換後修改掉
        GameManager.Instance.AddObserver(this);

        skinnedMaterial.SetFloat("_DissolveAmount", 0);


        GameManager.Instance.RigisterEnemy(characterStats);

        SaveManager.Instance.LoadEnemyStateData();
        SaveManager.Instance.LoadEnemyData();

        if (isDead)
            Destroy(this.gameObject);
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

        // 有多位敵人後 如何正確生成核心  傳遞兩個參數point和currentInt給這個方法       延遲生成核心
        StartCoroutine(Static.DelayToTurnCore(() => { coreManager.TureOnCore(point, currentInt); }, delayTurnOnCoreTime));
        //StartCoroutine(CoreManager.instance.TureOnCore(point, currentInt));
        StartCoroutine(Dissolve());
    }

    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;

        else if (FoundPlayer())
            enemyStates = EnemyStates.CHASE;            
             
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                animator.SetBool("Chase", false);
                
                if (transform.position != guardPos)
                {
                    animator.SetBool("Walk", true);
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        animator.SetBool("Walk", false);
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                        Debug.LogError("test");
                    }
                }
                break;
            case EnemyStates.PATROL:
                animator.SetBool("Chase", false);
                agent.speed = speed * 0.5f;

                //判斷是否到了隨機巡邏點
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
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
            return false;
    }

    //獲取一個新的巡邏目標位置，避免卡點。  
    void GetNewWayPoint()
    {
        remainLookAtTime = LookAtTime;

        //獲取的範圍[-potralRange,potralRange]
        float randomX = Random.Range(-potralRange, potralRange);
        float randomZ = Random.Range(-potralRange, potralRange);

        //在敵人自己本身的座標點上進行取隨機點
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        // 使用 NavMesh.SamplePosition 來確保巡邏目標位置不會位於障礙物或不可通行區域，
        // 以確保遊戲角色的移動路徑順暢且不會卡住。
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

    IEnumerator Dissolve()
    {
        if (VFXGraph != null)
        {
            VFXGraph.gameObject.SetActive(true);
            VFXGraph.Play();
        }

        float counter = 0;
        while (skinnedMaterial.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            skinnedMaterial.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
