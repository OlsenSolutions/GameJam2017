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
	static float growthTime = 20;

	/// <summary>
	/// The growth time passed in seconds.
	/// </summary>
	public float growthTimePassed;

	void Awake ()
	{
		growthTimePassed =75.0f;
		growthTime=Random.Range (80.0f, 100.0f);
		stagesPrefab = transform.Find("Stages");

		for (int i = 0; i < stagesPrefab.childCount; i++)
		{
			treeStages.Add(stagesPrefab.Find((i+1).ToString()).gameObject);
		}
	}

	public void Reset()
	{
		growthTimePassed = 0;
		growthTime=Random.Range (50.0f, 100.0f);
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