﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrapple : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform shootPoint;
    public LineRenderer lineRenderer;
    public SpringJoint2D springJoint;

    float timeToGrapple = 0;
    [HideInInspector] public bool isGrappled = false;
    bool isPulling = false;
    [HideInInspector] public GameObject target;



    private void Start()
    {
        lineRenderer.enabled = false;
        springJoint.enabled = false;
    }
    private void Update()
    {
        if (isGrappled)
        {
            if (target != null)
            {
                lineRenderer.SetPosition(0, shootPoint.position);
                lineRenderer.SetPosition(1, target.transform.position);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }


    }

    public void Shoot(Transform playerTransform)
    {
        Vector3 difference = playerTransform.position - shootPoint.position;
        float angleZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleZ);
        GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootPoint.right * bulletSpeed);
        Destroy(bulletInstance, 0.6f);//grappleRangeLimiter
        StartCoroutine(DeactivateBossGrapple(2f));
    }

    IEnumerator DeactivateBossGrapple(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        timeToGrapple = 0;
        target = null;
        DisableSprintJoint();
        isGrappled = false;
        isPulling = false;
    }
    public void PullableHit(GameObject hit) //when our hidden bullet hits the object with Grappable tag , we will call this method from GrappleBullet
    {
        target = hit;
        springJoint.enabled = true;
        springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        lineRenderer.enabled = true;
        isPulling = true;
    }

    public bool GetIsGrapple()
    {
        return isGrappled;
    }
    public bool GetIsPulling()
    {
        return isPulling;
    }
    public GameObject GetTarget()
    {
        return target;
    }
    public Vector3 GetTargetPos()
    {
        return target.transform.position;
    }
    public void DisableSprintJoint()
    {
        springJoint.enabled = false;
    }
}