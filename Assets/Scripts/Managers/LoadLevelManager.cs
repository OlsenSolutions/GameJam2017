﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadLevelGame ()

	{
		Application.LoadLevel ("game");

	}
		

	public void LoadLevelMenu ()

	{
		Application.LoadLevel ("menu");

	}
}
