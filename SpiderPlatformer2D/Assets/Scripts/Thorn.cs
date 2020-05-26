using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{

    [SerializeField]GameObject damageparticle;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            DamageParticles(col);
            col.gameObject.GetComponent<PlayerController>().UpdateHealth(10);
        }
        if (col.gameObject.tag == "Pullable")
        {
            if (col.gameObject.GetComponent<BeeEnemy>() != null)
            {
                col.gameObject.GetComponent<BeeEnemy>().Die();
                DamageParticles(col);
            }
        }
    }
    void DamageParticles(Collision2D col)
    {
        ContactPoint2D contact = col.contacts[0];
        GameObject particle = Instantiate(damageparticle,contact.point, Quaternion.identity);
        Destroy(particle, 0.7f);
    }
}
