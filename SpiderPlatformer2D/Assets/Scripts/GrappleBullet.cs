using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBullet : MonoBehaviour
{
    private Grapple grapple;
    private void Start()
    {
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grappable")
        {

                grapple.TargetHit(collision.gameObject);

            Destroy(gameObject);
           
        }
    }
    public void SetGrapple(Grapple grapple)
    {
        this.grapple = grapple;
    }
}
