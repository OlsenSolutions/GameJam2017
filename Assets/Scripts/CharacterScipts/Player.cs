using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : CharacterBase {

	private IStorable compartment;
	public Image Container;
	public Animator animator;

	public Transform itemHandle;

	void Start()
	{
		slider.gameObject.SetActive (false);
		StartCoroutine (HungerStrikes ());
		animator = GetComponent<Animator> ();
	}



	public IStorable Compartment
	{
		get{
			return compartment;

		}

		set {
			if (value is Wood) {
				Container.color = Color.green;
				compartment = value;
			} else if (value == null) {
				Container.color = new Color (255, 255, 255, 0);
				compartment = value;

			}
		}
	}

	IEnumerator HungerStrikes()
	{
		for(;;) {
			GetHungry (1);
			yield return new WaitForSeconds(1.0f);
			if (Hunger <= 0)
				Die ();
			Hunger = 100;
		}
	}

	void Die()
	{
		Debug.Log ("Die");
		animator.SetTrigger ("Die");
	}
}
