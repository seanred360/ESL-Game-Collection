using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

/// <summary>
/// UIDojoSelector Class just handles the Calling of a Method That increments a static variable and writes it to PlayerPrefs.
/// </summary>
public class UIDojoSelector : MonoBehaviour
{

    /// <summary>
    /// this method is called by a button on the menu/settings canvas to cycle forward through the dojo's.
    /// </summary>
    public void RunChangeNext()
    {
        #if UNITY_5_3_OR_NEWER

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SelectDojoBackground.instance.ChangeDojoBGNext();

        }
        #else
        if(Application.loadedLevel != 0)
        {
            SelectDojoBackground.instance.ChangeDojoBGNext();
        }
        #endif
    }


    /// <summary>
    /// this method is called by a button on the menu/settings canvas to cycle backwards through the dojo's.
    /// </summary>
    public void RunChangePrevious()
    {
        #if UNITY_5_3_OR_NEWER

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SelectDojoBackground.instance.ChangeDojoBGPrevious();

        }
        #else
        if(Application.loadedLevel != 0)
        {
            SelectDojoBackground.instance.ChangeDojoBGPrevious();

        }
        #endif
    }
}
