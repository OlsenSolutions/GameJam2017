using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CompleteProject;
using UnityEngine.SceneManagement;

public  class GameManager : MonoBehaviour {

	void Awake()
	{
		
		//StartCoroutine ("HungerStrikes");

	}

	void Update()
	{


		woodCounter.text = ship.planksAddedNumber.ToString ();
		lifeSlider.value = selectedPlayer.Hunger;

		if (timePassing) {
			
			timeToNextWave -= Time.deltaTime;
			timecounter.text = ((int)timeToNextWave).ToString ();

			if (timeToNextWave <= 0) {
				Wave ();


			}
		} else {
			timeBetweenWaves -= Time.deltaTime;
			if (timeBetweenWaves <= 0) {
				timePassing = true;
				timeBetweenWaves = 5.0f;

			}
		}
	}

	void Start()
	{
		selectedPlayer.slider.gameObject.SetActive (true);
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
	[SerializeField]
	private Player selectedPlayer;
	public IClickable selected;
	public Ship ship;
	public GameObject wave;
	public float timeToNextWave=30;
	public bool timePassing=true;
	public Text timecounter;
	public Text woodCounter;
	public float timeBetweenWaves = 5.0f;
	public GameObject playerPrefab;
	public Slider lifeSlider;

	public Player SelectedPlayer
	{




		get{ return selectedPlayer;}
		set{
			if (selectedPlayer != null) {
				selectedPlayer.slider.gameObject.SetActive (false);
				selectedPlayer = value;
				selectedPlayer.slider.gameObject.SetActive (true);
				GameObject.FindObjectOfType<CameraFollow> ().target = selectedPlayer.transform;
			} else {
				//foreach(Player p in )
			}


		}
	}

	public int Wood
	{
		get{ return wood;}
		set{
			if (value < 0)
				value = 0;
			if (value > ship.maxPlanksNumber)
				value = ship.maxPlanksNumber;
			wood = value;
			ship.planksAddedNumber=value;

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
		GameManager.instance.Wood -= 5;
		timeToNextWave = 60.0f;
		//ship.planksAddedNumber = -5;
		Instantiate(playerPrefab,new Vector3(-40,-1,5), Quaternion.identity);
	}

	public void GameOver()

	{
		Debug.Log ("GameOver");
		SceneManager.LoadScene ("Menu");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}





}
