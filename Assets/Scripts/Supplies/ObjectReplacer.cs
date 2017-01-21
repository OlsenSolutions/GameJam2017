#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


public class ObjectReplacer : ScriptableWizard 
{
	public Object replaceWith;

	public bool _position = true;
	public bool _rotation = true;
	public bool _size = true;
	public bool keepParenting = true;

	public GameObject[] objectsToReplace;


	
	[MenuItem("Edit/Replace")]
	//[MenuItem("GameObject/Replace", false, 0)]
	public static void OpenReplaceWizard()
	{
		ScriptableWizard.DisplayWizard("Replace Objects", typeof( ObjectReplacer ), "Replace" );
	}
	
	public void OnWizardUpdate( )
	{
		if ( Selection.objects.Length > 0 )
		{
			objectsToReplace = Selection.gameObjects;
		} else {
			Debug.Log ("No Objects Selected");
			objectsToReplace = new GameObject[0];
		}
	}
	string path;

	public void OnWizardCreate( )	
	{
		if(replaceWith == null)
		{
			Debug.Log ("Replacement not assigned, aborting.");
			return;
		}

		for(int i = 0; i < objectsToReplace.Length; i++)
		{
			GameObject replacement = PrefabUtility.InstantiatePrefab(replaceWith) as GameObject;

			if(_position)
				replacement.transform.position = objectsToReplace[i].transform.position;
			if(_rotation)
				replacement.transform.rotation = objectsToReplace[i].transform.rotation;
			if(_size)
				replacement.transform.localScale = objectsToReplace[i].transform.localScale;

			if(keepParenting && objectsToReplace[i].transform.parent != null)
			{
				replacement.transform.parent = objectsToReplace[i].transform.parent;
			}

			DestroyImmediate (objectsToReplace[i]);
		}
	}
}
#endif




























