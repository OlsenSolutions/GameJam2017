using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace CompleteProject
{

	public class ClickToMove : MonoBehaviour
	{
		public bool isWorking = false;

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
		public IStorable resourceBeingCollected;
		public GameObject targetStore;
		public float gatherWoodDistance = 3;
		public float FishingDistance = 6;
		public float StoreDistance = 10;



		void ResetAnimations()
		{
			anim.SetBool("Walking", false);
			anim.SetBool("Idle", false);
			anim.SetBool("Chopping", false);
			anim.SetBool("Fishing", false);
			//anim.SetBool ("Carry", false);
		}
		// Use this for initialization
		void Awake()
		{
			anim = GetComponent<Animator>();
			navMeshAgent = GetComponent<NavMeshAgent>();
		}

		private IEnumerator WaitForEndOfWork()
		{
			isWorking = true;
			float timeToComplete = 3f;
			float startTime = Time.time;

			while (Time.time - startTime < timeToComplete)
			{
				if (isWorking)
					yield return new WaitForEndOfFrame();
				else
					break;
			}
			ClearHandItem();
			if(Time.time - startTime >= timeToComplete)
				GatherResourcesWhenActionEnded();
		}

		public void StartWork()
		{
			StartCoroutine("WaitForEndOfWork");
		}

		public void GatherResourcesWhenActionEnded()
		{
			GetComponent<Player>().Compartment = resourceBeingCollected;
			resourceToCarryName = "Planks";
			if (anim.GetBool("Chopping"))
			{
				GetComponent<Player>().Compartment = resourceBeingCollected;
				resourceToCarryName = "Planks";
			}
			else if (anim.GetBool("Fishing"))
				GetComponent<Player>().Hunger += 50;

			ResetAnimations();
			anim.SetBool("Carry", true);

			//resourceBeingCollected.Collect();
			resourceBeingCollected = null;
			targetCollectible = null;
		}

		// Update is called once per frame
		void Update()
		{
			if (GameManager.Instance.SelectedPlayer == this.gameObject.GetComponent<Player>())
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Input.GetButtonDown("Fire1"))
				{
					targetCollectible = null;
					if (Physics.Raycast(ray, out hit, 500))
					{
						Debug.Log(hit.collider.gameObject.name);
						if (hit.collider.CompareTag("Player"))
						{
							GameManager.Instance.SelectedPlayer = hit.collider.gameObject.GetComponent<Player>();
						}
						else if (hit.collider.CompareTag("Ground"))
						{
							isWorking = false;

							navMeshAgent.destination = hit.point;
							navMeshAgent.Resume();
							ResetAnimations();
							navMeshAgent.destination = hit.point;
							navMeshAgent.Resume();
							//ResetAnimations ();
							anim.SetBool("Walking", true);
							anim.SetBool("Idle", false);
						}
						else if (hit.collider.CompareTag("Fish"))
						{
							isWorking = false;

							navMeshAgent.destination = hit.point;
							navMeshAgent.Resume();
							distanceToTarget = Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position);
//							if (distanceToTarget < FishingDistance) {
//								hit.collider.gameObject.GetComponent<Fish> ().Collect ();
//								GetComponent<Player> ().Hunger += 50;
//								targetCollectible = null;
//								navMeshAgent.Stop ();
//							} else {
							targetCollectible = hit.collider.gameObject.GetComponent<Fish>();
//							}
						
						}
						else if (hit.collider.CompareTag("Tree"))
						{
							isWorking = false;

							if (GameManager.Instance.SelectedPlayer.Compartment == null)
							{
								navMeshAgent.destination = hit.point;
								navMeshAgent.Resume();
								distanceToTarget = Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position);
//								if (distanceToTarget < gatherWoodDistance) {
//								
//									GetComponent<Player> ().Compartment = hit.collider.gameObject.GetComponent<Wood> ();
//									hit.collider.gameObject.GetComponent<Wood> ().Collect ();
//									navMeshAgent.Stop ();
//								} else {
								targetCollectible = hit.collider.gameObject.GetComponent<Wood>();
//								}
							}
						}
						else if (hit.collider.CompareTag("Storage"))
						{
							isWorking = false;

							if (GameManager.Instance.SelectedPlayer.Compartment != null)
							{
								navMeshAgent.destination = hit.point;
								navMeshAgent.Resume();
								distanceToTarget = Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position);
								if (distanceToTarget < StoreDistance)
								{	



									(gameObject.GetComponent<Player>().Compartment as IStorable).Store();
									gameObject.GetComponent<Player>().Compartment = null;
									navMeshAgent.Stop();
									ClearHandItem();
									anim.SetBool("Carry", false);
								}
								else
								{
									targetStore = hit.collider.gameObject;
								}
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
						//Debug.Log ("Finished");
						ResetAnimations();
						anim.SetBool("Idle", true);

						//if (anim.GetBool ("Chopping") || anim.GetBool ("Fishing") || anim.GetBool ("Walking")) {
						//Debug.Log ("Finished");
						//	ResetAnimations ();
						//	anim.SetBool ("Idle", true);
						//}

					}
				}
			}



			if ((targetCollectible as MonoBehaviour) != null)
			{
				//Debug.Log((targetCollectible as MonoBehaviour).gameObject.transform.position);

				distanceToTarget = Vector3.Distance(gameObject.transform.position, (targetCollectible as MonoBehaviour).gameObject.transform.position);
				if (targetCollectible is Fish)
				{
					if (distanceToTarget < FishingDistance)
					{
						resourceBeingCollected = targetCollectible as IStorable;
						ResetAnimations();
						anim.SetBool("Fishing", true);
						navMeshAgent.Stop();
					}
				}
				else if (targetCollectible is Wood)
				{
					if (distanceToTarget < gatherWoodDistance)
					{
						resourceBeingCollected = targetCollectible as IStorable;
						ResetAnimations();
						anim.SetBool("Chopping", true);
						navMeshAgent.Stop();
					}
				}
			}
			else if (targetStore != null)
			{
				distanceToTarget = Vector3.Distance(gameObject.transform.position, targetStore.transform.position);
				if (distanceToTarget < StoreDistance)
				{
					targetStore = null;
					navMeshAgent.Stop();
					GameManager.Instance.SelectedPlayer.Compartment.Store();
					ClearHandItem();
					anim.SetBool("Carry", false);
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

		public void ClearHandItem()
		{
			for (int i = 0; i < GameManager.Instance.SelectedPlayer.itemHandle.childCount; i++)
			{
				GameManager.Instance.SelectedPlayer.itemHandle.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}