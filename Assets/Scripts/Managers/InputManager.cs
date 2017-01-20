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
			if(hitInfo.transform.gameObject.GetComponent<MonoBehaviour>()!=null)
			{
				MonoBehaviour[] list = hitInfo.transform.gameObject.GetComponents<MonoBehaviour>();
				foreach(MonoBehaviour mb in list)
				{
					if (mb is ISelectable)
					{
						ISelectable selected = mb as ISelectable;
						selected.Select ();
					}
				}


			}
		}

		if (Input.GetMouseButtonUp (1)) {
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if(hitInfo.transform!=null)
				//if(hitInfo.transform.gameObject.layer==LayerMask.NameToLayer("Enemy"))
			if(hitInfo.transform.gameObject.GetComponent<MonoBehaviour>()!=null)
					
			{
				CharacterBase character=hitInfo.transform.gameObject.GetComponent<CharacterBase>();
				
				character.Hp+=5;
			}
		}
	}
}
