using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Bool")]
    public bool canMove = true;
    public bool canTalk = false;

    [Header("Player Speed")]
    public float speed;
    public float runSpeed;
    public float crouchSpeed;
    private float normalSpeed;
    
    [Header("Player Jump")]
    public float jumpfarce;
    public float gravity = -9.81f;

    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    
    [Header("Bag")]
    public GameObject bag;
    bool isOpen;

    public bool isGround;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;        

    private CharacterController coll;
    private Animator animator;
    private CharacterStats characterStats;
    private GameObject attackTarget;

    [Header("Basic Setting")]
    public float sighRadius;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private void Awake()
    {
        coll = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Start()
    {
        characterStats.MaxHealth = 2;
        normalSpeed = speed;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Update()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGround && velocity.y < 0)
            velocity.y = -2f;

        if (canMove)
        {
            Movement();
            Jump();
            Absorb();
        }

        OpenMyBag();
    }  

    void Absorb()
    {
        if (FoundEnemy() && Input.GetKey(KeyCode.E))
        {
            animator.SetBool("test", true);
        }
        else
        {
            animator.SetBool("test", false);
        }
    }

    void Movement()
    {             
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            coll.Move(moveDir.normalized * speed * Time.deltaTime);
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
            isOpen = !isOpen;
            bag.SetActive(isOpen);
        }
    }

    void Jump()
    {
        if (Input.GetButton("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpfarce * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        coll.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sighRadius);
    }

    bool FoundEnemy()
    {
        var colliders = Physics.OverlapSphere(transform.position, sighRadius);

        foreach(var target in colliders)
        {
            if (target.CompareTag("Enemy"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    
    public void AbsorbAnimation()
    {
        Destroy(attackTarget.gameObject);
    }

    //TODO MoveToAttackTarget

    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();

        targetStats.TakeDamage(characterStats, targetStats);
    }     
}
