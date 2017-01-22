using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CompleteProject;

public class ItemInHandActivator : StateMachineBehaviour {
	private GameObject currentItem;
	Player playerScript;
	ClickToMove click;
//	  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		CheckWithItemToActivate(animator);
		click.ClearHandItem();
		currentItem.SetActive(true);
	}

	void CheckWithItemToActivate(Animator animator)
	{
		playerScript = animator.GetComponent<Player>();
		click = animator.GetComponent<ClickToMove>();

		if (animator.GetBool("Chopping"))
		{
			click.StartWork();
			currentItem = playerScript.itemHandle.Find("Axe").gameObject;
		}
		else if (animator.GetBool("Fishing"))
		{
			click.StartWork();
			currentItem = playerScript.itemHandle.Find("FishingRod").gameObject;
		}else
		{
			currentItem = playerScript.itemHandle.Find(click.resourceToCarryName).gameObject;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
