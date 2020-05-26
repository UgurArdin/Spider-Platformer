using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBullet : MonoBehaviour
{
    public GameObject hitParticle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grappable"|| collision.gameObject.tag == "Pullable")
        {
            ContactPoint2D contact = collision.contacts[0];
            GameObject bulletInstance = Instantiate(hitParticle, contact.point, Quaternion.identity);
            bulletInstance.transform.parent = collision.transform;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
