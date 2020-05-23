using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrappleBullet : MonoBehaviour
{
    public bool bossHoldingPlayer = false;
    
    BossGrapple grapple;
    [SerializeField] GameObject grappableObject;
    [SerializeField] GameObject webParticle;

    private void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Quaternion angle = Quaternion.identity;
            //angle.eulerAngles = grapple.shootPoint.eulerAngles + new Vector3(0, 0, -90f);
           
            ContactPoint2D contact = collision.contacts[0];
            GameObject bulletInstance = Instantiate(grappableObject, contact.point, Quaternion.identity);
            GameObject webPrefab = Instantiate(webParticle, contact.point, Quaternion.identity);
            webPrefab.transform.parent = collision.transform;
            bulletInstance.transform.parent = collision.transform;
            grapple.PullableHit(bulletInstance);
            
           

            Destroy(gameObject);

        }
    }


    public void SetGrapple(BossGrapple grapple)
    {
        this.grapple = grapple;
    }
}
