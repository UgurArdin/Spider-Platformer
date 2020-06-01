using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    [SerializeField] Animator wallAnim;
    [SerializeField] GameObject bossSpider;
    [SerializeField] GameObject defaultCamera;
    [SerializeField] GameObject bossCamera;
    [SerializeField] GameObject bossHealthUI;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            wallAnim.SetBool("isClosed", true);
            bossSpider.SetActive(true);
            bossHealthUI.SetActive(true);
            defaultCamera.SetActive(false);
            bossCamera.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
