using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveTimeManager : MonoBehaviour
{
    public static float waveTimer;


    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
		waveTimer = 0;
    }


    void Update ()
    {
		text.text = " " + waveTimer;
    }
}
