using UnityEngine;


public class bl_APlayList : MonoBehaviour {
  
    public GameObject PlayPrefab = null;
    public Transform PlayListPanel = null;
    public Animation PlayListAnimation = null;
    //Call all script when new clip is added
    public delegate void NewClip(AudioClip clip);
    public static NewClip OnNewClip;

    private int ClipsCount = 0;
    private bl_AudioPlayer m_APlayer = null;

    void Awake()
    {
        m_APlayer = GetComponent<bl_AudioPlayer>();
    }

    // Use this for initialization
    void Start()
    {
        if (m_APlayer == null)
        {
            return;
        }

        for (int i = 0; i < m_APlayer.m_Clip.Count; i++)
        {
            InstanceInList(m_APlayer.m_Clip[i]);
        }
        //Register in event
        OnNewClip += InstanceInList;
    }
    /// <summary>
    /// add UI in PlayList
    /// </summary>
    /// <param name="clip"></param>
    public void InstanceInList(AudioClip clip)
    {
        //Clip ID
        ClipsCount++;
        GameObject o = Instantiate(PlayPrefab) as GameObject;
        o.name = clip.name;
        o.transform.SetParent(PlayListPanel, false);
        bl_ClipInfo ci = o.GetComponent<bl_ClipInfo>();
        ci.GetInfo(clip, ClipsCount);  
    }
    /// <summary>
    /// Add new clip to the play list event
    /// </summary>
    /// <param name="clip">new clip</param>
    public static void AddNewClip(AudioClip clip)
    {
        if (OnNewClip != null)
            OnNewClip(clip);
    }
    /// <summary>
    /// 
    /// </summary>
    private bool m_show = true;
    public void ShowHidePlayList()
    {
        if (PlayListAnimation == null)
            return;

        m_show = !m_show;
        if (m_show)
        {
            PlayListAnimation["ShowHidePlayList"].speed = 1.0f;
            PlayListAnimation.CrossFade("ShowHidePlayList", 0.2f);
        }
        else
        {
            if (PlayListAnimation["ShowHidePlayList"].time == 0.0f)
            {
                PlayListAnimation["ShowHidePlayList"].time = PlayListAnimation["ShowHidePlayList"].length;
            }

            PlayListAnimation["ShowHidePlayList"].speed = -1.0f;
            PlayListAnimation.CrossFade("ShowHidePlayList", 0.2f);
        }
    }
}