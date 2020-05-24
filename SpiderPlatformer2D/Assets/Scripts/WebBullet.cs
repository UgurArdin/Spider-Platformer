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
            Vector3 defaultScale = hitParticle.transform.localScale;
            GameObject bulletInstance = Instantiate(hitParticle, contact.point, Quaternion.identity);
            bulletInstance.transform.SetParent(collision.gameObject.transform,true);
            bulletInstance.transform.localScale = defaultScale/ collision.gameObject.transform.localScale.x;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
