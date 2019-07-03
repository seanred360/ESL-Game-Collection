using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class UIParticleShaderEditor : ShaderGUI
{
    public class BlendingPair
    {
        public BlendMode blend, blend2;

        public BlendingPair(BlendMode blend, BlendMode blend2)
        {
            this.blend = blend;
            this.blend2 = blend2;
        }
    }
    private Dictionary<string, BlendingPair> blendings = new Dictionary<string, BlendingPair>()
    {
        {"Alpha Blend", new BlendingPair(BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha)},
        {"Additive", new BlendingPair(BlendMode.SrcAlpha, BlendMode.One)},
        {"Additive Smooth", new BlendingPair(BlendMode.One, BlendMode.OneMinusSrcColor)},
        {"Pre Multiply", new BlendingPair(BlendMode.One, BlendMode.OneMinusSrcAlpha)},
        {"Multiply", new BlendingPair(BlendMode.Zero, BlendMode.SrcColor)},
        {"Double Multiply", new BlendingPair(BlendMode.DstColor, BlendMode.SrcColor)},
    };

    public enum ClippingMode
    {
        InsideMask, OutsideMask
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);

        Material material = materialEditor.target as Material;

        EditorGUI.BeginChangeCheck();

        DrawClippingMaskSettings(material);

        DrawBlendingSettings(material);

        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(material);
        }
    }

    private static void DrawClippingMaskSettings(Material material)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        bool useClippingMask = material.IsKeywordEnabled("USE_CLIPPING_MASK");
        useClippingMask = EditorGUILayout.Toggle("Use clipping mask?", useClippingMask);
        if(useClippingMask != material.IsKeywordEnabled("USE_CLIPPING_MASK"))
        {
            if(useClippingMask)
                material.EnableKeyword("USE_CLIPPING_MASK");
            else
                material.DisableKeyword("USE_CLIPPING_MASK");
        }

        if(useClippingMask)
        {
            int maskValue = Mathf.RoundToInt(material.GetFloat("_ClippingMaskVal") * 255f);
            int maskValTmp = maskValue;
            maskValue = EditorGUILayout.IntSlider("Clipping mask", maskValue, 0, 255);
            if(maskValue != maskValTmp)
            {
                material.SetFloat("_ClippingMaskVal", maskValue / 255f);
            }

            ClippingMode clippingMode = material.IsKeywordEnabled("CLIPPINGMODE_OUTSIDE") ? ClippingMode.OutsideMask : ClippingMode.InsideMask;
            ClippingMode newClippingMode = (ClippingMode)EditorGUILayout.EnumPopup("Clipping mode", clippingMode);
            if(clippingMode != newClippingMode)
            {
                if(newClippingMode == ClippingMode.InsideMask)
                {
                    material.EnableKeyword("CLIPPINGMODE_INSIDE");
                    material.DisableKeyword("CLIPPINGMODE_OUTSIDE");
                }
                else
                {
                    material.DisableKeyword("CLIPPINGMODE_INSIDE");
                    material.EnableKeyword("CLIPPINGMODE_OUTSIDE");
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawBlendingSettings(Material material)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        string currentBlending = "Custom";
        int currentBlend = (int)material.GetFloat("_Blend");
        int currentBlend2 = (int)material.GetFloat("_Blend2");
        foreach(var blendData in blendings)
        {
            if((int)blendData.Value.blend == currentBlend
                && (int)blendData.Value.blend2 == currentBlend2)
            {
                currentBlending = blendData.Key;
                break;
            }
        }
        EditorGUILayout.LabelField(string.Format("Set Blending mode (current - {0}):", currentBlending));

        foreach(var blendData in blendings)
        {
            if(GUILayout.Button(blendData.Key))
            {
                material.SetInt("_Blend", (int)blendData.Value.blend);
                material.SetInt("_Blend2", (int)blendData.Value.blend2);
            }
        }

        EditorGUILayout.EndVertical();
    }
}