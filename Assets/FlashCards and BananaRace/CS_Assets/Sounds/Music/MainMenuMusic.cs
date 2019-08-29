using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    [Tooltip("The tag of the music source")]
    public string musicTag = "Music";

    [Tooltip("The time this instance of the music source has been in the game")]
    internal float instanceTime = 0;

    string currentScene;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the 
    /// lifetime of the script instance. Awake is called after all objects are initialized so you can safely speak to other 
    /// objects or query them using eg. GameObject.FindWithTag. Each GameObject's Awake is called in a random order between objects. 
    /// Because of this, you should use Awake to set up references between scripts, and use Start to pass any information back and forth. 
    /// Awake is always called before any Start functions. This allows you to order initialization of scripts. Awake can not act as a coroutine.
    /// </summary>
    void Awake()
    {
        //Find all the music objects in the scene
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag(musicTag);

        //Keep only the music object which has been in the game for more than 0 seconds
        if (musicObjects.Length > 1)
        {
            foreach (var musicObject in musicObjects)
            {
                if (musicObject.GetComponent<MainMenuMusic>().instanceTime <= 0) Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Start is only called once in the lifetime of the behaviour.
    /// The difference between Awake and Start is that Start is only called if the script instance is enabled.
    /// This allows you to delay any initialization code, until it is really needed.
    /// Awake is always called before any Start functions.
    /// This allows you to order initialization of scripts
    /// </summary>
    void Start()
    {
        //Don't destroy this object when loading a new scene
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // only play this music on the main menus
        if ((currentScene != "KBAMainMenu") && (currentScene != "KBBMainMenu") && (currentScene != "RootMenu"))
            Destroy(gameObject);
    }
}

