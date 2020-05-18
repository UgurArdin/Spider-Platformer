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
        if (collision.gameObject.tag == "Grappable")
        {
            Debug.Log(grapple.shootPoint.eulerAngles);
            Quaternion angle = Quaternion.identity;
            angle.eulerAngles = grapple.shootPoint.eulerAngles + new Vector3(0, 0, -90f);
            ContactPoint2D contact = collision.contacts[0];
            GameObject bulletInstance = Instantiate(grappableObject, contact.point, Quaternion.identity);
            GameObject webPrefab = Instantiate(webParticle, contact.point, angle);
            Debug.Log(webPrefab.transform.eulerAngles);
            webPrefab.transform.parent = collision.transform;
            bulletInstance.transform.parent = collision.transform;
            grapple.TargetHit(bulletInstance);

            Destroy(gameObject);

        }
    }

    public void SetGrapple(Grapple grapple)
    {
        this.grapple = grapple;
    }
}
