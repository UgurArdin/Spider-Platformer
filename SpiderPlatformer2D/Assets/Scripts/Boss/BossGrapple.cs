using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrapple : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    public Transform shootPoint;
    public LineRenderer lineRenderer;
    [HideInInspector] public bool isGrappled = false;
    [HideInInspector] public GameObject target;
    Animator anim;
    GameObject playerObject;    


    private void Start()
    {
        playerObject= GameObject.FindGameObjectWithTag("Player");
        lineRenderer.enabled = false;
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isGrappled)
        {
            if (target != null)
            {
                anim.SetBool("isGrappled", true);
                lineRenderer.SetPosition(0, shootPoint.position);
                lineRenderer.SetPosition(1, target.transform.position);
            }
            else
            {
                anim.SetBool("isGrappled", false);
                lineRenderer.enabled = false;
            }
        }
    }

    public void Shoot()
    {
        if (!isGrappled) //burdan devam edicez
        {
            Vector3 difference = playerObject.transform.position - shootPoint.position;
            float angleZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            shootPoint.rotation = Quaternion.Euler(0, 0, angleZ);
            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootPoint.right * bulletSpeed);
            bulletInstance.GetComponent<BossGrappleBullet>().SetGrapple(this);
            Destroy(bulletInstance, 0.8f);//grappleRangeLimiter
           
            isGrappled = true;
            StartCoroutine(DeactivateBossGrapple(2f));
        }
    }

    IEnumerator DeactivateBossGrapple(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        anim.SetBool("isGrappled", false);
        target = null;
        lineRenderer.enabled = false;
        BossGrappleBullet.bossHoldingPlayer = false;
        //yield return new WaitForSeconds(1f);        //yield return new WaitForSeconds(1f);
        isGrappled = false;
    }
    public void PullableHit(GameObject hit) //when our hidden bullet hits the object with Grappable tag , we will call this method from GrappleBullet
    {
        target = hit;
        lineRenderer.enabled = true;
    }

    public bool GetIsGrapple()
    {
        return isGrappled;
    }
    public GameObject GetTarget()
    {
        return target;
    }
    public Vector3 GetTargetPos()
    {
        return target.transform.position;
    }
}
