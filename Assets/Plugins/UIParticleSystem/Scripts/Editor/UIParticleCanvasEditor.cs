using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UIParticleCanvas))]
public class UIParticleCanvasEditor : Editor
{
    private UIParticleCanvas particleCanvas;
    private static bool debugMask;
    private static bool debugCullMask;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        particleCanvas = (UIParticleCanvas)target;

        if(particleCanvas)
        {
            EditorGUI.BeginChangeCheck();

            particleCanvas.maskResolutionScale = EditorGUILayout.Slider("Mask resolution scale", particleCanvas.maskResolutionScale, 0.05f, 1f);
            particleCanvas.maskLayer = EditorGUILayout.LayerField("Mask layer", particleCanvas.maskLayer);
			if(particleCanvas.Canvas.worldCamera == null)
			{
				EditorGUILayout.HelpBox("Attach camera to GUI Canvas!", MessageType.Error);
			}
			else
			{
				bool layerIsInCulling = particleCanvas.Canvas.worldCamera.cullingMask == (particleCanvas.Canvas.worldCamera.cullingMask | (1 << particleCanvas.maskLayer));
				if(layerIsInCulling)
					EditorGUILayout.HelpBox("Mask layer shouldn't be the same as layer rendered by GUI camera, change the mask layer or change culling mask in GUI camera", MessageType.Error);
			}

			if(EditorGUI.EndChangeCheck())
            {
                particleCanvas.enabled = false;
                particleCanvas.SetToReinitialize();
                particleCanvas.enabled = true;
                if(!Application.isPlaying)
                {
                    EditorUtility.SetDirty(particleCanvas);
                    if(particleCanvas.gameObject.scene.IsValid())
                        EditorSceneManager.MarkSceneDirty(particleCanvas.gameObject.scene);
                }
            }

            if(particleCanvas.Mask != null && (debugMask = EditorGUILayout.Foldout(debugMask, "Show depth mask")))
            {
                Rect texRect = GUILayoutUtility.GetAspectRect((float)particleCanvas.Mask.width / particleCanvas.Mask.height);
                EditorGUI.DrawPreviewTexture(texRect, particleCanvas.Mask);
            }

            if(particleCanvas.Mask != null && (debugCullMask = EditorGUILayout.Foldout(debugCullMask, "Show culling mask")))
            {
                Rect texRect = GUILayoutUtility.GetAspectRect((float)particleCanvas.Mask.width / particleCanvas.Mask.height);
                EditorGUI.DrawTextureAlpha(texRect, particleCanvas.Mask);
            }

            if(GUILayout.Button("Refresh"))
            {
                particleCanvas.enabled = false;
                particleCanvas.SetToReinitialize();
                particleCanvas.enabled = true;
            }

            if(particleCanvas.EditorErrorLog != null && particleCanvas.EditorErrorLog.Length > 0)
            {
                EditorGUILayout.HelpBox(particleCanvas.EditorErrorLog, MessageType.Error);
            }
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

}
