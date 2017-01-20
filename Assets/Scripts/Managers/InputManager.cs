using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if(hitInfo.transform!=null)
			if(hitInfo.transform.gameObject.GetComponent<CharacterBase>()!=null)
				
				{
				CharacterBase character=hitInfo.transform.gameObject.GetComponent<CharacterBase>();
				character.Select();
				character.Hp-=5;

				}
		}

		if (Input.GetMouseButtonUp (1)) {
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if(hitInfo.transform!=null)
				//if(hitInfo.transform.gameObject.layer==LayerMask.NameToLayer("Enemy"))
				if(hitInfo.transform.gameObject.GetComponent<CharacterBase>()!=null)
					
			{
				CharacterBase character=hitInfo.transform.gameObject.GetComponent<CharacterBase>();
				
				character.Hp+=5;
			}
		}
	}
}
