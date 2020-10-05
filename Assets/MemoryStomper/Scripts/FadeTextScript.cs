using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TMPro
{
    public class FadeTextScript : MonoBehaviour
    {

        TextMeshPro textMesh;

        private void Start()
        {
            textMesh = GetComponent<TextMeshPro>();
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            textMesh.color = Color.white;
            yield return new WaitForSeconds(.5f);
            textMesh.color = Color.clear;
            StartCoroutine(Flash());
        }
    }
}
