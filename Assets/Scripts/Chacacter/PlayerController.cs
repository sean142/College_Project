using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Player Bool")]
    public bool canMove = true;
    public bool canTalk = false;
    public bool canAnimator = true;
    public bool canJamp = true;
    public bool isGround;
    public bool isDead;
    public bool canMoveObstacle;
    public bool canPush;

    [Header("Player Speed")]
    public float currentSpeed;
    public float runSpeed;
    public float normalSpeed;
    public float speedTransitionTime;  // 控制Animator Blend Trees參數的過渡時間
    public float runSpeedTransitionTime = 0f;
    public float normalSpeedTransitionTime = 1f;

    [Header("Player Attack")]
    //public float attackCooldown;
    public bool isAttacking = false;

    private GameObject lockedEnemy;
    private float lastAttackTime;
    public LayerMask enemyLayer; 

    [Header("Player Jump")]
    public float jumpfarce;
    public float gravity = -9.81f;
    Vector3 velocity;

    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    public GameObject camObj;
    //public bool isLockedCamera;

    [Header("Disintegrate&&Dissolve")]
    public SkinnedMeshRenderer skinnedMesh;
    public Material skinnedMaterial;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    [Header("VFX")]
    public GameObject vfxAbsorb;
    public GameObject vfxCaneTrail;
    public VisualEffect vfxWarpSpeed;
    private GameObject vfxWarpSpeedObject;
    public float rate = 0.02f;
    private bool warpActive;

    [Space(10)]
    public float forceMagnitude;//做用力的大小  

    [Space(10)]
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    private CharacterController coll;
    private Animator animator;
    private CharacterStats characterStats;
    public Animator caneAnimator;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        coll = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        vfxWarpSpeedObject = GameObject.FindGameObjectWithTag("VfxWarpSpeed");
        vfxWarpSpeed = vfxWarpSpeedObject.GetComponent<VisualEffect>();

        vfxCaneTrail = GameObject.FindGameObjectWithTag("VfxCaneTrail");
        camObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camObj.transform;
    }

    void Start()
    {
        currentSpeed = normalSpeed;
        speedTransitionTime = normalSpeedTransitionTime;

        GameManager.Instance.RigisterPlayer(characterStats);

        SaveManager.Instance.LoadPlayerData();
        SaveManager.Instance.LoadCoreData();

        if (!SceneController.instance.isFirstTimeInGame)
        {
            SaveManager.Instance.LoadPlayerPositionData();
        }
        else
            animator.SetTrigger("StandUp");


        //characterStats.CurrentHealth = 50;
        vfxCaneTrail.SetActive(false);
        vfxWarpSpeed.Stop();
        vfxWarpSpeed.SetFloat("WarpAmount", 0);
    }

    private void Update()
    {
        isGround = Physics.Raycast(groundCheck.position, -transform.up, groundDistance, groundMask);
       
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (canMove)
        {
            Movement();
            Jump();
            AbsorptionAndUseCore();
        }
        Attack();
        SwitchAnimator();
        OpenBag();
        Push();
        ContinueIdle();

        lastAttackTime -= Time.deltaTime;
        //attackCooldown -= Time.deltaTime;

        if (characterStats.CurrentHealth == 0)
        {
            animator.SetBool("Death", true);
            isDead = true;
            canJamp = false;
            canMove = false;
        }

        if (isDead)
            GameManager.Instance.NotifyObservers();

        //if(Input.GetKeyDown(KeyCode.Space))
        //     StartCoroutine(Dissolve());

    }

    void Attack()
    {
        if (FoundEnemy() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            vfxCaneTrail.SetActive(true);
            if (!isAttacking)
                StartCoroutine(Attacking());
            else
                StartCoroutine(MoveToAttackTarget()); 

        }

        // 判斷當前播放的動畫長度是否超過70%
        AnimatorStateInfo currentAnimState = animator.GetCurrentAnimatorStateInfo(1);
        if (currentAnimState.normalizedTime >= 0.7f && (currentAnimState.IsName("hit1") || currentAnimState.IsName("hit2") || currentAnimState.IsName("hit3")))
        {
            animator.SetTrigger("idle");
            isAttacking = false;
            vfxCaneTrail.SetActive(false);
        }    
    }

    private IEnumerator Attacking()
    {
        if (lockedEnemy != null)
        {
            if (Vector3.Distance(transform.position, lockedEnemy.transform.position) < characterStats.attackData.attackRange)
            {
                transform.LookAt(lockedEnemy.transform);
                yield return null;
            }
        }
        //if (attackCooldown <= 0f)
        //{
        //    animator.SetTrigger("hit1");
        //    isAttacking = true;
        //    attackCooldown = characterStats.attackData.coolDown;
        //}
        if (lastAttackTime <= 0f)
        {
            animator.SetTrigger("hit1");
            isAttacking = true;
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    private IEnumerator MoveToAttackTarget()
    {
        if (lockedEnemy != null)
        {
            if (Vector3.Distance(transform.position, lockedEnemy.transform.position) < characterStats.attackData.attackRange)
            {
                transform.LookAt(lockedEnemy.transform);
                yield return null;
            }
        }
        animator.SetTrigger("attack");

        if (lastAttackTime <= 0f)        //cool down
        {
            //animator.SetTrigger("Attack");
            //lastAttackTime = characterStats.attackData.coolDown;

            //canMove = false;
        }
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;

        float inputMagnitude = Mathf.Clamp01(direction.magnitude);
        animator.SetFloat("Speed", inputMagnitude, speedTransitionTime, Time.deltaTime);

        if (direction.magnitude >= 0.1f && currentSpeed > 0f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            coll.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
   
    void Jump()
    {
        if (canJamp)
        {
            if (Input.GetButton("Jump") && isGround)
            {
                canJamp = false;
                animator.SetBool("Move", false);
                velocity.y = Mathf.Sqrt(jumpfarce * -2f * gravity);
                animator.SetBool("Jump", true);
            }
        }        
        
        velocity.y += gravity * Time.deltaTime;
        coll.Move(velocity * Time.deltaTime);
    }   
    
    void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //CoreInventory.instance.ToggleInventory();
            InvertoryManager.instance.OnControlBagButtonClick();
        }
    }

    void Push()
    {
        if (Input.GetKey(KeyCode.R) &&canPush)
        {
            animator.SetBool("canPush", true);
            canMoveObstacle = true;          
        }
        else
        {
            animator.SetBool("canPush", false);
            canMoveObstacle = false;          
        }
    }

    bool FoundEnemy()
    {
        // 當玩家死亡後 直接返回false 不會再搜尋敵人 
        if (isDead)
        {
            lockedEnemy = null;
            return false;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, characterStats.attackData.attackRange, enemyLayer);
        if (colliders.Length > 0)
        {
            // 找到最近的敵人
            float minDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = col.gameObject;
                }
            }
            lockedEnemy = nearestEnemy;
            return true;
        }       

        else
        {
            lockedEnemy = null;
            return false;
        }      
    }     

    void SwitchAnimator()
    {
        if (!canAnimator)
        {
            animator.SetBool("Move", false);
        }
        
        if (isGround)
        {
            animator.SetBool("Grounded", true);
            animator.SetBool("Fall", false);
        }
        else if (!isGround && velocity.y > 0)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Fall", false);
        }
        else if (!isGround && velocity.y < 0)
        {
            animator.SetBool("Grounded", false);
            animator.SetBool("Fall", true);
            canJamp = false;
        }
    }

    //當控制器與物體碰撞時觸發此函數
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 檢查是否有碰撞到帶有剛體的物體
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null && canMoveObstacle)
        {
            //計算施加力的方向向量，從控制器指向被碰撞物體的位置
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.z = 0;

            //正規化方向向量，確保長度為 1，但方向不變(與原始向量相同)
            forceDirection.Normalize();

            // 以腳下位置 (transform.position) 施加力到物體上，使用衝量模式 (ForceMode.Impulse)
            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }        
    }

    //Animation Event
    public void Hit()
    {
        var targetStats = lockedEnemy.GetComponent<CharacterStats>();

        targetStats.TakeDamage(characterStats, targetStats);
    }

    public void AnimatorClear()
    {
        animator.SetBool("Jump", false);
        canJamp = true;
    }
    // 吸收敵人的靈魂 
    public void AbsorbAnimation()
    {       
        //Destroy(lockedEnemy.gameObject);
    }
    
    public void CanMoveAnimationEvent()
    {
        canMove = true;
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

    private void AbsorptionAndUseCore()
    {
        if (!CoreManager.instance.isUseTime)
        {
            if (Input.GetKeyDown(KeyCode.F) && CoreInventory.instance.currentInt != 0)
            {
                CoreManager.instance.UseCoreAbility(CoreInventory.instance.currentInt);
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
            vfxAbsorb.SetActive(true);
        if(Input.GetKeyUp(KeyCode.E))
            vfxAbsorb.SetActive(false);

        if (Input.GetKeyDown(KeyCode.E) && !CoreManager.instance.isBeingAbsorbed && CoreManager.instance.isCoreTurnOn && !CoreManager.instance.isCoreGenerating)
        {
            // 開始吸收計時
            animator.SetBool("Absorb", true);
            caneAnimator.SetBool("openCaneHead", true);
            CoreManager.instance.TurnOnTrail();
            //CoreManager.instance.absorptionTimer = 0.0f;

            CoreManager.instance.isBeingAbsorbed = true;
            vfxAbsorb.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.E) && CoreManager.instance.isBeingAbsorbed && !CoreManager.instance.isCoreGenerating)
        {
            animator.SetBool("Absorb", false);
            caneAnimator.SetBool("openCaneHead", false);

            CoreManager.instance.isBeingAbsorbed = false;
            vfxAbsorb.SetActive(false);
            CoreManager.instance.TurnOffTrail();
        }
    }
    
    public void StandUPControlAnimationEvent()
    {
        SceneController.instance.isStandingUp = true;
    }

    void ContinueIdle()
    {
        if (SceneController.instance.isStandingUp)
        {
            canMove = true;
            GameManager.Instance.followCinema.m_YAxis.m_MaxSpeed = 2;
            GameManager.Instance.followCinema.m_XAxis.m_MaxSpeed = 400;        
        }      
    }

    public void DoParticles()
    {
        warpActive = true;
        StartCoroutine(ActivateParticles());
    }

    public void NotDoParticles()
    {
        warpActive = false;
        StartCoroutine(ActivateParticles());
    }

    IEnumerator ActivateParticles()
    {
        if (warpActive)
        {
            vfxWarpSpeed.Play();

            float amount = vfxWarpSpeed.GetFloat("WarpAmount");
            while (amount < 1 && warpActive)
            {
                amount += rate;
                vfxWarpSpeed.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            float amount = vfxWarpSpeed.GetFloat("WarpAmount");
            while (amount > 0 && !warpActive)
            {
                amount -= rate;
                vfxWarpSpeed.SetFloat("WarpAmount", amount);

                if (amount <= 0 + rate)
                {
                    amount = 0;
                    vfxWarpSpeed.SetFloat("WarpAmount", amount);
                    vfxWarpSpeed.Stop();
                }
            }

        }
    }
}
