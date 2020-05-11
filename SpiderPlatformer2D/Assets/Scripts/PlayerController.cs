using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    float health;
    //Component Referances
    Rigidbody2D rigidBody;
    Animator animator;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D playerFeetCollider2D;

    //State
    bool isFacingRight = true;
    bool isAlive = true;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        playerFeetCollider2D = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        if(isAlive)
        {
            Run();
            Jump();
            FlipSprite();
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
        if(!playerFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Vector2 jumpForce = new Vector2(0, jumpSpeed);
            rigidBody.velocity += jumpForce;
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
