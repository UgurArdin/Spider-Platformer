using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BeeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed;
    public float nextWaypointDistance;
    public float maxChaseRange;
    [SerializeField] private GameObject beeAttackParticle;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    int currentWaypoint = 0;
    bool reachedEndOfPath= false;
    bool isChasing=false;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    void UpdatePath()
    {
        if(seeker.IsDone()&& isChasing)
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < maxChaseRange)
        {
            isChasing = true;
        }
        if (isChasing) 
        {
            if (path == null)
            {
                return;
            }
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);
            float distanceBetweenWaypoints = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distanceBetweenWaypoints < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if (force.x >= 0.01f)
            {
                sr.flipX = true;
            }
            else if (force.x <= 0.01f)
            {
                sr.flipX = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject particle = Instantiate(beeAttackParticle,col.transform.position, Quaternion.identity);
            Destroy(particle, 0.7f);
            col.gameObject.GetComponent<PlayerController>().UpdateHealth(10);
        }
        if (col.gameObject.tag == "WebBullet")
        {
            anim.SetTrigger("Die");
            Destroy(gameObject, 0.7f);
            
        }
    }
}
