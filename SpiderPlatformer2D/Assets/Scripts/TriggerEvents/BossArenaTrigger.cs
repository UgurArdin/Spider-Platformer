using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    [SerializeField] Animator wallAnim;
    [SerializeField] GameObject bossSpider;
    [SerializeField] GameObject defaultCamera;
    [SerializeField] GameObject bossCamera;


    private void OnTriggerExit2D(Collider2D collision)
    {
        wallAnim.SetBool("isClosed", true);
        bossSpider.SetActive(true);
        defaultCamera.SetActive(false);
        bossCamera.SetActive(true);
    }
}
