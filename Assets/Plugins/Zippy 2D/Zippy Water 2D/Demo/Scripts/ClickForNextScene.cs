using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickForNextScene : MonoBehaviour {
	int lvl;
	public static ClickForNextScene instance;				

	void OnApplicationQuit() {
		instance = null;
	}

	void Start() {
		if (instance) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Update() {
		if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > 1) {
			if(SceneManager.sceneCountInBuildSettings == 0) {
				Debug.LogWarning("No scenes to load: No scenes in Build Settings.");
				return;
			}
			lvl++;
			lvl = lvl % SceneManager.sceneCountInBuildSettings;
			SceneManager.LoadScene(lvl);
		}
	}
}
