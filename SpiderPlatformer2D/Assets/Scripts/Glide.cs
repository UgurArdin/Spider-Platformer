using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glide : MonoBehaviour
{
    bool glideGravity;
    float defaultGravity;
    public float glidingGravity;
    public float glideTime;
    private Rigidbody2D rb;
    

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
        
    }


    void Update()
    {
        if (rb.velocity.y <0)
        {
       
            
            if (Input.GetKey(KeyCode.Space)) 
            {


                if (glideGravity == false)
                {
                    glideGravity = true;
                    GetComponent<Rigidbody2D>().gravityScale = defaultGravity * glidingGravity;
                }
               
            }
           



        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (glideGravity)
            {
                glideGravity = false;
                GetComponent<Rigidbody2D>().gravityScale = defaultGravity;
            }
        }



    }
        
        }



