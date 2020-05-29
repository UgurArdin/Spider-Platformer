using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class WaspBoss : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed;
    public float chargeSpeed;
    public float nextWaypointDistance;
    public float maxChaseRange;
    public float chargeDistance;
    public float attackRate;
    public int attackDamage;
    public Slider bossHealthSlider;
    public float bossHealth;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Animator anim;
    public GameObject bossAttackParticle;
    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    int currentWaypoint = 0;
    bool isInVulnearable = false;
    bool reachedEndOfPath= false;
    bool isChasing=false;
    bool isDead = false;
    bool canAttackDirectly=true;
    bool isAttackReady;
    bool isCharging;
    bool canCharge;
    float attackRateValue;
    float xScaleValue;
    float maxBossHealth;
    float chargeWaitTime;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        xScaleValue = transform.localScale.x;
        attackRateValue = attackRate;
        maxBossHealth = bossHealth;
        bossHealthSlider.maxValue = bossHealth;

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
     void Update()
    {
        chargeWaitTime -= Time.deltaTime;
        if (chargeWaitTime <= 0)
        {
            canCharge = true;
        }
    }
    void FixedUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < maxChaseRange && !isDead)
        {
            isChasing = true;
            if (canCharge)
            {
                isCharging = true;
                canCharge = false;
                chargeWaitTime = Random.Range(5, 12);
                anim.SetBool("Charge", true);
            }
            else
            {
                anim.SetBool("Charge", false);
            }

        }
        if (isChasing && !isDead&& !isCharging)
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
                if (directionOfPlayer.x> 0.2f)
                {
                    transform.localScale = new Vector3(xScaleValue, transform.localScale.y, 1);
                }
                else if (directionOfPlayer.x < 0.2f)
                {
                    transform.localScale = new Vector3(-xScaleValue, transform.localScale.y, 1);
                }
            }

        }
    }
    public void ChargeForce()//animation event de çağırlıyor
    {
        rb.AddForce((target.position - transform.position).normalized * chargeSpeed);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "WebBullet")
        {
            getDamage(10);
        }
         if (col.gameObject.tag == "Player" && !isDead && isCharging)
        {
            DamagePlayer(col.gameObject);
            isCharging = false;
        }
        else if (col.gameObject.tag == "Player" && !isDead)
        {
            anim.SetTrigger("Attack");
            canAttackDirectly = false;
        }

    }
    public void AttackEvent()
    {
        if (isDead) return;
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            DamagePlayer(colInfo.gameObject);
            colInfo.GetComponent<PlayerController>().GetPoisoned();
        }
    }
    void DamagePlayer(GameObject playerObject)
    {
        GameObject particle = Instantiate(bossAttackParticle, playerObject.transform.position, Quaternion.identity);
        Destroy(particle, 0.7f);
        playerObject.GetComponent<PlayerController>().UpdateHealth(30);
    }


    public void getDamage(float damage)
    {
        if (isDead || isInVulnearable || bossHealth < 0) { return; }
        if (bossHealth > 0)
        {
            bossHealth -= damage;
            bossHealthSlider.value = bossHealth;
        }
        //if(bossHealth<=maxBossHealth/2)
        //{
        //    this.gameObject.GetComponent<Animator>().SetBool("isEnrage", true);
        //}
        else
        {
            //boss dead animation sounds etc.
            Die();
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            //rb.isKinematic = true;
            bossHealthSlider.gameObject.SetActive(false);
        }
    }
    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        Destroy(gameObject, 3f);
        rb.gravityScale = 4;
    }
    public void FinishChargingEvent()//charge animation sonunda çağırıyoruz follow yapsın diye sonrasında
    {
        isCharging = false;
    }
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
