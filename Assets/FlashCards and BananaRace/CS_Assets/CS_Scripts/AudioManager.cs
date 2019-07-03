using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AudioManager : MonoBehaviour
    {

        public AudioSource Music;
        public AudioSource SFX;
        public AudioSource Voice;

        public float MasterVol = 1;
        public float MusicVol = 1;
        public float SFXVol = 1;
        public float VoiceVol = 1;

        public int MIndex = 0;
        public int SIndex = 0;
        public int VIndex = 0;

        public AudioClip[] MClips;// music
        public AudioClip[] SClips;// sfx
        public AudioClip[] VClips;// voice

        public void PlayMusic()
        {
            PlayMusic(MIndex);
        }

        public void PlayMusic(int useIndex)
        {
            if (Music != null && MClips != null)
            {
                if (MasterVol <= 1)
                {
                    Music.clip = MClips[useIndex];
                    Music.ignoreListenerVolume = false;
                    Music.PlayOneShot(MClips[useIndex], MusicVol);
                }
                else
                {
                    Music.clip = MClips[useIndex];
                    Music.ignoreListenerVolume = true;
                    Music.PlayOneShot(MClips[useIndex], MusicVol);
                }
            }
        }

        public void PlaySFX(int useIndex)
        {
            if (SFX != null && SClips != null)
            {
                if (MasterVol <= 1)
                {
                    SFX.clip = SClips[useIndex];
                    SFX.ignoreListenerVolume = false;
                    SFX.PlayOneShot(SClips[useIndex], SFXVol);
                }
                else
                {
                    SFX.clip = SClips[useIndex];
                    SFX.ignoreListenerVolume = true;
                    SFX.PlayOneShot(SClips[useIndex], SFXVol);
                }
            }
        }

        public void StopMusic()
        {
            Music.Stop();
        }
    }
