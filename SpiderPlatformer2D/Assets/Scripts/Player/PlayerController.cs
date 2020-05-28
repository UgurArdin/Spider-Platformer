using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed ;
    [SerializeField] float pullingForceMultiplier;
    [SerializeField] float grappleForceMultiplier;
    [SerializeField] float maxGrappleForce;
    [SerializeField] float grappleRadious;
    [SerializeField] float playerHealth;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float bossGrappleForceMultiplier;
    [SerializeField] Boss boss;

    public delegate void DestroySomeStuff();
    public static event DestroySomeStuff DestroyBoxesInPlayerController;
    public delegate void DestroyWeb();
    public static event DestroyWeb DestroyWebs;

    [HideInInspector] public float gravityDefaultValue;

    //Component Referances
    Rigidbody2D rigidBody;
    Grapple grapple;
    public Animator animator;
    public Slider HealthBar;
    public GameObject DeadMenu;
    [SerializeField] GameObject webSnapParticle;
    [SerializeField] Transform groundCheck;


    //State
    [SerializeField] public bool isAlive = true;
    bool isJumping;
    bool isFacingRight;
    bool isGrounded;

    [Header("Wall Jump")]
    public LayerMask groundMask;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    public float jumpDelay = 2f;
    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;
    float mx = 0;

    void Start()
    {
        Cursor.visible = true;
        grapple = GetComponentInChildren<Grapple>();
        rigidBody = GetComponent<Rigidbody2D>();
        gravityDefaultValue = rigidBody.gravityScale;
    }


    void Update()
    {
        if (isAlive)
        {
            InteractionWithBoss();
            Run();
            Jump();
            FlipSprite();
            WallJump();
            ManageJumpingAndFallingAnim();
            if (grapple.GetIsGrapple())
            {
                if (grapple.GetTarget() != null)
                {
                    float distanceBetweenObjectAndPlayer = Vector3.Distance(grapple.GetTargetPos(), transform.position);
                    GameObject targetInstance = grapple.GetTarget();
                    if (distanceBetweenObjectAndPlayer >= 2f)
                    {
                        Vector3 direction = targetInstance.transform.position - transform.position;
                        direction.x = Mathf.Clamp(direction.x, -maxGrappleForce, maxGrappleForce);
                        direction.y = Mathf.Clamp(direction.y, -maxGrappleForce, maxGrappleForce);
                        rigidBody.AddForce(direction * grappleForceMultiplier * Time.deltaTime);

                        rigidBody.gravityScale = 0;
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {
                            GameObject particle = Instantiate(webSnapParticle, (transform.position + 
                                grapple.target.transform.position) / 2,Quaternion.identity);

                            if(DestroyWebs!=null)
                            {
                                DestroyWebs();
                            }
                            if(DestroyBoxesInPlayerController!=null)
                            {
                                DestroyBoxesInPlayerController();
                            }
                            Destroy(particle, 1);
                            grapple.target = null;
                            grapple.springJoint.enabled = false;
                            rigidBody.gravityScale = gravityDefaultValue;
                        }
                    }
                    else
                    {
                        rigidBody.gravityScale = gravityDefaultValue;
                        GetComponentInChildren<Grapple>().DisableSprintJoint();
                    }
                }
            }

            if (grapple.GetIsPulling())
            {
                if (grapple.GetTarget() != null)
                {
                    float distanceBetweenObjectAndPlayer = Vector3.Distance(grapple.GetTargetPos(), transform.position);
                    GameObject targetInstance = grapple.GetTarget();
                    if (distanceBetweenObjectAndPlayer >= 2f)
                    {
                        Vector3 direction = targetInstance.transform.position - transform.position;
                        targetInstance.GetComponent<Rigidbody2D>().AddForce(-direction * pullingForceMultiplier * Time.deltaTime);
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {
                            GameObject particle = Instantiate(webSnapParticle, (transform.position + grapple.target.transform.position) / 2,
                            Quaternion.identity);
                            grapple.target = null;
                            //DestroyBoxes(); // change this with event
                            if(DestroyBoxesInPlayerController!=null)
                            {
                                DestroyBoxesInPlayerController();
                            }
                            Destroy(particle, 1);
                            grapple.springJoint.enabled = false;
                        }
                    }
                    else
                    {
                        GetComponentInChildren<Grapple>().DisableSprintJoint();
                    }
                }
            }
        }

    }
    private void FixedUpdate()
    {
        getIfGrounded();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "WaterDroplet")
        {
            col.GetComponent<Animator>().SetTrigger("getTaken");
            Destroy(col.gameObject, 2f);
            UpdateHealth(-10);
        }
        if (col.tag == "Wormling")
        {

        }
    }
    private  void InteractionWithBoss()
    {
        if (boss == null) return;
        Vector3 direction = boss.BossPos() - transform.position;
        if(BossGrappleBullet.bossHoldingPlayer)
        {
            rigidBody.AddForce(direction*bossGrappleForceMultiplier*Time.deltaTime);
        }
    }


    private void WallJump()
    {     
        mx = Input.GetAxis("Horizontal");
        if (mx < 0)
            {
                isFacingRight = false;
            }
            else
            {
                isFacingRight = true;
            }
            if (isFacingRight)
            {
                WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundMask);

            }
            else
            {
                WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundMask);

            }

            if (WallCheckHit && !isGrounded && PlayerHasVelocity())
            {
                isWallSliding = true;
                jumpTime = Time.time + wallJumpTime;
            }
            else if (jumpTime < Time.time)
            {
                isWallSliding = false;
            }

            if (isWallSliding)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, wallSlideSpeed, float.MaxValue));
            }
        
       
    }

    private void Run()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(horizontal * runSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;
        animator.SetBool("isRunning", PlayerHasVelocity());
    }
    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) ||isWallSliding &&Input.GetKeyDown(KeyCode.Space))
        {
            
            if (!isGrounded && !isWallSliding) 
            { 
                return; 
            }
            else
            {
                isJumping = true;
                Vector2 jumpForce = new Vector2(0, jumpSpeed);
                rigidBody.velocity += jumpForce;
            }
        }
    }
    private void getIfGrounded() 
    {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));//aq ugur
        animator.SetBool("isJumping", false);
        isJumping = false;

    }

    void ManageJumpingAndFallingAnim()
    {
        if (rigidBody.velocity.y > 0&& !isGrounded)
        {
            animator.SetBool("isJumping", true);
        }

        else
        {
            animator.SetBool("isJumping", false);
            isJumping = false;
        }

        if (rigidBody.velocity.y < -.1f&&!isGrounded)
        {
            animator.SetBool("isFalling", true);
        }

        else
        {
            animator.SetBool("isFalling", false);
        }
    }
    public void UpdateHealth(int damage)
    {
        
        playerHealth -= damage;
        HealthBar.value = playerHealth;
        if (HealthBar.value <= 0&& isAlive)
        {
            isAlive = false;
            animator.SetTrigger("Die");
            DeadMenu.SetActive(true);
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
    private void FlipSprite()
    {
        if (PlayerHasVelocity())
        {

            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(rigidBody.velocity.x), transform.localScale.y); // return 1 if velocity.x greater than 0 , return -1 if velocity is less than 0; //change back to 1
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    private bool PlayerHasVelocity()
    {
        if (Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon)
        {
            return true;
        }
        return false;
    }
   
    
}
