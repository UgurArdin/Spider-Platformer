using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web_Projectile : MonoBehaviour
{
    public float webRate;
    public float webDamage;
    public LayerMask whatToHit;

    public Transform webTrailPrefab;

    float timeToWeb;
    public Transform webPoint;
    public GameObject bulletPref;

    public float bulletforce = 20f;
    void Awake()
    {
        GetComponent<Rigidbody2D>();
        if (webPoint == null)
        {
            Debug.LogError("hay amk");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (webRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1") && Time.deltaTime > timeToWeb)
            {
                timeToWeb = Time.deltaTime + webRate;
                Shoot();
            }
        }
    void Shoot()
        {
            Vector2 mousePos;
            mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 webPointPos = new Vector2(webPoint.position.x, webPoint.position.y);
            RaycastHit2D hit = Physics2D.Raycast(webPointPos, mousePos - webPointPos, 100, whatToHit);
            Effect();
            Debug.DrawLine(webPointPos, (mousePos - webPointPos) * 100, Color.cyan);
            if(hit.collider!= null)
            {
                Debug.DrawLine(webPointPos, hit.point, Color.red);
                //Debug.Log("FIÇIK" + hit.collider.name);
            }

            GameObject bullet = Instantiate(bulletPref, webPoint.position, webPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(webPoint.right * bulletforce, ForceMode2D.Impulse);
        
        
        
        }

        void Effect()
        {
            Instantiate(webTrailPrefab, webPoint.position, webPoint.rotation);
        }
    }
}
