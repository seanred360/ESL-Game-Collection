using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class WaterMove : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.GetComponent<Renderer>().material.SetFloat("_FlowSpeedY", .15f);
        }
    }
}
