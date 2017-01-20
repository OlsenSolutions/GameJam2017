using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour, IClickable {

	[Range(0,100)]
	private int hunger=100;

	 
	
	public int Hunger {
		get{return hunger;}
		
		set{
			if(value<=0)
			{
				hunger=value;
				SetSlider();

			}
			else
			{
				
				hunger=value;
				SetSlider();
				
			}
		}
	}

	public void Click()
	{

	}

	

	
	public void GetHungry(int n)
	{
		Hunger -= n;
		SetSlider();
	}

	public Slider slider;
	public Image fillImage;
	

	
	
	void Awake()
	{
		if (slider != null) {
			SetSlider ();
			fillImage.enabled = false;
		}
	}

	public void SetSlider()
	{
		if (slider != null) {
			slider.value = hunger;
			fillImage.color = Color.Lerp (Color.red, Color.green, hunger / 100.0f);
		}
	}
}
