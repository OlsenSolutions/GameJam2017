using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	public int planksAddedNumber = 0;
	private int lastPlanksAddedNumber = 0;

	public int maxPlanksNumber;

	private List<GameObject> addedPlanksGOs = new List<GameObject>();
	private List<GameObject> notAddedPlanksGOs = new List<GameObject>();

	private Transform planksParent;


	void Awake()
	{
		planksParent = transform.Find("Planks");
		maxPlanksNumber = planksParent.childCount;
		lastPlanksAddedNumber = planksAddedNumber;

		for (int i = 0; i < maxPlanksNumber; i++)
		{
			notAddedPlanksGOs.Add(planksParent.GetChild(i).gameObject);
			planksParent.GetChild(i).gameObject.SetActive(false);
		}

	}

	void Update()
	{
		planksAddedNumber = Mathf.Clamp(planksAddedNumber, 0, maxPlanksNumber);

		if (planksAddedNumber > lastPlanksAddedNumber)
		{
			for (int i = 0; i < planksAddedNumber - lastPlanksAddedNumber; i++)
			{
				int id = Random.Range(0, notAddedPlanksGOs.Count);
				addedPlanksGOs.Add(notAddedPlanksGOs[id]);
				notAddedPlanksGOs[id].SetActive(true);
				notAddedPlanksGOs.RemoveAt(id);
			}
		}
		else if (planksAddedNumber < lastPlanksAddedNumber)
		{
			for (int i = 0; i < lastPlanksAddedNumber - planksAddedNumber; i++)
			{
				int id = Random.Range(0, addedPlanksGOs.Count);
				notAddedPlanksGOs.Add(addedPlanksGOs[id]);
				addedPlanksGOs[id].SetActive(false);
				addedPlanksGOs.RemoveAt(id);
			}
		}

		lastPlanksAddedNumber = planksAddedNumber;
	}
}
