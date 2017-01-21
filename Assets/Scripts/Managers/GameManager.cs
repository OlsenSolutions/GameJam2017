using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameManager : MonoBehaviour {

	void Awake()
	{
		
		//StartCoroutine ("HungerStrikes");

	}

	private static GameManager instance;
	public static GameManager Instance
	{
		get{
			if(instance==null)
			{
				instance=GameObject.FindObjectOfType<GameManager>() as GameManager;
			}
			return instance;

		}
	}

	public int fishes=0;
	public int wood=0;
	public int waveNumber=0;
	public int boatPlanks =0;
	public Player player;
	public IClickable selected;



}
