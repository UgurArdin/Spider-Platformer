using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed = 2000f;
    public PlayerController playerController;

    public Transform shootPoint;
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
        RotateGrapple();
        if (Input.GetMouseButton(1)&&!isGrappled && !isPulling)
        {
            Shoot();
        }
        else if (Input.GetMouseButtonUp(1)) // Added later
        {
            var webBullet = GameObject.FindGameObjectsWithTag("GrappleWeb");
            if (webBullet != null)
            {
                foreach (GameObject web in webBullet)
                {
                    Destroy(web.gameObject,2f);
                }
            }
            playerController.DestroyBoxes();
            timeToGrapple = 0;
            target = null;
            DisableSprintJoint();
            GetComponentInParent<Rigidbody2D>().gravityScale = playerController.gravityDefaultValue;
            isGrappled = false;
            isPulling = false;
        }
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
    private void RotateGrapple()
    {
        Vector2 direction = GetMousePos() - (Vector2)transform.position;
        float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }
    private void Shoot()
    {
        timeToGrapple += Time.deltaTime;
        if (timeToGrapple >= 0.3f)
        {
            timeToGrapple = 0;
            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bulletInstance.GetComponent<GrappleBullet>().SetGrapple(this); // this method will called immedialty when bullet instance is born.
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootPoint.right * bulletSpeed);
            Destroy(bulletInstance, 0.6f);
        }
    }

    public void TargetHit(GameObject hit) //when our hidden bullet hits the object with Grappable tag , we will call this method from GrappleBullet
    {
        target = hit;
        springJoint.enabled = true;
        springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        lineRenderer.enabled = true;
        isGrappled = true;
    }
    public void PullableHit(GameObject hit) //when our hidden bullet hits the object with Grappable tag , we will call this method from GrappleBullet
    {
        target = hit;
        springJoint.enabled = true;
        springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        lineRenderer.enabled = true;
        isPulling = true;
    }
    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
