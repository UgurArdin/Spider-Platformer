using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBullet : MonoBehaviour
{
    private Grapple grapple;
    [SerializeField] GameObject grappableObject;
    [SerializeField] GameObject webParticle;
    private void Start()
    {
      
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grappable"|| collision.gameObject.tag == "Pullable")
        {
            Quaternion angle = Quaternion.identity;
            angle.eulerAngles = grapple.shootPoint.eulerAngles + new Vector3(0, 0, -90f);
            ContactPoint2D contact = collision.contacts[0];
            GameObject bulletInstance = Instantiate(grappableObject, contact.point, Quaternion.identity);
            GameObject webPrefab = Instantiate(webParticle, contact.point, angle);
            webPrefab.transform.parent = collision.transform;
            bulletInstance.transform.parent = collision.transform;
            if (collision.gameObject.tag == "Grappable")
            {
                grapple.TargetHit(bulletInstance);
            }
            if (collision.gameObject.tag == "Pullable")
            {
                grapple.PullableHit(collision.gameObject);
            }

            Destroy(gameObject);

        }
    }


    public void SetGrapple(Grapple grapple)
    {
        this.grapple = grapple;
    }
}
