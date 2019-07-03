using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SimpleSceneLoad : MonoBehaviour 
{

    [Header("Load Settings")]
    public string levelToLoad;
	public string lastScene;
	public bool useLoadingBar = true;
	public Slider loadingBar;
	public bool enableLoadingBarText = true;
	public Text loadingBarText;
    [Space(5)]

    [Header("Text Settings")]
    [SerializeField]
    private string loadingScreenText;
    [SerializeField]
    private Font textFont;
	[SerializeField]
	private Color fontColour;
    [SerializeField]
    private Text loadingHeader;
    [SerializeField]
    private Text loadPercentageText;
    [Space(5)]

    [Header("Transition Settings")]
    [Range(1, 5)]
    public float transitionTime;
	[SerializeField]
	private TransitionType type;
	[SerializeField]
    private bool useRandomImage;
    [SerializeField]
	private List<Sprite> transitionOverlayImages;
    [Space(5)]

    [Header("Image Overlay Settings")]
    [SerializeField]
	private Sprite loadingScreenImage;
	[SerializeField]
	private Sprite transitionScreenImage;
    [SerializeField]
    private Image transitionImage;
	[Space(10)]

    [Header("Gameobject Settings")]
    [SerializeField]
    private GameObject loadingUI;
    [SerializeField]
    private GameObject textOverlays;
	[SerializeField]
    private GameObject loadingBarHolder;

    public AudioClip soundLoad;
    public GameObject soundSource;
    public string soundSourceTag = "Sound";


    public enum TransitionType
    {
        TypeHorizontal,
        TypeVerticle,
        TypeRadial90,
        TypeRadial180,
        TypeRadial360

    };

	public enum OverlayType
	{
        Image,
        ImageTransition
	};

	private static SimpleSceneLoad instance;

    void Awake()
    {
		if (instance == null)
        {
			instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
			Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (!soundSource && GameObject.FindGameObjectWithTag(soundSourceTag)) soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);
        loadingUI.SetActive(false);

		if(useLoadingBar)
		{
			if(loadingBar == null || loadingBarHolder == null)
			{
				Debug.LogWarning("SSL - LOADING BAR/LOADING BAR HOLDER NOT REFERENCED!");
				return;
			}
			else
            {
				loadingBar.maxValue = 1;
                loadingBar.value = 0;
			}

		}
    }
       
    void StoreLastScene(string lastSceneName)
	{
		lastScene = lastSceneName;
	}

    public void LoadLevel(string lvlToLoad)
    {

        //storing a list of all loaded scenes to allow for future use
        //e.g. going back to previous scene
        if (lastScene == "")
		{
			lastScene = SceneManager.GetActiveScene().name;
		}
		else
		{
			StoreLastScene(levelToLoad);
		}
      
        levelToLoad = lvlToLoad;
      
        loadingUI.SetActive(true);
        textOverlays.SetActive(true);

        loadingHeader.font = textFont;
		loadingHeader.color = fontColour;

		loadPercentageText.font = textFont;
        loadPercentageText.font = textFont;

        GetTransitionOverlayType(type);



        loadingHeader.text = loadingScreenText;
		loadingHeader.color = fontColour;

        transitionImage.type = Image.Type.Filled;

        StartCoroutine(Load(lvlToLoad));
    }


	public void LoadPreviousScene()
	{
		if(lastScene == "")
		{
			Debug.LogErrorFormat("SimpleSceneLoad - ERROR: Couldn't load last scene. Does it exist?");
		}
		else
		{
			LoadLevel(lastScene);
		}

	}

    private void GetTransitionOverlayType(TransitionType transitionType)
    {
        switch (transitionType)
        {
            case TransitionType.TypeRadial360:
                transitionImage.fillMethod = Image.FillMethod.Radial360;
                break;
            case TransitionType.TypeRadial180:
                transitionImage.fillMethod = Image.FillMethod.Radial180;
                break;
            case TransitionType.TypeRadial90:
                transitionImage.fillMethod = Image.FillMethod.Radial90;
                break;
            case TransitionType.TypeVerticle:
                transitionImage.fillMethod = Image.FillMethod.Vertical;
                break;
            default:
                transitionImage.fillMethod = Image.FillMethod.Horizontal;
                break;
        }

    }

    private IEnumerator Load(string lvl)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(lvl);

       
		if(useLoadingBar)
		{
			loadPercentageText.text = "";
			loadingBarHolder.SetActive(true);
		}
		else
		{
			loadingBarHolder.SetActive(false);
		}
       
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

			if(useLoadingBar)
			{
				loadingBar.value = progress;
				loadingBarText.text = (progress * 100).ToString() + "%";
			}
			else
			{
				loadPercentageText.text = (progress * 100).ToString() + "%";
			}
            

            yield return null;
        }

        LoadCompleted();
        StopCoroutine("Load");
    }

    private bool transition = false;

    private void LoadCompleted()
    {
        textOverlays.SetActive(false);
        
        transition = true;

		if (useRandomImage)
        {
            int x = Random.Range(0, transitionOverlayImages.Count);
            transitionImage.sprite = transitionOverlayImages[x];
        }
        else
        {
            transitionImage.sprite = transitionScreenImage;
        }
        
        StartCoroutine(TransitionUI());
    }
    
    private IEnumerator TransitionUI()
    {
        float t = transitionTime;
        float maxTime = t;
      
        while(transition)
        {
            
			t -= 1 * Time.deltaTime;

            transitionImage.fillAmount = t / maxTime;
         
			if (transitionImage.fillAmount <= 0)
            {
                transition = false;
                loadingUI.SetActive(false);
            }

            yield return null;
        }
    }
}
