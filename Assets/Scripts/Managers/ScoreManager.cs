using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int woodLeft;


    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
		woodLeft = 0;
    }


    void Update ()
    {
		text.text = " " + woodLeft;
    }
}
