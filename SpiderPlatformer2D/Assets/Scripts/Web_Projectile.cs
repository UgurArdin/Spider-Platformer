using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web_Projectile : MonoBehaviour
{
    public float webRate;
    public float webDamage;
    public LayerMask whatToHit;
    public Transform webTrailPrefab;
    public GameObject bulletPref;
    public float bulletforce;
    [SerializeField] Transform webPoint;

    PlayerController pc;

    float webTimer;
    void Awake()
    {
        GetComponent<Rigidbody2D>();
        pc=GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.isAlive)
        {
            webTimer += Time.deltaTime;
            if (Input.GetButtonDown("Fire1") && webTimer >= webRate)
            {
                Shoot();
                webTimer = 0;
            }
        }
    }
    void Shoot()
    {
        Vector2 mousePos;
        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 webPointPos = new Vector2(webPoint.position.x, webPoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(webPointPos, mousePos - webPointPos, 100, whatToHit);

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rot2 = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;


        GameObject bullet = Instantiate(bulletPref, webPoint.position, Quaternion.Euler(0f, 0f, rot2));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(difference.x, difference.y).normalized * bulletforce, ForceMode2D.Impulse);
    }
}
