using UnityEngine;
using System;
using UnityEngine.UI;

namespace OddOneOut
{
    /// <summary>
    /// This script holds an array of image groups. Each group of images is supposed to have similar features.
    /// </summary>
    public class OOOImageGroup : MonoBehaviour
    {
        [System.Serializable]
        public class ImageGroup
        {
            public Sprite[] images;
        }
        public ImageGroup[] imageGroups;
    }
}