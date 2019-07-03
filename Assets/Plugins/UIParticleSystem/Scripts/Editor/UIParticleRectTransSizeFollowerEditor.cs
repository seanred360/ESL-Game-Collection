using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UIParticleRectTransSizeFollower))]
public class UIParticleRectTransSizeFollowerEditor : Editor
{
    private UIParticleRectTransSizeFollower follower;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        follower = (UIParticleRectTransSizeFollower)target;

        if(follower)
        {
            if(follower.targetRectTransform == null)
            {
                EditorGUILayout.HelpBox("Select target rect transform", MessageType.Info);
                follower.dataSnapshot = new UIParticleRectTransSizeFollower.UISnapshot();
            }
            else if(follower.dataSnapshot.transformScale == Vector3.zero)
            {
                EditorGUILayout.HelpBox("Make rect snapshot to save settings to which object will be scaled", MessageType.Info);
            }

            if(follower.targetRectTransform != null && !Application.isPlaying && GUILayout.Button("Make rect snapshot"))
            {
                follower.MakeRectSnapshot();
                EditorUtility.SetDirty(follower);
            }
        }
    }

}
