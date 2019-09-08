using UnityEngine;

public class bl_Speaker : MonoBehaviour
{
    [SerializeField]private int Channel = 0;
    [SerializeField,Range(5,20)]private float KickLerp = 15;
    [SerializeField]private bl_SpeakerInfo[] Speakers;

    void Update()
    {
        //Get spectrum data samples from audio listener
        float[] data = new float[512];
        AudioListener.GetSpectrumData(data, Channel,FFTWindow.BlackmanHarris);

        for (int i = 0; i < Speakers.Length; i++)
        {
            if (Speakers[i].Transform != null)
            {
                float f = (data[Speakers[i].Frequency] * Speakers[i].Multipier);
                if (f > 0)
                {
                    Vector3 vs = Speakers[i].Transform.localScale;
                    vs.x = 1 + f;
                    vs.y = 1 + f;
                    vs.z = 1 + f;
                    Speakers[i].Transform.localScale = Vector3.Lerp(Speakers[i].Transform.localScale, vs, Time.deltaTime * KickLerp);
                }
                else
                {
                    Speakers[i].Transform.localScale = Vector3.Lerp(Speakers[i].Transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * KickLerp);
                }
            }
        }
    }
}