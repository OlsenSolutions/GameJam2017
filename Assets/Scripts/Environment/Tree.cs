using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	public bool isFullyGrown = false;

	private Transform stagesPrefab;
	private List<GameObject> treeStages = new List<GameObject>();

	/// <summary>
	/// The growth time in seconds.
	/// </summary>
	static float growthTime = 10;

	/// <summary>
	/// The growth time passed in seconds.
	/// </summary>
	public float growthTimePassed = 0;

	void Awake ()
	{
		stagesPrefab = transform.Find("Stages");

		for (int i = 0; i < stagesPrefab.childCount; i++)
		{
			treeStages.Add(stagesPrefab.Find((i+1).ToString()).gameObject);
		}
	}

	void Update()
	{
		if(!isFullyGrown)
			UpdateTreeSize();
	}

	void UpdateTreeSize()
	{
		if (growthTimePassed < growthTime)
			growthTimePassed += Time.deltaTime;
		else
		{
			isFullyGrown = true;
			return;
		}

		float stepLifeTime = growthTime / (treeStages.Count-1);
		int treeId = (int)(growthTimePassed/stepLifeTime);

		for (int i = 0; i < treeStages.Count; i++)
		{
			treeStages[i].SetActive(treeId == i ? true : false);
		}
	}
}