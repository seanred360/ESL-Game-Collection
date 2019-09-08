using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class bl_SampleToAlpha : MonoBehaviour {

    [SerializeField]private int Sample;
    [SerializeField]private int Channel = 0;
    [SerializeField,Range(5,20)]private float KickLerp = 15;
    [Range(1,5)]public float Multiplier = 2;

    private CanvasGroup Alpha;

    void Awake()
    {
        Alpha = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Alpha == null)
            return;

        //Get spectrum data samples from audio listener
        float[] data = new float[512];
        AudioListener.GetSpectrumData(data, Channel, FFTWindow.BlackmanHarris);
        float v = data[Sample] * Multiplier;
        Alpha.alpha = Mathf.Lerp(Alpha.alpha, v, Time.deltaTime * KickLerp);
    }
}