using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StripScroller : MonoBehaviour
{
    public float scrollSpeed = -5f;
    Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, 20);
        transform.position = startPos + Vector2.right * newPos;
    }
}
