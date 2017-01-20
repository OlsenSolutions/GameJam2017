using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour, ISelectable, IDamageable {

	[Range(0,100)]
	private int hp=100;
	private int hunger=0;
	 
	
	public int Hp {
		get{return hp;}
		
		set{
			if(value<=0)
			{
				hp=value;
				SetSlider();
				Die();
			}
			else
			{
				
				hp=value;
				SetSlider();
				
			}
		}
	}


	public int Hunger {
		get{return hunger;}

		set{
			if(value>=100)
			{
				hunger=value;
				//SetSlider();
				Die();
			}
			else
			{

				hunger=value;
				//SetSlider();

			}
		}
	}
	

	
	public void TakeDamage(int n)
	{
		Hp -= n;
		
	}
	
	public void Die()
	{
		
	}

	public void Select()
	{
		

		
	}

	public void Deselect ()

	{
		fillImage.enabled = false;

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
			slider.value = hp;
			fillImage.color = Color.Lerp (Color.red, Color.green, hp / 100.0f);
		}
	}
}
