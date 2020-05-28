using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    [SerializeField] Animator wallAnim;
    [SerializeField] GameObject bossSpider;
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        wallAnim.SetBool("isClosed", true);
        bossSpider.SetActive(true);
    }
}
