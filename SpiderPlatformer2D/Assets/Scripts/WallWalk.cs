using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalk : MonoBehaviour
{
    Rigidbody2D rb;
    bool isGrounded;
    [SerializeField] LayerMask groundLayers;
    

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
       
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapArea (new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
        new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f),groundLayers);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        {
            switch (collision.gameObject.tag)
            {
                case "UpGround":
                    Debug.Log("Upground" + isGrounded);
                    
                        transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case "RightGround":
                    if (isGrounded)
                        Debug.Log("Rightground" + isGrounded);
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case "LeftGround":
                    Debug.Log("Leftground" + isGrounded);
                    if (isGrounded)
                        transform.eulerAngles = new Vector3(0, 0, -90);
                    break;

            }
        }
    }

}
