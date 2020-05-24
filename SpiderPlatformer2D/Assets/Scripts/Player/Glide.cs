using System.Collections;
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
        if (Input.GetKey(KeyCode.Space) && rb.velocity.y < -0.4f && glideGravity == false)
        {
            glideGravity = true;
            glideTrail.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = defaultGravity * glidingGravity;
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



