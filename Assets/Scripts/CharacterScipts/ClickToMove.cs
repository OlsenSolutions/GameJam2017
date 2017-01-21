using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace CompleteProject
{

	public class ClickToMove : MonoBehaviour {



		private Animator anim;
		private NavMeshAgent navMeshAgent;
		private Transform targetedEnemy;
		private Ray shootRay;
		private RaycastHit shootHit;
		private bool walking;
		private bool enemyClicked;


		// Use this for initialization
		void Awake () 
		{
			anim = GetComponent<Animator> ();
			navMeshAgent = GetComponent<NavMeshAgent> ();
		}

		// Update is called once per frame
		void Update () 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Input.GetButtonDown ("Fire1")) 
			{
				if (Physics.Raycast(ray, out hit, 500))
				{
					Debug.Log (hit.collider.gameObject.name);
					if (hit.collider.CompareTag("Ground"))
					{

						walking = true;

						navMeshAgent.destination = hit.point;
						navMeshAgent.Resume();
					}
					else 
						if (hit.collider.CompareTag("Fish"))
						{
							hit.collider.gameObject.GetComponent<Fish> ().Collect ();
						
						}
						else 
							if (hit.collider.CompareTag("Tree"))
							{
								hit.collider.gameObject.GetComponent<Wood> ().Collect ();

							}
							else 
								if (hit.collider.CompareTag("Storage"))
								{
									if(GameManager.Instance.player.Compartment!=null)
						(GameManager.Instance.player.Compartment as IStorable).Store ();
						GameManager.Instance.player.Compartment = null;
								}
				}
			}



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