using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish :  MonoBehaviour, ICollectible, IClickable  {

	public void Click()
	{
		Collect ();
	}

	public void Collect()
	{
		GameObject.Destroy (this.gameObject);

	}

}
