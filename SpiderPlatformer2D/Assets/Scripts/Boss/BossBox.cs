using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBox : MonoBehaviour
{
    private void Awake()
    {

        BossRunAnimation.DestroyBossBoxesInTheScene += DestroyMe;
    }
    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        BossRunAnimation.DestroyBossBoxesInTheScene -= DestroyMe;
    }
}
