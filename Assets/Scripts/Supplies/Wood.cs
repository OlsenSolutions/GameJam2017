using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour, ICollectible, IStorable {

	public void Collect()
	{
		GameManager.Instance.player.Compartment = this;
		Tree t=this.gameObject.GetComponent<Tree> ();
		if (t != null) {
			t.Reset ();
		}
	}

	public void Store()
	{
		GameManager.Instance.player.Compartment = null;
		GameManager.Instance.Wood+=3;
	}


}