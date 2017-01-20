using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish :  MonoBehaviour, ICollectible {

	public void Collect()
	{
		GameManager.Instance.fishes++;
		GameObject.Destroy (this);

	}

}
