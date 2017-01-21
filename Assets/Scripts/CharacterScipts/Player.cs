using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : CharacterBase {

	private IStorable compartment;
	public Image Container;

	void Start()
	{
		StartCoroutine (HungerStrikes ());
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
		}
	}
}
