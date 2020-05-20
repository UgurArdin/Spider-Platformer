using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float grappleForceMultiplier = 3f;
    [SerializeField] float grappleRadious;
    [SerializeField] float maxGrappleForce;
    [SerializeField] float playerHealth;



    [HideInInspector] public float gravityDefaultValue;

    //Component Referances
    Rigidbody2D rigidBody;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D playerFeetCollider2D;
    Grapple grapple;
    public Animator animator;
    public Slider HealthBar;
    public GameObject DeadMenu;
    [SerializeField] GameObject webSnapParticle;


    //State
    bool isAlive = true;
    bool isJumping;
    bool isFacingRight;

    [Header("Wall Jump")]
    public LayerMask groundMask;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;
    float mx = 0;

    void Start()
    {
        grapple = GetComponentInChildren<Grapple>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        playerFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityDefaultValue = rigidBody.gravityScale;
    }


    void Update()
    {
        if (isAlive)
        {
            Run();
            Jump();
            FlipSprite();
            WallJump();
            ManageJumpingAndFallingAnim();
            if (grapple.GetIsGrapple())
            {
                if (grapple.GetTarget() != null)
                {
                    float distanceBetweenObjectAndPlayer = Vector3.Distance(grapple.GetTargetPos(),transform.position);
                    GameObject targetInstance = grapple.GetTarget();
                    if (distanceBetweenObjectAndPlayer >= 2f)
                    {
                        Vector3 direction = targetInstance.transform.position - transform.position;
                        direction.x = Mathf.Clamp(direction.x, -maxGrappleForce, maxGrappleForce);
                        direction.y = Mathf.Clamp(direction.y, -maxGrappleForce, maxGrappleForce);
                        rigidBody.AddForce(direction * grappleForceMultiplier);

                        rigidBody.gravityScale = 0;
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {
                            GameObject particle = Instantiate(webSnapParticle,(transform.position+ grapple.target.transform.position)/2, 
                                Quaternion.identity);
                            Destroy(particle,1);
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
                        targetInstance.GetComponent<Rigidbody2D>().AddForce(-direction * 5);
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {
                            GameObject particle = Instantiate(webSnapParticle, (transform.position + grapple.target.transform.position) / 2,
 Quaternion.identity);
                            grapple.target = null;
                            DestroyBoxes();
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
            //Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.black);

        }
        else 
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundMask);
            //Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.black);

        }
       
        if (WallCheckHit && NotInGround() && PlayerHasVelocity())
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
        }

        if(isWallSliding)
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
        if (NotInGround() &&!isWallSliding) { return; }

        if (Input.GetKeyDown(KeyCode.Space) ||isWallSliding &&Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 jumpForce = new Vector2(0, jumpSpeed);
            rigidBody.velocity += jumpForce;
        }
    }

    private bool NotInGround()
    {

        return !playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void ManageJumpingAndFallingAnim()
    {
        if (rigidBody.velocity.y > 0)
        {
            animator.SetBool("isJumping", true);
            isJumping = true;
        }

        else
        {
            animator.SetBool("isJumping", false);
            isJumping = false;
        }

        if (rigidBody.velocity.y < -.1f)
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
    public void DestroyBoxes()
    {
        var boxes = FindObjectsOfType<Boxes>();
        if(boxes!=null)
        {
            foreach (Boxes box in boxes)
            {
                Destroy(box.gameObject);
            }
        }     
    }
    
}
