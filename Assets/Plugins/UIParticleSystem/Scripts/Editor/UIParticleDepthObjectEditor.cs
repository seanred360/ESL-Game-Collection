using System.Collections;
using System.Collections.Generic;
using UIParticleEnums;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UIParticleDepthObject))]
public class UIParticleDepthObjectEditor : Editor
{
    private UIParticleDepthObject depthObj;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        depthObj = (UIParticleDepthObject)target;

        if(depthObj)
        {
            EditorGUI.BeginChangeCheck();

            depthObj.source = (UIParticleMaskSourceMode)EditorGUILayout.EnumPopup("Graphic source", depthObj.source);
            depthObj.renderMode = (UIParticleMaskRenderMode)EditorGUILayout.EnumPopup("Render mode", depthObj.renderMode);
            depthObj.alphaMode = (UIParticleMaskAlphaMode)EditorGUILayout.EnumPopup("Alpha mode", depthObj.alphaMode);

            if(depthObj.source == UIParticleMaskSourceMode.MaskTexture)
                depthObj.maskTexture = EditorGUILayout.ObjectField("Texture", depthObj.maskTexture, typeof(Texture2D), false) as Texture2D;

            if(depthObj.alphaMode == UIParticleMaskAlphaMode.AlphaTest)
            {
                depthObj.alphaTestTreshold = EditorGUILayout.Slider("Alpha test treshold", depthObj.alphaTestTreshold, 0f, 1f);
            }

            if(depthObj.alphaMode == UIParticleMaskAlphaMode.Dithering)
            {
                depthObj.ditheringSteps = EditorGUILayout.IntSlider("Alpha dithering steps", depthObj.ditheringSteps, 1, 255);
			}

			if(depthObj.alphaMode == UIParticleMaskAlphaMode.Translucency)
			{
				depthObj.translucencyFactor = EditorGUILayout.Slider("Visibility", depthObj.translucencyFactor, 0f, 1f);
			}

            if(depthObj.renderMode == UIParticleMaskRenderMode.CullingMask)
            {
                depthObj.cullingMaskVal = EditorGUILayout.IntSlider("Culling mask value(layer)", depthObj.cullingMaskVal, 0, 255);
            }

            depthObj.willRectResizeInRuntime = EditorGUILayout.Toggle("Will Rect resize in runtime", depthObj.willRectResizeInRuntime);

            if(EditorGUI.EndChangeCheck())
            {
                depthObj.RefreshRenderer(true);
                if(!Application.isPlaying)
                {
                    EditorUtility.SetDirty(depthObj);
                    if(depthObj.gameObject.scene.IsValid())
                        EditorSceneManager.MarkSceneDirty(depthObj.gameObject.scene);
                }
            }
            
            if(depthObj.EditorErrorLog != null && depthObj.EditorErrorLog.Length > 0)
            {
                EditorGUILayout.HelpBox(depthObj.EditorErrorLog, MessageType.Error);
            }
        }
    }

}
