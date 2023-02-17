using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Player Bool")]
    public bool canMove = true;
    public bool canTalk = false;
    public bool canAnimator = true;
    public bool canJamp = true;
    public bool isGround;
    public bool isOpenBag;

    [Header("Player Speed")]
    public float speed;
    public float runSpeed;
    public float crouchSpeed;
    private float normalSpeed;

    [Header("Player Attack")]
    private GameObject lockedEnemy;
    private float lastAttackTime;
    public LayerMask enemyLayer;
    private bool isLockedOn = false;  // 是否鎖定視角
    private bool isAttacking = false;  // 

    [Header("Player Jump")]
    public float jumpfarce;
    public float gravity = -9.81f;

    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    public GameObject camObj;      

    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    RaycastHit hit;

    private CharacterController coll;
    private Animator animator;
    private CharacterStats characterStats;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

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
    }

    void Start()
    {
        characterStats.MaxHealth = 2;
        normalSpeed = speed;

        GameManager.Instance.RigisterPlayer(characterStats);
        camObj = GameObject.FindGameObjectWithTag("MainCamera");        
        cam = camObj.transform;
        
        //MouseManager.Instance.OnEnemyClicked += EventAttack;
    }  

    private void Update()
    {
        isGround = Physics.Raycast(groundCheck.position, -transform.up,groundDistance,groundMask);
        //isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (FoundEnemy() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(MoveToAttackTarget());
        }
        /*
        if (Input.GetKeyDown(KeyCode.Mouse1))
            isLockedOn = !isLockedOn;

        if (isLockedOn && lockedEnemy != null)
        {
            // 锁定状态下，朝向敌人
            transform.LookAt(lockedEnemy.transform.position);
        }
        */
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }       

        if (canMove)
        {
            Movement();
            Jump();           
        }

        OpenMyBag();
        SwitchAnimator();
        lastAttackTime -= Time.deltaTime;

       
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

        if (lastAttackTime <= 0f)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }
   

    void Movement()
    {             
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;

        float inputMagnitude = Mathf.Clamp01(direction.magnitude);
        animator.SetFloat("Speed", inputMagnitude, 0.6f, Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            coll.Move(moveDir.normalized * speed * Time.deltaTime);
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
        }        
        else
        {
            speed = normalSpeed;
        }
    }

    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenBag = !isOpenBag;
            InvertoryManager.instance.bag.SetActive(isOpenBag);
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
    
    bool FoundEnemy()
    {
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
        }
    }
   
    void Hit()
    {
        var targetStats = lockedEnemy.GetComponent<CharacterStats>();

        targetStats.TakeDamage(characterStats, targetStats);
    }     

    public void AnimatorClear()
    {
        animator.SetBool("Jump", false);
        canJamp = true;
    }

    

    public void AbsorbAnimation()
    {
        Destroy(lockedEnemy.gameObject);
    }
}
