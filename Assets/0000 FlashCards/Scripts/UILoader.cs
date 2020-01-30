using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetSceneByName("OptionsUI").isLoaded)
            SceneManager.UnloadSceneAsync("OptionsUI");
    }

    public void ToggleOptionsUI()
    {
        if (SceneManager.GetSceneByName("OptionsUI").isLoaded == false)
            SceneManager.LoadSceneAsync("OptionsUI", LoadSceneMode.Additive);
        else
            SceneManager.UnloadSceneAsync("OptionsUI");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (SceneManager.GetSceneByName("OptionsUI").isLoaded == false)
                SceneManager.LoadSceneAsync("OptionsUI", LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("OptionsUI");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (SceneManager.GetSceneByName("Level2").isLoaded)
                SceneManager.UnloadSceneAsync("Level2");

            if (SceneManager.GetSceneByName("Level1").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive).completed += operation => 
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (SceneManager.GetSceneByName("Level1").isLoaded)
                SceneManager.UnloadSceneAsync("Level1");

            if (SceneManager.GetSceneByName("Level2").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive)
                    .completed += HandleLevel2LoadCompleted;
            }
        }
    }

    private void HandleLevel2LoadCompleted(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level2"));
    }
}