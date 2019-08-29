using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLoadButton : MonoBehaviour 
{

	private SimpleSceneLoad loadScript;
	private bool canLoad = true;
       
	private void Start()
	{
		loadScript = GameObject.FindGameObjectWithTag("SimpleSceneLoad").GetComponent<SimpleSceneLoad>();

		if(loadScript == null)
		{
			Debug.LogWarningFormat("SimpleLoadButton was unable to find loadScript!");
			canLoad = false;
			return;
		}
	}

	public void LoadScene(string scene)
	{
		if(canLoad)
		{
			loadScript.LoadLevel(scene);
		}

	}

	public void LoadPreviousScene()
	{
		if(canLoad)
		{
			loadScript.LoadPreviousScene();	
		}

	}


}
