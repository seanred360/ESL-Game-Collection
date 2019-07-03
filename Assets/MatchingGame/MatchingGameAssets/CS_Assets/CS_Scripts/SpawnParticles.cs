using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour {

    public Transform starParticles;
    public Transform spawnLocation;


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && (spawnLocation.gameObject.activeSelf == true))
        {
            Transform starParticlesInstance;
            starParticlesInstance = Instantiate(starParticles, spawnLocation.position, spawnLocation.rotation) as Transform;
        }
    }
}
