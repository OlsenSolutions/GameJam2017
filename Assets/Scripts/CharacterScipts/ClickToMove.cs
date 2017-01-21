using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace CompleteProject
{

	public class ClickToMove : MonoBehaviour {

		public string resourceToCarryName = "";
		private Animator anim;
		private NavMeshAgent navMeshAgent;
		private Transform targetedEnemy;
		private Ray shootRay;
		private RaycastHit shootHit;
		private bool walking;
		private bool enemyClicked;
		public float distanceToTarget = 0;
		public ICollectible targetCollectible;
		public ICollectible resourceBeingCollected;
		public GameObject targetStore;
		public float gatherWoodDistance=3;
		public float FishingDistance=6;
		public float StoreDistance=6;



		void ResetAnimations()
		{
			anim.SetBool ("Walking", false);
			anim.SetBool ("Idle", false);
			anim.SetBool ("Chopping", false);
			anim.SetBool ("Fishing", false);
			//anim.SetBool ("Carry", false);
		}
		// Use this for initialization
		void Awake () 
		{
			anim = GetComponent<Animator> ();
			navMeshAgent = GetComponent<NavMeshAgent> ();
		}

		public void GatherResourcesWhenActionEnded()
		{
			if (anim.GetBool("Chopping"))
				resourceToCarryName = "Planks";

			ResetAnimations();
			anim.SetBool("Carry", true);

			resourceBeingCollected.Collect ();
			resourceBeingCollected = null;
			targetCollectible = null;
		}

		// Update is called once per frame
		void Update () 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Input.GetButtonDown ("Fire1")) 
			{
				targetCollectible = null;
				if (Physics.Raycast (ray, out hit, 500)) {
					//Debug.Log (hit.collider.gameObject.name);
					if (hit.collider.CompareTag ("Ground")) {
						navMeshAgent.destination = hit.point;
						navMeshAgent.Resume ();
						ResetAnimations ();
						anim.SetBool ("Walking", true);
					} else if (hit.collider.CompareTag ("Fish")) {
							
						navMeshAgent.destination = hit.point;
						navMeshAgent.Resume ();
						distanceToTarget = Vector3.Distance (gameObject.transform.position, hit.collider.gameObject.transform.position);
//						if (distanceToTarget < FishingDistance) {
//							hit.collider.gameObject.GetComponent<Fish> ().Collect ();
//							targetCollectible = null;
//							navMeshAgent.Stop ();
//						} else {
							targetCollectible = hit.collider.gameObject.GetComponent<Fish> ();
//						}
						
					} else if (hit.collider.CompareTag ("Tree")) {
						if (GameManager.Instance.player.Compartment == null) {
							navMeshAgent.destination = hit.point;
							navMeshAgent.Resume ();
							distanceToTarget = Vector3.Distance (gameObject.transform.position, hit.collider.gameObject.transform.position);
//							if (distanceToTarget < gatherWoodDistance) {
//								hit.collider.gameObject.GetComponent<Wood> ().Collect ();
//								navMeshAgent.Stop ();
//							} else {
								targetCollectible = hit.collider.gameObject.GetComponent<Wood> ();
//							}
						}
					} else if (hit.collider.CompareTag ("Storage")) {
						if (GameManager.Instance.player.Compartment != null) {
							navMeshAgent.destination = hit.point;
							navMeshAgent.Resume ();
							distanceToTarget = Vector3.Distance (gameObject.transform.position, hit.collider.gameObject.transform.position);
							if (distanceToTarget < StoreDistance) {
								(GameManager.Instance.player.Compartment as IStorable).Store ();
								GameManager.Instance.player.Compartment = null;
								navMeshAgent.Stop ();
							} else {
								targetStore = hit.collider.gameObject;

							}
							
						}
					}
				}
			}

			if (!navMeshAgent.pathPending)
			{
				if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
				{
					if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
					{
						Debug.Log ("Finished");
						ResetAnimations();
						anim.SetBool ("Idle", true);

						//if (anim.GetBool ("Chopping") || anim.GetBool ("Fishing") || anim.GetBool ("Walking")) {
						Debug.Log ("Finished");
						//	ResetAnimations ();
						//	anim.SetBool ("Idle", true);
						//}

					}
				}
			}



			if ((targetCollectible as MonoBehaviour) != null) {
				//Debug.Log((targetCollectible as MonoBehaviour).gameObject.transform.position);

				distanceToTarget = Vector3.Distance (gameObject.transform.position, (targetCollectible as MonoBehaviour).gameObject.transform.position);
				if (targetCollectible is Fish) {
					if (distanceToTarget < FishingDistance) {
						resourceBeingCollected = targetCollectible;
						ResetAnimations();
						anim.SetTrigger ("Fishing");
						navMeshAgent.Stop ();
					}
				}
				else if(targetCollectible is Wood)
				 {
					if (distanceToTarget < gatherWoodDistance) {
						resourceBeingCollected = targetCollectible;
						ResetAnimations();
						anim.SetTrigger ("Chopping");
						navMeshAgent.Stop ();
					}
				}
			} else if (targetStore != null) {
				distanceToTarget = Vector3.Distance (gameObject.transform.position, targetStore.transform.position);
				if (distanceToTarget < StoreDistance) {
					targetStore = null;
					navMeshAgent.Stop ();
					GameManager.Instance.player.Compartment.Store ();
				}
			}



			/*
			if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete) {
				anim.SetBool ("Walking", false);
				if (!anim.GetBool ("Chopping") && !anim.GetBool ("Fishing")) {
					anim.SetBool ("Idle", true);
				}
			}
			*/

/*			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
				if (!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
					walking = false;
			} else {
				walking = true;
			}

			anim.SetBool ("IsWalking", walking);
			*/
		}


	}

}