﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glide : MonoBehaviour
{
    bool glideGravity;
    float defaultGravity;
    public float glidingGravity;
    public float glideTime;
    public GameObject glideTrail;
    private Rigidbody2D rb;
    

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
        
    }


    void Update()
    {
        if (rb.velocity.y < -0.1f)
        {
       
            
            if (Input.GetKey(KeyCode.Space)) 
            {


                if (glideGravity == false)
                {
                    Debug.Log(rb.velocity.y);
                    glideGravity = true;
                    glideTrail.SetActive(true);
                    GetComponent<Rigidbody2D>().gravityScale = defaultGravity * glidingGravity;
                }
               
            }
           



        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (glideGravity)
            {
                glideGravity = false;
                glideTrail.SetActive(false);
                GetComponent<Rigidbody2D>().gravityScale = defaultGravity;
            }
        }



    }
        
        }



