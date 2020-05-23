
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRunAnimation : StateMachineBehaviour
{

	public float speed;
	public float meleeAttackRange;
	public float grappleRange;
	Transform player;
	Rigidbody2D rb;
	Boss boss;
	BossGrapple bossGrapple;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("GrappleAttack");
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = animator.GetComponent<Rigidbody2D>();
		boss = animator.GetComponent<Boss>();
		bossGrapple = animator.GetComponentInChildren<BossGrapple>();
		var boxes = FindObjectsOfType<BossBox>();
		if (boxes != null)
		{
			foreach (BossBox box in boxes)
			{
				Destroy(box.gameObject);
			}
		}
		var webs = GameObject.FindGameObjectsWithTag("BossGrappleWeb");
		if (webs != null)
		{
			foreach (GameObject web in webs)
			{
				Destroy(web);
			}
		}

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.LookAtPlayer();

		Vector2 target = new Vector2(player.position.x, rb.position.y);
		Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
		rb.MovePosition(newPos);

		if (Vector2.Distance(player.position, rb.position) <= meleeAttackRange&& !boss.grappling)
		{

			animator.SetTrigger("Attack");
		}
		if (Vector2.Distance(player.position, rb.position) >= grappleRange)
		{
			boss.grappling = true;
			animator.SetTrigger("GrappleAttack");
			
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("Attack");
	}
}