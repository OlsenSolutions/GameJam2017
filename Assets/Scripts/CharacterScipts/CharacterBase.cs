using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour, ISelectable, IDamageable {

	[Range(0,100)]
	private int hp=100;
	
	
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
	

	
	public void TakeDamage(int n)
	{
		Hp -= n;
		
	}
	
	public void Die()
	{
		
	}

	public void Select()
	{
		if (GameManager.Instance.selectedCharacter != gameObject.GetComponent<CharacterBase> ()) {
			if(GameManager.Instance.selectedCharacter!=null)
			GameManager.Instance.selectedCharacter.Deselect();
			fillImage.enabled = true;
			GameManager.Instance.selectedCharacter = gameObject.GetComponent<CharacterBase>();
		}

		
	}

	public void Deselect ()

	{
		fillImage.enabled = false;
		GameManager.Instance.selectedCharacter = null;
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
