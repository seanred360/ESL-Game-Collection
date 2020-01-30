﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AudioManager : MonoBehaviour
    {

        public AudioSource Music;
        public AudioSource SFX;
        public AudioSource Voice;
        public AudioSource Points;

        public float MasterVol = 1;
        public float MusicVol = 1;
        public float SFXVol = 1;
        public float VoiceVol = 1;
        public float PointsVol = 1;

        public int MIndex = 0;
        public int SIndex = 0;
        public int VIndex = 0;
        public int PIndex = 0;

        public AudioClip[] MClips;// music
        public AudioClip[] SClips;// sfx
        public AudioClip[] VClips;// voice
        public AudioClip[] PClips;

        public float audioPitch = 1;

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

    public void PlayVoice(int useIndex)
    {
        if (Voice != null && VClips != null)
        {
            if (MasterVol <= 1)
            {
                Voice.clip = VClips[useIndex];
                Voice.ignoreListenerVolume = false;
                Voice.PlayOneShot(VClips[useIndex], VoiceVol);
            }
            else
            {
                Voice.clip = VClips[useIndex];
                Voice.ignoreListenerVolume = true;
                Voice.PlayOneShot(VClips[useIndex], VoiceVol);
            }
        }
    }

    public void PlayPointsSound(int useIndex)
        {
            if (Points != null && PClips != null)
            {
                if (MasterVol <= 1)
                {
                    Points.pitch = audioPitch;
                    Points.clip = PClips[useIndex];
                    Points.ignoreListenerVolume = false;
                    Points.PlayOneShot(PClips[useIndex], PointsVol);
                }
                else
                {
                    Points.pitch = audioPitch;
                    Points.clip = PClips[useIndex];
                    Points.ignoreListenerVolume = true;
                    Points.PlayOneShot(PClips[useIndex], PointsVol);
                }
                audioPitch += .1f;
            }
        }

        public void StopMusic()
        {
            Music.Stop();
        }
    }