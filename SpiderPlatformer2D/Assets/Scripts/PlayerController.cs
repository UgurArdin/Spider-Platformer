using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float grapplePullSpeed = 3f;
    [SerializeField] float grappleForce = 3f;
    [SerializeField] float grappleRadious;

    [HideInInspector] public float gravityDefaultValue;

    //Component Referances
    Rigidbody2D rigidBody;
    Animator animator;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D playerFeetCollider2D;
    Grapple grapple;

    float currentGrappleSpeed;
    float health;
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

        currentGrappleSpeed = grapplePullSpeed;
        grapple = GetComponentInChildren<Grapple>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
                        //transform.position += direction * Time.deltaTime * grapplePullSpeed;
                        rigidBody.AddForce(direction * grappleForce);
                        grapplePullSpeed -= Time.deltaTime*4f;
                        if(grapplePullSpeed<=0.5f)
                        {
                            grapplePullSpeed = 0.5f;
                        }
                        rigidBody.gravityScale = 0;
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {

                            grapple.target = null;
                            grapple.springJoint.enabled = false;
                            rigidBody.gravityScale = gravityDefaultValue;

                        }
                    }
                    else
                    {
                        grapplePullSpeed = currentGrappleSpeed;
                        rigidBody.gravityScale = gravityDefaultValue;
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
            Debug.Log("Facing right");
        }
        else 
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundMask);
            //Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.black);
            Debug.Log("Facing left");
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
            animator.SetBool("IsJumping", true);
            isJumping = true;
        }

        else
        {
            animator.SetBool("IsJumping", false);
            isJumping = false;
        }

        if (rigidBody.velocity.y < -.1f)
        {
            animator.SetBool("IsFalling", true);
        }

        else
        {
            animator.SetBool("IsFalling", false);
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
    public void ResetGrappleSpeed()
    {
        grapplePullSpeed = currentGrappleSpeed;
    }
    
}
