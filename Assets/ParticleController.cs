using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject[] mixParticles;
    public GameObject[] correctParticles;

    private void Start()
    {
        mixParticles = GameObject.FindGameObjectsWithTag("MixParticle");
        correctParticles = GameObject.FindGameObjectsWithTag("CorrectParticle");
    }

    public void EnableMixParticles()
    {
        foreach (GameObject mixParticles in mixParticles)
        {
            mixParticles.SetActive(true);
        }
    }

    public void EnableCorrectParticles()
    {
        foreach (GameObject correctParticles in correctParticles)
        {
            correctParticles.SetActive(true);
        }
    }
}
