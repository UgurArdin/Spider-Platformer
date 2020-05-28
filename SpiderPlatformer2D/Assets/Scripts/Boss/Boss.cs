using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

	public GameObject player;
    [HideInInspector]public bool grappling;
    [HideInInspector] public bool isFlipped = false;
    bool isInVulnearable = false;

    public Slider bossHealthSlider;
    public float bossHealth;
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;
    private float maxBossHealth;
    public GameObject bossAttackParticle;
    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    private void Start()
    {
        maxBossHealth = bossHealth;
        bossHealthSlider.maxValue = bossHealth;
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            GameObject bulletInstance = Instantiate(bossAttackParticle, player.transform.position, Quaternion.identity);
            colInfo.GetComponent<PlayerController>().UpdateHealth(attackDamage);
        }
    }

    public void EnragedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().UpdateHealth(enragedAttackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    public  Vector3 BossPos()
    {
        return transform.position;
    }
    public void getDamage()
    {
        if(isInVulnearable) { return; }
        if (bossHealth > 0)
        {
            bossHealth -= 100;
            bossHealthSlider.value = bossHealth;
        }
        //if(bossHealth<=maxBossHealth/2)
        //{
        //    this.gameObject.GetComponent<Animator>().SetBool("isEnrage", true);
        //}
        else
        {
            //boss dead animation sounds etc.
            GetComponent<Animator>().SetTrigger("Die");
        }
    }

}