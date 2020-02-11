using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRoute : MonoBehaviour
{
    Transform[] childObjects;
    public List<Transform> childNodeList = new List<Transform>();
    //public LineRenderer lineRenderer;

    private void Awake()
    {
        //lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        FillNodes();
        //lineRenderer.positionCount = childNodeList.Count;

        for (int i = 0; i < childNodeList.Count; i++)
        {
            //LineRenderer lineRenderer = childNodeList[i].GetComponent<LineRenderer>();
            Vector3 currentPos = childNodeList[i].position;
            if (i > 0)
            {
                Vector3 prevPos = childNodeList[i - 1].position;
                Gizmos.DrawLine(prevPos, currentPos);
                //lineRenderer.SetPosition(i - 1, prevPos);
                //lineRenderer.SetPosition(i, currentPos);
                //lineRenderer.SetPosition(0, prevPos);
                //lineRenderer.SetPosition(1, currentPos);
            }
        }
    }

    private void FillNodes()
    {
        childNodeList.Clear();
        childObjects = GetComponentsInChildren<Transform>();

        foreach(Transform child in childObjects)
        {
            if(child != this.transform && child.tag =="Node")
            {
                childNodeList.Add(child);
            }
        }
    }
}
