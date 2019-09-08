using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class bl_SpectrumAudio : MonoBehaviour
{
    [HideInInspector]
    public List<bl_SpectrumUI> m_Recs = new List<bl_SpectrumUI>();
    [Header("Channel Settings")]
    [Range(32, 256)]public int m_Samples = 64;
    [Range(0, 1)]public int m_Channel = 1;
    [Range(0, 25)]public float Space = 1;
    public FFTWindow FTT = FFTWindow.BlackmanHarris;
    [Header("Spectrum Settings")]
    public Color SpectrumColor = new Color(0.9f, 0.9f, 1, 1);
    [Range(5, 100)]public float MaxOutPout = 20;
    [Range(0.01f, 30)]public float DownLerp = 12;
    [Range(1, 30)]public float UpLerp = 12;
    [Range(0.001f, 0.5f)]public float Rate = 0.1f;
    [Range(1, 10)]public float Multiplier = 2;
    [Header("References")]
    public GameObject SpectrumPrefab;
    public Transform SpectrumPanel;
    public RectTransform RootSpectrumUI;

    private float NextRate;

    // Use this for initialization
    void Start()
    {
        InstanceSpectrum();
    }
    /// <summary>
    /// 
    /// </summary>
    void InstanceSpectrum()
    {
        //Calculate the size of each UI bar
        float sf = ((RootSpectrumUI.sizeDelta.x - (Space * m_Samples)) / m_Samples);
        //start position
        float sp = -sf;

        for (int i = 0; i < m_Samples; i++)
        {
            //next position
            sp += sf + Space;
            GameObject s = Instantiate(SpectrumPrefab) as GameObject;
            s.name = string.Format("Spectrum[{0}]", i + 1);
            s.transform.SetParent(SpectrumPanel, false);

            bl_SpectrumUI r = s.GetComponent<bl_SpectrumUI>();
            r.SpectrumColor = SpectrumColor;
            m_Recs.Add(r);

            Vector2 v = r.anchoredPosition;
            v.x = sp;
            r.RootRect.anchoredPosition = v;

            Vector2 size = r.sizeDelta;
            size.x = sf;
            r.RootRect.sizeDelta = size;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {

        //Get spectrum data samples from audio listener
        float[] data = new float[m_Samples];
        AudioListener.GetSpectrumData(data, m_Channel, FTT);

        if (Time.time >= NextRate)
        {
            //apply to the bars ui
            for (int i = 0; i < m_Recs.Count; i++)
            {
                bl_SpectrumUI spectrum = m_Recs[i];
                Vector2 v = spectrum.sizeDelta;
                Vector2 vs = spectrum.anchoredPosition;

                v.y = ((data[i] * (RootSpectrumUI.sizeDelta.y * 0.5f)) * Multiplier);
                vs = spectrum.anchoredPosition;
                vs.y = 0 + ((data[i] * RootSpectrumUI.sizeDelta.y * 0.5f));

                vs.y = 0;
                v.y = Mathf.Clamp(v.y, 0, MaxOutPout * Multiplier);
                NextRate = Time.time + Rate;

                spectrum.anchoredPosition = Vector2.Lerp(spectrum.anchoredPosition, vs, Time.deltaTime * UpLerp);
                spectrum.sizeDelta = Vector2.Lerp(spectrum.sizeDelta, v, Time.deltaTime * UpLerp);
            }
            NextRate = Time.time + Rate;
        }
        else
        {
            //apply to the bars ui
            for (int i = 0; i < m_Recs.Count; i++)
            {
                bl_SpectrumUI spectrum = m_Recs[i];
                Vector2 v = spectrum.sizeDelta;
                Vector2 vs = spectrum.anchoredPosition;

                v.y = Mathf.Lerp(v.y, 0, Time.deltaTime * DownLerp);
                vs.y = Mathf.Lerp(vs.y, 0, Time.deltaTime * DownLerp);
                spectrum.anchoredPosition = vs;
                spectrum.sizeDelta = v;
            }
        }
    }

}