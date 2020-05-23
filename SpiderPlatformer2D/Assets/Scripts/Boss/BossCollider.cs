using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    public Boss boss;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "GrappleBullet") 
        boss.getDamage();
    }
}
