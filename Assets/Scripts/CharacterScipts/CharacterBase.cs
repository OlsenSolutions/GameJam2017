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
				value = 0;
				hunger=value;
			
				SetSlider();


			}
			else
			{
				
				hunger=value;
				if (hunger > 100)
					hunger = 100;
				SetSlider();
				
			}
		}
	}


	public void Click()
	{

	}

	public  void Die()
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
			fillImage.enabled = true;
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
