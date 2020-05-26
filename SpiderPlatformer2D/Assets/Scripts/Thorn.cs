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
            DamageParticles(col);
            col.gameObject.GetComponent<BeeEnemy>().Die();
        }
    }
    void DamageParticles(Collision2D col)
    {
        GameObject particle = Instantiate(damageparticle, col.transform.position, Quaternion.identity);
        Destroy(particle, 0.7f);
    }
}
