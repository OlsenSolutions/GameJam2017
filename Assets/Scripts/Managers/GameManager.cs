using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameManager : MonoBehaviour {

	void Awake()
	{
		
		//StartCoroutine ("HungerStrikes");

	}

	void Update()
	{
		if (timePassing) {
			timeToNextWave -= Time.deltaTime;
			if (timeToNextWave <= 0) {
				Wave ();


			}
		}
	}

	void Start()
	{
		
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



	//public int fishes=0;
	private int wood=0;
	public int waveNumber=0;
	public int boatPlanks =0;
	public Player player;
	public IClickable selected;
	public Ship ship;
	public GameObject wave;
	public float timeToNextWave=30;
	public bool timePassing=true;

	public int Wood
	{
		get{ return wood;}
		set{
			if (value < 0)
				value = 0;
			if (value > ship.maxPlanksNumber)
				value = ship.maxPlanksNumber;
			wood = value;
			ship.planksAddedNumber = wood;

		}
	}

	IEnumerator Tsunami()
	{
		for(int i=0;i<1000;i++)
		{
			wave.transform.position = new Vector3 (wave.transform.position.x + 0.5f, wave.transform.position.y, wave.transform.position.z );
			yield return new WaitForSeconds(0.1f);
			if (i == 1000) {
				timePassing = true;
			}
		}
	}

	void Wave()
	{
		timePassing = false;
		//StartCoroutine (Tsunami ());
		Tree[] trees=GameObject.FindObjectsOfType<Tree>();
		for (int i=0;i<trees.Length;i=i+2)
		{
			Tree t = trees[i] ;
			t.Reset ();
		}
		ship.planksAddedNumber = -5;
	}



}
