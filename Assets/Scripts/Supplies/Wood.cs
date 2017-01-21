using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour, ICollectible, IStorable {

	public void Collect()
	{
		GameManager.Instance.player.Compartment = this;
		GameObject.Destroy (this.gameObject);
	}

	public void Store()
	{
		GameManager.Instance.player.Compartment = null;
		GameManager.Instance.wood++;
	}
}