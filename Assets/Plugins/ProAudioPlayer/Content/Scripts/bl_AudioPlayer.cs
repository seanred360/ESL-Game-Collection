using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class bl_AudioPlayer : MonoBehaviour {

    [Header("Global")]
    public List<AudioClip> m_Clip = new List<AudioClip>();
    [Range(0.0f,1.0f)]
    public float m_Volumen = 1.0f;
    public bool SaveVolumen = true;
    public float LerpVolumen = 8.0f;
    public bool m_Repeat = false;
    [Header("Audio")]
    [SerializeField]private AudioMixerSnapshot NormalSnapShot;
    [SerializeField]private AudioMixerSnapshot SwitchSnapShot;
    [Header("UI")]
    public Image PlayPauseImage = null;
    public Sprite PlaySprite = null;
    public Sprite PauseSprite = null;
    [Space(5)]
    public Image MuteImage = null;
    public Sprite MuteSprite = null;
    public Sprite UnMuteSprite = null;
    [Space(5)]
    public Text CurrentTimeText = null;
    public Text DurationText = null;
    [Space(5)]
    public Slider ProgressSlider = null;
    public Slider VolumenSlider = null;
    [Space(5)]
    public Image RepeatImage = null;

    private bool ProgressSelect = false;
    private bool mMute = false;
    private int CurrentClip = 0;

    public const string AudioPlayerName = "Pro Audio Player";
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        this.gameObject.name = AudioPlayerName;
        //If have saved volumen
        if (PlayerPrefs.HasKey(VolumenKey))
        {
            m_Volumen = PlayerPrefs.GetFloat(VolumenKey);
            m_Source.volume = PlayerPrefs.GetFloat(VolumenKey);
            if (VolumenSlider != null)
            {
                VolumenSlider.value = PlayerPrefs.GetFloat(VolumenKey);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (m_Source.clip == null)
            return;

        if (m_Source.isPlaying)
        {
            TimePlayed();
        }
        if (m_Clip.Count > 1)
        {
            AutoChange();
        }
        VolumenControl();

    }
    /// <summary>
    /// 
    /// </summary>
    void AutoChange()
    {
        //When current clip if played, then continue when the next clip
        if (isDoneClip)
        {
            ChangeClip(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void TimePlayed()
    {
        if (CurrentTimeText != null)
        {
            float t = m_Source.time;
            CurrentTimeText.text = bl_AudioPlayerUtils.TimeFormat(t, "#00:00");
        }
        if (ProgressSlider != null )
        {
            if (ProgressSlider.maxValue != GetDuration())
            {
                ProgressSlider.maxValue = GetDuration();
            }
            //Just send value when slider is not select
            if (!ProgressSelect)
            {
                ProgressSlider.value = GetCurrentProgress();
            }

        }
    }
    /// <summary>
    /// 
    /// </summary>
    void VolumenControl()
    {
        if (m_Source.volume != m_Volumen)
        {
            m_Source.volume = Mathf.Lerp(m_Source.volume, m_Volumen, Time.deltaTime * LerpVolumen);

            if (VolumenSlider != null)
            {
                VolumenSlider.value = m_Source.volume;
            }
        }
    }
    /// <summary>
    /// Change the clip to next or previous
    /// </summary>
    /// <param name="forward"></param>
    public void ChangeClip(bool forward)
    {
        if (m_Clip.Count > 1 && !m_Repeat)
        {
            if (forward) { CurrentClip = (CurrentClip + 1) % m_Clip.Count; }
            else { if (CurrentClip != 0) { CurrentClip = (CurrentClip - 1) % m_Clip.Count; } else { CurrentClip = (m_Clip.Count - 1); } }

            NewClip(m_Clip[CurrentClip]);
            PlayPause();
            StopAllCoroutines();
            StartCoroutine(SwitchEffect());
        }
        else if (m_Repeat)
        {
            m_Source.time = 0.0f;
            m_Source.PlayDelayed(0.2f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Mute()
    {
        mMute = !mMute;
        m_Source.mute = mMute;
        if (mMute)
        {
            MuteImage.sprite = UnMuteSprite;
        }
        else
        {
            MuteImage.sprite = MuteSprite;
        }
    }
    /// <summary>
    /// When change the slider progress
    /// </summary>
    /// <param name="b"></param>
    public void OnSelectProgress(bool b)
    {
        ProgressSelect = b;
        //When change the slider progress
        if (!b)
        {
            //go to the correct value of slider with the song audio
            m_Source.time = ProgressSlider.value;
        }
    }
    /// <summary>
    /// Player event pause and play
    /// </summary>
    public void PlayPause()
    {
        if (m_Clip.Count <= 0 )
            return;

        if (m_Source.clip == null)
        {
            if (m_Clip != null) { NewClip(m_Clip[0]); } else { Debug.Log("Dont have clip yet!"); return; } 
        }

        bool p = m_Source.isPlaying;

        if (p)
        {
            m_Source.Pause();
            PlayPauseImage.sprite = PlaySprite;
        }
        else
        {
            m_Source.Play();
            PlayPauseImage.sprite = PauseSprite;
        }
    }
    /// <summary>
    /// Just Play current clip
    /// </summary>
    public void Play()
    {
        if (m_Clip.Count <= 0)
            return;

        if (m_Source.clip == null)
        {
            if (m_Clip != null) { NewClip(m_Clip[0]); } else { Debug.Log("Dont have clip yet!"); return; }
        }

        m_Source.Play();
        PlayPauseImage.sprite = PauseSprite;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mClip"></param>
    public void NewClip(AudioClip mClip,bool play = false)
    {
        m_Source.clip = mClip;
        if (DurationText != null)
        {
            DurationText.text = bl_AudioPlayerUtils.TimeFormat(mClip.length, "#00:00");
        }
        m_Source.time = 0.0f;

        if (m_Title != null) { m_Title.ChangeText(mClip.name); }
        if (play)
        {
            Play();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchEffect()
    {
        SwitchSnapShot.TransitionTo(0.5f);
        yield return new WaitForSeconds(0.5f);
        NormalSnapShot.TransitionTo(0.5f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Duration od Current Clip</returns>
    public float GetDuration() { float d = 0.0f; if (m_Source.clip != null) { d = m_Source.clip.length; } return d; }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Get the progress of audio playing</returns>
    public float GetCurrentProgress() { float p = m_Source.time; return p; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    public void onChangeVolumen(float v) { m_Volumen = v; if (SaveVolumen) { PlayerPrefs.SetFloat(VolumenKey, v); } }
    public const string VolumenKey = "APVolumen";
    /// <summary>
    /// 
    /// </summary>
    public void ChangeRepeat() { m_Repeat = !m_Repeat; RepeatImage.color = (m_Repeat) ? new Color32(50, 167, 236, 200) : new Color32(255,255,255,200); }
    /// <summary>
    /// 
    /// </summary>
    public bool isDoneClip
    {
        get
        {
            if (m_Source.clip == null)
                return false;

            int t = (int)GetCurrentProgress();
            int d = (int)GetDuration();

            if (t >= d)
            {
                return true;
            }

            return false;
        }
    }

    private AudioSource mSource = null;
    public AudioSource m_Source
    {
        get
        {
            if (mSource == null)
            {
                mSource = this.GetComponent<AudioSource>();
            }
            return mSource;
        }
    }

    private bl_ScrollTitle mTitle = null;
    public bl_ScrollTitle m_Title
    {
        get
        {
            if (mTitle == null)
            {
                mTitle = GetComponentInChildren<bl_ScrollTitle>();
            }
            return mTitle;
        }
    }
}