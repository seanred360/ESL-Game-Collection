using UnityEngine;
using System.Collections;

[System.Serializable]
public class bl_APAudioWeb  {

    public string AudioTitle = "Title";
    public string URL = "";
    public AudioType m_AudioType = AudioType.OGGVORBIS;

    //This not need fill in editor.
    [HideInInspector] public AudioClip CacheAudio = null;
    [HideInInspector] public bool isDownloaded = false;
}