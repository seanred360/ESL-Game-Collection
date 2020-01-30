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
        {
            SceneManager.LoadSceneAsync("OptionsUI", LoadSceneMode.Additive);
            Time.timeScale = 0;
        }

        else
        {
            SceneManager.UnloadSceneAsync("OptionsUI");
            Time.timeScale = 1;
        }
    }
}