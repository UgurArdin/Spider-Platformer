using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BeeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed;
    public float nextWaypointDistance;
    public float maxChaseRange;
    public float attackRate;
    [SerializeField] private GameObject beeAttackParticle;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Animator anim;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    int currentWaypoint = 0;
    bool reachedEndOfPath= false;
    bool isChasing=false;
    bool isDead = false;
    bool canAttackDirectly=true;
    bool isAttackReady;
    float attackRateValue;
    float xScaleValue;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        xScaleValue = transform.localScale.x;
        attackRateValue = attackRate;

    }
    void UpdatePath()
    {
        if(seeker.IsDone()&& isChasing &&!isDead)
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
        if (distance < maxChaseRange && !isDead)
        {
            isChasing = true;
        }
        if (isChasing && !isDead)
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
            Vector2 directionOfPlayer = ((Vector2)target.transform.position- rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);
            float distanceBetweenWaypoints = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distanceBetweenWaypoints < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            if (rb.velocity.magnitude > 0.2)
            {
                if (Mathf.Sign(directionOfPlayer.x) > 0)
                {
                    transform.localScale = new Vector3(xScaleValue, transform.localScale.y, 1);
                }
                else if (Mathf.Sign(directionOfPlayer.x) < 0)
                {
                    transform.localScale = new Vector3(-xScaleValue, transform.localScale.y, 1);
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !isDead&& canAttackDirectly)
        {
                DamagePlayer(col);
            canAttackDirectly = false;


        }
        if (col.gameObject.tag == "WebBullet" &&!isDead)
        {
            Die();
        }

    }
    private void OnCollisionStay2D(Collision2D col)
    {

        if (col.gameObject.tag == "Player" && !isDead)
        {
            attackRateValue -= Time.deltaTime;
            if (attackRateValue <= 0)
            {
                DamagePlayer(col);
                attackRateValue = attackRate;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(AttackExitDelay(0.5f));
    }

    IEnumerator AttackExitDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canAttackDirectly = true;
    }
    void DamagePlayer(Collision2D col)
        {
        anim.SetTrigger("Attack");
        GameObject particle = Instantiate(beeAttackParticle, col.transform.position, Quaternion.identity);
            Destroy(particle, 0.7f);
            col.gameObject.GetComponent<PlayerController>().UpdateHealth(10);
        }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        Destroy(gameObject, 1.1f);
        rb.gravityScale = 2;
    }

}
