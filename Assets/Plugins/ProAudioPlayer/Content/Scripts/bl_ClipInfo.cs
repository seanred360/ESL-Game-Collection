using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_ClipInfo : MonoBehaviour {

    public AudioClip m_Clip = null;
    public Text ClipIDText = null;
    public Text ClipNameText = null;
    public Text ClipDurationText = null;

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clip"></param>
    public void GetInfo(AudioClip clip,int id)
    {
        m_Clip = clip;
        ClipNameText.text = m_Clip.name;
        ClipDurationText.text = bl_AudioPlayerUtils.TimeFormat(m_Clip.length, "#00:00");
        ClipIDText.text = id.ToString();
    }
    /// <summary>
    /// 
    /// </summary>
    public void PlayClip()
    {
        m_Player.NewClip(m_Clip,true);
    }

    private bl_AudioPlayer m_APlayer = null;
    private bl_AudioPlayer m_Player
    {
        get
        {
            if (m_APlayer == null)
            {
                m_APlayer = GameObject.Find(bl_AudioPlayer.AudioPlayerName).GetComponent<bl_AudioPlayer>();
            }
            return m_APlayer;
        }
    }
}