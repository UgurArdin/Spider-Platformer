using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrappleAnimation : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    public float speed;
    Boss boss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boss = animator.GetComponent<Boss>();
        rb = animator.GetComponent<Rigidbody2D>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.grappling = false;
   
    }


}
