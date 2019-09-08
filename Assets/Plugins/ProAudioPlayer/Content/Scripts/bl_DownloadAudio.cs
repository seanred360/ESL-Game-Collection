using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class bl_DownloadAudio : MonoBehaviour {

    public List<bl_APAudioWeb> AudioURLs = new List<bl_APAudioWeb>();
    public bool PlayOnLoad = true;
    [Header("UI")]
    public GameObject RootDownloadUI = null;
    public Slider DownloadProgress = null;
    public Text DownloadText = null;

    private int CurrentDownload = -1;
    private bool isDownloading = false;
    private bl_AudioPlayer m_APlayer = null;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        m_APlayer = GetComponent<bl_AudioPlayer>();
    }

    /// <summary>
    /// 
    /// </summary>
    void CheckToDownload()
    {
        if (isDownloading)
            return;
        if(AudioURLs.Count <= 0)
            return;

        if (CurrentDownload + 1 <= AudioURLs.Count - 1)
        {
            CurrentDownload++;
        }
        else { RootDownloadUI.SetActive(false);  return; }

        StartCoroutine(DownLoadAudio(AudioURLs[CurrentDownload]));
    }
    /// <summary>
    /// Download all audio in list 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator DownLoadAudio(bl_APAudioWeb info)
    {
        isDownloading = true;
        using (var www = UnityWebRequestMultimedia.GetAudioClip(info.URL, info.m_AudioType))
        {
            www.SendWebRequest();
            //check if url have a response (audio)
            if (www.error != null)
            {
                Debug.LogWarning(www.error);
                DownloadText.text = www.error;
                isDownloading = false;
                yield break;
            }
            //while downloading
            while (!www.isDone)
            {
                if (DownloadProgress != null)
                {
                    DownloadProgress.value = www.downloadProgress;
                }
                DownloadText.text = "Downloading " + info.AudioTitle + " " + (www.downloadProgress * 100).ToString("00") + "%...";
                //stop in bucle for update progress
                yield return null;
            }
            //create a new audio
            AudioClip c = null;
            //when download is download
            if (www.isDone || www.downloadProgress == 1)
            {
                c = DownloadHandlerAudioClip.GetContent(www);
                c.name = AudioURLs[CurrentDownload].AudioTitle;

                //Just play when load, if not playing and not have a clip 
                if (!m_APlayer.m_Source.isPlaying)
                {
                    m_APlayer.NewClip(c);
                }
                //add to play list

                m_APlayer.m_Clip.Add(c);
                if (bl_APlayList.OnNewClip != null)
                    bl_APlayList.OnNewClip(c);
            }

            //cache the current download audio
            AudioURLs[CurrentDownload].CacheAudio = c;
            DownloadText.text = "Done!";
            //if play on load
            if (PlayOnLoad && !m_APlayer.m_Source.isPlaying) { m_APlayer.PlayPause(); }
            isDownloading = false;
            AudioURLs[CurrentDownload].isDownloaded = true;
            yield return new WaitForSeconds(0.5f);
            //go to download the next audio if exist.
            CheckToDownload();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartDownload() { CheckToDownload(); }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisabledInTime(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        obj.SetActive(false);
    }
}