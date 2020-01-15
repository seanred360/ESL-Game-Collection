//RealToonGUI(Lite)
//MJQStudioWorks
//2019

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

public class RealToonShaderGUI_Lite : ShaderGUI
{

    #region foldout bools variable

    static bool ShowTextureColor;
    static bool ShowNormalMap;
    static bool ShowTransparency;
    static bool ShowMatCap;
    static bool ShowOutline;
    static bool ShowSelfLit;
    static bool ShowGloss;
    static bool ShowShadow;
    static bool ShowLighting;
    static bool ShowFReflection;
    static bool ShowRimLight;
    static bool ShowSeeThrough;
    static bool ShowDisableEnable;
    static bool ShowUI = true;

    static string ShowUIString = "Hide UI";

    #endregion

    #region Variables

    string shader_name;
    string shader_type;

    #endregion

    #region Material Properties Variables

    MaterialProperty _DoubleSided;

    MaterialProperty _MainTex;
    MaterialProperty _MainColor;
    MaterialProperty _MainColorAffectTexture;
    MaterialProperty _MVCOL;
    MaterialProperty _MCIALO;
    MaterialProperty _EnableTextureTransparent;

    MaterialProperty _MCapIntensity;
    MaterialProperty _MCap;
    MaterialProperty _MCapMask;

    MaterialProperty _NormalMap;
    MaterialProperty _NormalMapIntensity;

    MaterialProperty _Opacity;
    MaterialProperty _TransparentThreshold;
    MaterialProperty _AffectShadow;
    MaterialProperty _MaskTransparency;

    MaterialProperty _OutlineWidth;
    MaterialProperty _OutlineWidthControl;
    MaterialProperty _OutlineExtrudeMethod;
    MaterialProperty _OutlineOffset;
    MaterialProperty _OutlineZPostionInCamera;
    MaterialProperty _DoubleSidedOutline;
    MaterialProperty _VertexColorRedAffectOutlineWitdh;
    MaterialProperty _OutlineWidthAffectedByViewDistance;
    MaterialProperty _FarDistanceMaxWidth;
    MaterialProperty _OutlineColor;
    MaterialProperty _MixMainTexToOutline;
    MaterialProperty _LightAffectOutlineColor;

    MaterialProperty _SelfLitIntensity;
    MaterialProperty _SelfLitColor;
    MaterialProperty _SelfLitPower;
    MaterialProperty _SelfLitHighContrast;
    MaterialProperty _MaskSelfLit;

    MaterialProperty _Glossiness;
    MaterialProperty _GlossSoftness;
    MaterialProperty _GlossColor;
    MaterialProperty _GlossColorPower;
    MaterialProperty _MaskGloss;

    MaterialProperty _GlossTexture;
    MaterialProperty _GlossTextureSoftness;
    MaterialProperty _GlossTextureFollowObjectRotation;
    MaterialProperty _GlossTextureFollowLight;

    MaterialProperty _OverallShadowColor;
    MaterialProperty _OverallShadowColorPower;
    MaterialProperty _SelfShadowShadowTAtViewDirection;

    MaterialProperty _HighlightColor;
    MaterialProperty _HighlightColorPower;

    MaterialProperty _SelfShadowThreshold;
    MaterialProperty _VertexColorGreenControlSelfShadowThreshold;
    MaterialProperty _SelfShadowHardness;
    MaterialProperty _SelfShadowRealTimeShadowColor;
    MaterialProperty _SelfShadowRealTimeShadowColorPower;

    MaterialProperty _SmoothObjectNormal;
    MaterialProperty _VertexColorBlueControlSmoothObjectNormal;
    MaterialProperty _XYZPosition;
    MaterialProperty _XYZHardness;
    MaterialProperty _ShowNormal;

    MaterialProperty _ShadowColorTexture;
    MaterialProperty _ShadowColorTexturePower;

    MaterialProperty _ShadowT;
    MaterialProperty _ShadowTLightThreshold;
    MaterialProperty _ShadowTShadowThreshold;
    MaterialProperty _ShadowTColor;
    MaterialProperty _ShadowTColorPower;
    MaterialProperty _ShadowTHardness;
    MaterialProperty _STIL;
    MaterialProperty _LightFalloffAffectShadowT;

    MaterialProperty _PTexture;
    MaterialProperty _PTexturePower;

    MaterialProperty _DirectionalLightIntensity;
    MaterialProperty _PointSpotlightIntensity;
    MaterialProperty _LightFalloffSoftness;
    MaterialProperty _ReduceShadowPointLight;
    MaterialProperty _ReduceShadowSpotDirectionalLight;

    MaterialProperty _CustomLightDirectionIntensity;
    MaterialProperty _CustomLightDirectionFollowObjectRotation;
    MaterialProperty _CustomLightDirection;

    MaterialProperty _FReflectionIntensity;
    MaterialProperty _FReflection;
    MaterialProperty _FReflectionRoughtness;
    MaterialProperty _RefMetallic;
    MaterialProperty _MaskFReflection;

    MaterialProperty _RimLightUnfill;
    MaterialProperty _RimLightColor;
    MaterialProperty _RimLightColorPower;
    MaterialProperty _LightAffectRimLightColor;
    MaterialProperty _RimLightSoftness;
    MaterialProperty _RimLightInLight;

    MaterialProperty _RefVal;
    MaterialProperty _Oper;
    MaterialProperty _Compa;

    MaterialProperty _L_F_MC;
    MaterialProperty _L_F_NM;
    MaterialProperty _L_F_O;
    MaterialProperty _L_F_SL;
    MaterialProperty _L_F_GLO;
    MaterialProperty _L_F_GLOT;
    MaterialProperty _L_F_SS;
    MaterialProperty _L_F_SON;
    MaterialProperty _L_F_SCT;
    MaterialProperty _L_F_ST;
    MaterialProperty _L_F_PT;
    MaterialProperty _L_F_UOAL;
    MaterialProperty _L_F_CLD;
    MaterialProperty _L_F_FR;
    MaterialProperty _L_F_RL;
    MaterialProperty _L_F_HPSS;
    MaterialProperty _ZWrite;

    #endregion

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        //This Material
        Material targetMat = materialEditor.target as Material;

        //Settings
        materialEditor.SetDefaultGUIWidths();

        //Content

        #region Shader Name Switch

        switch (targetMat.shader.name)
        {
            case "RealToon/Version 5/Lite/Default":
                shader_name = "lite_d";
                shader_type = "Default";
                break;

            case "RealToon/Version 5/Lite/Fade Transparency":
                shader_name = "lite_ft";
                shader_type = "Fade Transperancy";
                break;
            default:
                shader_name = string.Empty;
                shader_type = string.Empty;
                break;
        }

        #endregion

        #region Material Properties


        _DoubleSided = ShaderGUI.FindProperty("_DoubleSided", properties);

        _MainTex = ShaderGUI.FindProperty("_MainTex", properties);
        _MainColor = ShaderGUI.FindProperty("_MainColor", properties);

        _MVCOL = ShaderGUI.FindProperty("_MVCOL", properties);

        _MCIALO = ShaderGUI.FindProperty("_MCIALO", properties);

        _MCapIntensity = ShaderGUI.FindProperty("_MCapIntensity", properties);
        _MCap = ShaderGUI.FindProperty("_MCap", properties);
        _MCapMask = ShaderGUI.FindProperty("_MCapMask", properties);

        if (shader_name == "lite_d")
        {
            _EnableTextureTransparent = ShaderGUI.FindProperty("_EnableTextureTransparent", properties);
            _Opacity = null;
            _AffectShadow = null;
            _MaskTransparency = null;
        }
        else if (shader_name == "lite_ft")
        {
            _EnableTextureTransparent = null;
            _Opacity = ShaderGUI.FindProperty("_Opacity", properties);
            _TransparentThreshold = ShaderGUI.FindProperty("_TransparentThreshold", properties);
            _AffectShadow = ShaderGUI.FindProperty("_AffectShadow", properties);
            _MaskTransparency = ShaderGUI.FindProperty("_MaskTransparency", properties);
        }

        _NormalMap = ShaderGUI.FindProperty("_NormalMap", properties);
        _NormalMapIntensity = ShaderGUI.FindProperty("_NormalMapIntensity", properties);

        if (shader_name == "lite_d")
        {
            _OutlineWidth = ShaderGUI.FindProperty("_OutlineWidth", properties);
            _OutlineWidthControl = ShaderGUI.FindProperty("_OutlineWidthControl", properties);
            _OutlineExtrudeMethod = ShaderGUI.FindProperty("_OutlineExtrudeMethod", properties);
            _OutlineOffset = ShaderGUI.FindProperty("_OutlineOffset", properties);
            _OutlineZPostionInCamera = ShaderGUI.FindProperty("_OutlineZPostionInCamera", properties);
            _DoubleSidedOutline = ShaderGUI.FindProperty("_DoubleSidedOutline", properties);
            _OutlineColor = ShaderGUI.FindProperty("_OutlineColor", properties);
            _MixMainTexToOutline = ShaderGUI.FindProperty("_MixMainTexToOutline", properties);
            _OutlineWidthAffectedByViewDistance = ShaderGUI.FindProperty("_OutlineWidthAffectedByViewDistance", properties);
            _FarDistanceMaxWidth = ShaderGUI.FindProperty("_FarDistanceMaxWidth", properties);
            _VertexColorRedAffectOutlineWitdh = ShaderGUI.FindProperty("_VertexColorRedAffectOutlineWitdh", properties);
            _LightAffectOutlineColor = ShaderGUI.FindProperty("_LightAffectOutlineColor", properties);

        }
        else if (shader_name == "lite_ft")
        {
            _OutlineWidth = null;
            _OutlineWidthControl = null;
            _OutlineExtrudeMethod = null;
            _OutlineOffset = null;
            _OutlineZPostionInCamera = null;
            _DoubleSidedOutline = null;
            _OutlineColor = null;
            _MixMainTexToOutline = null;
            _OutlineWidthAffectedByViewDistance = null;
            _FarDistanceMaxWidth = null;
            _LightAffectOutlineColor = null;
            _VertexColorRedAffectOutlineWitdh = null;
        }

        _SelfLitIntensity = ShaderGUI.FindProperty("_SelfLitIntensity", properties);
        _SelfLitColor = ShaderGUI.FindProperty("_SelfLitColor", properties);
        _SelfLitPower = ShaderGUI.FindProperty("_SelfLitPower", properties);
        _SelfLitHighContrast = ShaderGUI.FindProperty("_SelfLitHighContrast", properties);
        _MaskSelfLit = ShaderGUI.FindProperty("_MaskSelfLit", properties);

        _Glossiness = ShaderGUI.FindProperty("_Glossiness", properties);
        _GlossSoftness = ShaderGUI.FindProperty("_GlossSoftness", properties);
        _GlossColor = ShaderGUI.FindProperty("_GlossColor", properties);
        _GlossColorPower = ShaderGUI.FindProperty("_GlossColorPower", properties);
        _MaskGloss = ShaderGUI.FindProperty("_MaskGloss", properties);

        _GlossTexture = ShaderGUI.FindProperty("_GlossTexture", properties);
        _GlossTextureSoftness = ShaderGUI.FindProperty("_GlossTextureSoftness", properties);
        _GlossTextureFollowObjectRotation = ShaderGUI.FindProperty("_GlossTextureFollowObjectRotation", properties);
        _GlossTextureFollowLight = ShaderGUI.FindProperty("_GlossTextureFollowLight", properties);

        _OverallShadowColor = ShaderGUI.FindProperty("_OverallShadowColor", properties);
        _OverallShadowColorPower = ShaderGUI.FindProperty("_OverallShadowColorPower", properties);
        _SelfShadowShadowTAtViewDirection = ShaderGUI.FindProperty("_SelfShadowShadowTAtViewDirection", properties);

        _HighlightColor = ShaderGUI.FindProperty("_HighlightColor", properties);
        _HighlightColorPower = ShaderGUI.FindProperty("_HighlightColorPower", properties);

        _SelfShadowThreshold = ShaderGUI.FindProperty("_SelfShadowThreshold", properties);
        _VertexColorGreenControlSelfShadowThreshold = ShaderGUI.FindProperty("_VertexColorGreenControlSelfShadowThreshold", properties);
        _SelfShadowHardness = ShaderGUI.FindProperty("_SelfShadowHardness", properties);

        if (shader_name == "lite_d")
        {
            _SelfShadowRealTimeShadowColor = ShaderGUI.FindProperty("_SelfShadowRealTimeShadowColor", properties);
            _SelfShadowRealTimeShadowColorPower = ShaderGUI.FindProperty("_SelfShadowRealTimeShadowColorPower", properties);
        }
        else if (shader_name == "lite_ft")
        {
            _SelfShadowRealTimeShadowColor = ShaderGUI.FindProperty("_SelfShadowColor", properties);
            _SelfShadowRealTimeShadowColorPower = ShaderGUI.FindProperty("_SelfShadowColorPower", properties);
        }

        _SmoothObjectNormal = ShaderGUI.FindProperty("_SmoothObjectNormal", properties);
        _VertexColorBlueControlSmoothObjectNormal = ShaderGUI.FindProperty("_VertexColorBlueControlSmoothObjectNormal", properties);
        _XYZPosition = ShaderGUI.FindProperty("_XYZPosition", properties);
        _XYZHardness = ShaderGUI.FindProperty("_XYZHardness", properties);
        _ShowNormal = ShaderGUI.FindProperty("_ShowNormal", properties);

        _ShadowColorTexture = ShaderGUI.FindProperty("_ShadowColorTexture", properties);
        _ShadowColorTexturePower = ShaderGUI.FindProperty("_ShadowColorTexturePower", properties);

        _ShadowT = ShaderGUI.FindProperty("_ShadowT", properties);
        _ShadowTLightThreshold = ShaderGUI.FindProperty("_ShadowTLightThreshold", properties);
        _ShadowTShadowThreshold = ShaderGUI.FindProperty("_ShadowTShadowThreshold", properties);
        _ShadowTColor = ShaderGUI.FindProperty("_ShadowTColor", properties);
        _ShadowTColorPower = ShaderGUI.FindProperty("_ShadowTColorPower", properties);
        _ShadowTHardness = ShaderGUI.FindProperty("_ShadowTHardness", properties);
        _STIL = ShaderGUI.FindProperty("_STIL", properties);
        _LightFalloffAffectShadowT = ShaderGUI.FindProperty("_LightFalloffAffectShadowT", properties);

        _PTexture = ShaderGUI.FindProperty("_PTexture", properties);
        _PTexturePower = ShaderGUI.FindProperty("_PTexturePower", properties);

        _DirectionalLightIntensity = ShaderGUI.FindProperty("_DirectionalLightIntensity", properties);
        _PointSpotlightIntensity = ShaderGUI.FindProperty("_PointSpotlightIntensity", properties);
        _LightFalloffSoftness = ShaderGUI.FindProperty("_LightFalloffSoftness", properties);

        _CustomLightDirectionIntensity = ShaderGUI.FindProperty("_CustomLightDirectionIntensity", properties);
        _CustomLightDirectionFollowObjectRotation = ShaderGUI.FindProperty("_CustomLightDirectionFollowObjectRotation", properties);
        _CustomLightDirection = ShaderGUI.FindProperty("_CustomLightDirection", properties);

        _FReflectionIntensity = ShaderGUI.FindProperty("_FReflectionIntensity", properties);
        _FReflection = ShaderGUI.FindProperty("_FReflection", properties);
        _FReflectionRoughtness = ShaderGUI.FindProperty("_FReflectionRoughtness", properties);
        _RefMetallic = ShaderGUI.FindProperty("_RefMetallic", properties);
        _MaskFReflection = ShaderGUI.FindProperty("_MaskFReflection", properties);

        _RimLightUnfill = ShaderGUI.FindProperty("_RimLightUnfill", properties);
        _RimLightColor = ShaderGUI.FindProperty("_RimLightColor", properties);
        _RimLightColorPower = ShaderGUI.FindProperty("_RimLightColorPower", properties);
        _LightAffectRimLightColor = ShaderGUI.FindProperty("_LightAffectRimLightColor", properties);
        _RimLightSoftness = ShaderGUI.FindProperty("_RimLightSoftness", properties);
        _RimLightInLight = ShaderGUI.FindProperty("_RimLightInLight", properties);

        _RefVal = ShaderGUI.FindProperty("_RefVal", properties);
        _Oper = ShaderGUI.FindProperty("_Oper", properties);
        _Compa = ShaderGUI.FindProperty("_Compa", properties);

        _L_F_MC = ShaderGUI.FindProperty("_L_F_MC", properties);

        _L_F_NM = ShaderGUI.FindProperty("_L_F_NM", properties);

        if (shader_name == "lite_d")
        {
            _L_F_O = ShaderGUI.FindProperty("_L_F_O", properties);
        }

        _L_F_SL = ShaderGUI.FindProperty("_L_F_SL", properties);
        _L_F_GLO = ShaderGUI.FindProperty("_L_F_GLO", properties);
        _L_F_GLOT = ShaderGUI.FindProperty("_L_F_GLOT", properties);
        _L_F_SS = ShaderGUI.FindProperty("_L_F_SS", properties);
        _L_F_SON = ShaderGUI.FindProperty("_L_F_SON", properties);
        _L_F_SCT = ShaderGUI.FindProperty("_L_F_SCT", properties);
        _L_F_ST = ShaderGUI.FindProperty("_L_F_ST", properties);
        _L_F_PT = ShaderGUI.FindProperty("_L_F_PT", properties);
        _L_F_UOAL = ShaderGUI.FindProperty("_L_F_UOAL", properties);
        _L_F_CLD = ShaderGUI.FindProperty("_L_F_CLD", properties);
        _L_F_FR = ShaderGUI.FindProperty("_L_F_FR", properties);
        _L_F_RL = ShaderGUI.FindProperty("_L_F_RL", properties);

        if (shader_name == "lite_d")
        {
            _L_F_HPSS = ShaderGUI.FindProperty("_L_F_HPSS", properties);
            _ZWrite = null;
        }
        else if (shader_name == "lite_ft")
        {
            _L_F_HPSS = null;
            _ZWrite = ShaderGUI.FindProperty("_ZWrite", properties);
        }

        #endregion

        //UI

        #region UI

        //Header
        Rect r_header = EditorGUILayout.BeginVertical("HelpBox");
        EditorGUILayout.LabelField("RealToon Lite 5.0.5", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("(" + shader_type + ")", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        if (ShowUI == true)
        {

            GUILayout.Space(20);


            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            //Light Blend

            #region Light Blend

            Rect r_lightblend = EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("Light Blend Style: Traditional");
            EditorGUILayout.EndVertical();

            #endregion

            //Double Sided

            #region Double Sided

            Rect r_doublesided = EditorGUILayout.BeginVertical("HelpBox");
            materialEditor.ShaderProperty(_DoubleSided, _DoubleSided.displayName);
            EditorGUILayout.EndVertical();

            #endregion

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Space(20);

            //Texture - Color

            #region Texture - Color

            Rect r_texturecolor = EditorGUILayout.BeginVertical("Button");
            ShowTextureColor = EditorGUILayout.Foldout(ShowTextureColor, "(Texture - Color)", true, EditorStyles.foldout);

            if (ShowTextureColor)
            {
                GUILayout.Space(10);

                materialEditor.ShaderProperty(_MainTex, _MainTex.displayName);
                materialEditor.ShaderProperty(_MainColor, _MainColor.displayName);
                materialEditor.ShaderProperty(_MVCOL, _MVCOL.displayName);

                GUILayout.Space(10);
                materialEditor.ShaderProperty(_MCIALO, _MCIALO.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_HighlightColor, _HighlightColor.displayName);
                materialEditor.ShaderProperty(_HighlightColorPower, _HighlightColorPower.displayName);

                GUILayout.Space(10);

                if (shader_name != "lite_ft")
                {
                    EditorGUI.BeginDisabledGroup(_MainTex.textureValue == null);
                    materialEditor.ShaderProperty(_EnableTextureTransparent, _EnableTextureTransparent.displayName);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    _EnableTextureTransparent = null;
                }

                ShowTextureColor = true;
            }

            EditorGUILayout.EndVertical();

            #endregion

            //MatCap

            #region MatCap

            if (_L_F_MC.floatValue == 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_matcap = EditorGUILayout.BeginVertical("Button");
                ShowMatCap = EditorGUILayout.Foldout(ShowMatCap, "(MatCap)", true, EditorStyles.foldout);

                if (ShowMatCap)
                {

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_MCapIntensity, _MCapIntensity.displayName);
                    materialEditor.ShaderProperty(_MCap, _MCap.displayName);
                    materialEditor.ShaderProperty(_MCapMask, _MCapMask.displayName);

                }

                EditorGUILayout.EndVertical();
            }

            #endregion

            //Normal Map

            #region Normal Map

            if (_L_F_NM.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_normalmap = EditorGUILayout.BeginVertical("Button");
                ShowNormalMap = EditorGUILayout.Foldout(ShowNormalMap, "(Normal Map)", true, EditorStyles.foldout);

                if (ShowNormalMap)
                {
                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_NormalMap, _NormalMap.displayName);

                    EditorGUI.BeginDisabledGroup(_NormalMap.textureValue == null);
                    materialEditor.ShaderProperty(_NormalMapIntensity, _NormalMapIntensity.displayName);
                    EditorGUI.EndDisabledGroup();

                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            //Transperancy

            #region Transperancy

            if (shader_name == "lite_ft")
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_transparency = EditorGUILayout.BeginVertical("Button");
                ShowTransparency = EditorGUILayout.Foldout(ShowTransparency, "(Transparency)", true, EditorStyles.foldout);

                if (ShowTransparency)
                {
                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_Opacity, _Opacity.displayName);
                    materialEditor.ShaderProperty(_TransparentThreshold, _TransparentThreshold.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_AffectShadow, _AffectShadow.displayName);

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_MaskTransparency, _MaskTransparency.displayName);

                }

                EditorGUILayout.EndVertical();
            }

            #endregion

            //Outline

            #region Outline

            if (shader_name == "lite_d")
            {
                if (_L_F_O.floatValue == 1)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    Rect r_outline = EditorGUILayout.BeginVertical("Button");
                    ShowOutline = EditorGUILayout.Foldout(ShowOutline, "(Outline)", true, EditorStyles.foldout);


                    if (ShowOutline)
                    {
                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_OutlineWidth, new GUIContent(_OutlineWidth.displayName, "Outline Width"));

                        materialEditor.ShaderProperty(_OutlineWidthControl, _OutlineWidthControl.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_OutlineExtrudeMethod, _OutlineExtrudeMethod.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_OutlineOffset, _OutlineOffset.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_OutlineZPostionInCamera, _OutlineZPostionInCamera.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_DoubleSidedOutline, _DoubleSidedOutline.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_OutlineColor, _OutlineColor.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_MixMainTexToOutline, _MixMainTexToOutline.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_LightAffectOutlineColor, _LightAffectOutlineColor.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_OutlineWidthAffectedByViewDistance, _OutlineWidthAffectedByViewDistance.displayName);
                        EditorGUI.BeginDisabledGroup(_OutlineWidthAffectedByViewDistance.floatValue == 0);
                        materialEditor.ShaderProperty(_FarDistanceMaxWidth, _FarDistanceMaxWidth.displayName);
                        EditorGUI.EndDisabledGroup();

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_VertexColorRedAffectOutlineWitdh, _VertexColorRedAffectOutlineWitdh.displayName);

                    }

                    EditorGUILayout.EndVertical();

                }

            }

            #endregion

            //Self Lit

            #region SelfLit

            if (_L_F_SL.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_selflit = EditorGUILayout.BeginVertical("Button");
                ShowSelfLit = EditorGUILayout.Foldout(ShowSelfLit, "(Self Lit)", true, EditorStyles.foldout);

                if (ShowSelfLit)
                {

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_SelfLitIntensity, _SelfLitIntensity.displayName);
                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_SelfLitColor, _SelfLitColor.displayName);
                    materialEditor.ShaderProperty(_SelfLitPower, _SelfLitPower.displayName);
                    materialEditor.ShaderProperty(_SelfLitHighContrast, _SelfLitHighContrast.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_MaskSelfLit, _MaskSelfLit.displayName);

                }

                EditorGUILayout.EndVertical();

            }
            #endregion

            //Gloss

            #region Gloss

            if (_L_F_GLO.floatValue == 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_gloss = EditorGUILayout.BeginVertical("Button");
                ShowGloss = EditorGUILayout.Foldout(ShowGloss, "(Gloss)", true, EditorStyles.foldout);

                if (ShowGloss)
                {
                    GUILayout.Space(10);

                    EditorGUI.BeginDisabledGroup(_L_F_GLOT.floatValue == 1);
                    materialEditor.ShaderProperty(_Glossiness, _Glossiness.displayName);
                    materialEditor.ShaderProperty(_GlossSoftness, _GlossSoftness.displayName);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_GlossColor, _GlossColor.displayName);
                    materialEditor.ShaderProperty(_GlossColorPower, _GlossColorPower.displayName);

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_MaskGloss, _MaskGloss.displayName);

                    GUILayout.Space(10);

                    //Gloss Texture

                    #region Gloss Texture

                    if (_L_F_GLOT.floatValue == 1)
                    {

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        Rect r_glosstexture = EditorGUILayout.BeginVertical("Button");
                        GUILayout.Label("Gloss Texture", EditorStyles.boldLabel);
                        EditorGUILayout.EndVertical();

                        if (_L_F_GLOT.floatValue == 1)
                        {
                            GUILayout.Space(10);

                            materialEditor.ShaderProperty(_GlossTexture, _GlossTexture.displayName);

                            GUILayout.Space(10);
                            EditorGUI.BeginDisabledGroup(_GlossTexture.textureValue == null);
                            materialEditor.ShaderProperty(_GlossTextureSoftness, _GlossTextureSoftness.displayName);
                            materialEditor.ShaderProperty(_GlossTextureFollowObjectRotation, _GlossTextureFollowObjectRotation.displayName);
                            materialEditor.ShaderProperty(_GlossTextureFollowLight, _GlossTextureFollowLight.displayName);
                            EditorGUI.EndDisabledGroup();

                        }


                    }
                    #endregion


                }

                EditorGUILayout.EndVertical();

            }
            #endregion

            //Shadow

            #region Shadow

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_shadow = EditorGUILayout.BeginVertical("Button");
            ShowShadow = EditorGUILayout.Foldout(ShowShadow, "(Shadow)", true, EditorStyles.foldout);

            if (ShowShadow)
            {

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_OverallShadowColor, _OverallShadowColor.displayName);
                materialEditor.ShaderProperty(_OverallShadowColorPower, _OverallShadowColorPower.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_SelfShadowShadowTAtViewDirection, _SelfShadowShadowTAtViewDirection.displayName);

                //Self Shadow

                #region Self Shadow

                if (_L_F_SS.floatValue == 1)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    Rect r_selfshadow = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("Self Shadow", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    if (_L_F_SS.floatValue == 1)
                    {

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_SelfShadowThreshold, _SelfShadowThreshold.displayName);
                        materialEditor.ShaderProperty(_VertexColorGreenControlSelfShadowThreshold, _VertexColorGreenControlSelfShadowThreshold.displayName);
                        materialEditor.ShaderProperty(_SelfShadowHardness, _SelfShadowHardness.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_SelfShadowRealTimeShadowColor, _SelfShadowRealTimeShadowColor.displayName);
                        materialEditor.ShaderProperty(_SelfShadowRealTimeShadowColorPower, _SelfShadowRealTimeShadowColorPower.displayName);

                    }

                }
                #endregion

                //Smooth Object Normal

                #region Smooth Object normal

                if (_L_F_SON.floatValue == 1)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if (_L_F_SS.floatValue == 0)
                    {
                        _L_F_SON.floatValue = 0;
                        targetMat.DisableKeyword("F_SS_ON");
                        _ShowNormal.floatValue = 0;
                    }

                    Rect r_smoothobjectnormal = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("Smooth Object Normal", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    if (_L_F_SON.floatValue == 1)
                    {
                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_SmoothObjectNormal, _SmoothObjectNormal.displayName);
                        materialEditor.ShaderProperty(_VertexColorBlueControlSmoothObjectNormal, _VertexColorBlueControlSmoothObjectNormal.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_XYZPosition, _XYZPosition.displayName);
                        materialEditor.ShaderProperty(_XYZHardness, _XYZHardness.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_ShowNormal, _ShowNormal.displayName);

                    }

                }
                #endregion

                //Shadow Color Texture

                #region Shadow Color Texture

                if (_L_F_SCT.floatValue == 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    Rect r_shadowcolortexture = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("Shadow Color Texture", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    if (_L_F_SCT.floatValue == 1)
                    {
                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_ShadowColorTexture, _ShadowColorTexture.displayName);
                        materialEditor.ShaderProperty(_ShadowColorTexturePower, _ShadowColorTexturePower.displayName);

                    }

                }

                #endregion

                //ShadowT

                #region ShadowT

                if (_L_F_ST.floatValue == 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    Rect r_shadowt = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("ShadowT", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    if (_L_F_ST.floatValue == 1)
                    {

                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_ShadowT, _ShadowT.displayName);
                        materialEditor.ShaderProperty(_ShadowTLightThreshold, _ShadowTLightThreshold.displayName);
                        materialEditor.ShaderProperty(_ShadowTShadowThreshold, _ShadowTShadowThreshold.displayName);
                        materialEditor.ShaderProperty(_ShadowTHardness, _ShadowTHardness.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_ShadowTColor, _ShadowTColor.displayName);
                        materialEditor.ShaderProperty(_ShadowTColorPower, _ShadowTColorPower.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_STIL, _STIL.displayName);

                        GUILayout.Space(10);
                        materialEditor.ShaderProperty(_LightFalloffAffectShadowT, _LightFalloffAffectShadowT.displayName);

                    }

                }

                #endregion

                //Shadow PTexture

                #region PTexture

                if (_L_F_PT.floatValue == 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    Rect r_ptexture = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("PTexture", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    if (_L_F_PT.floatValue == 1)
                    {
                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_PTexture, _PTexture.displayName);
                        materialEditor.ShaderProperty(_PTexturePower, _PTexturePower.displayName);
                    }

                }

                #endregion

            }

            EditorGUILayout.EndVertical();

            #endregion

            //Lighting

            #region Lighting

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_lighting = EditorGUILayout.BeginVertical("Button");
            ShowLighting = EditorGUILayout.Foldout(ShowLighting, "(Lighting)", true, EditorStyles.foldout);

            if (ShowLighting)
            {
                GUILayout.Space(10);
                materialEditor.ShaderProperty(_DirectionalLightIntensity, _DirectionalLightIntensity.displayName);
                materialEditor.ShaderProperty(_PointSpotlightIntensity, _PointSpotlightIntensity.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_L_F_UOAL, _L_F_UOAL.displayName);

                GUILayout.Space(10);

                materialEditor.ShaderProperty(_LightFalloffSoftness, _LightFalloffSoftness.displayName);


                //Custom Light Direction

                #region Custom Light Direction

                if (_L_F_CLD.floatValue == 1)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUI.BeginDisabledGroup(_L_F_CLD.floatValue == 0);

                    Rect r_customlightdirection = EditorGUILayout.BeginVertical("Button");
                    GUILayout.Label("Custom Light Direction", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();

                    if (_L_F_CLD.floatValue == 1)
                    {

                        GUILayout.Space(10);

                        materialEditor.ShaderProperty(_CustomLightDirectionIntensity, _CustomLightDirectionIntensity.displayName);
                        materialEditor.ShaderProperty(_CustomLightDirection, _CustomLightDirection.displayName);
                        materialEditor.ShaderProperty(_CustomLightDirectionFollowObjectRotation, _CustomLightDirectionFollowObjectRotation.displayName);

                    }

                    EditorGUI.EndDisabledGroup();

                }

                #endregion
            }

            EditorGUILayout.EndVertical();

            #endregion

            //Reflection

            #region FReflection

            if (_L_F_FR.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_freflection = EditorGUILayout.BeginVertical("Button");
                ShowFReflection = EditorGUILayout.Foldout(ShowFReflection, "(Reflection)", true, EditorStyles.foldout);

                if (ShowFReflection)
                {

                    GUILayout.Space(10);

                    EditorGUI.BeginDisabledGroup(_FReflection.textureValue == null);
                    materialEditor.ShaderProperty(_FReflectionIntensity, _FReflectionIntensity.displayName);
                    EditorGUI.EndDisabledGroup();

                    materialEditor.ShaderProperty(_FReflection, _FReflection.displayName);

                    GUILayout.Space(10);

                    EditorGUI.BeginDisabledGroup(_FReflection.textureValue == null);
                    materialEditor.ShaderProperty(_FReflectionRoughtness, _FReflectionRoughtness.displayName);
                    materialEditor.ShaderProperty(_RefMetallic, _RefMetallic.displayName);
                    materialEditor.ShaderProperty(_MaskFReflection, _MaskFReflection.displayName);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10);

                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            //Fresnel

            #region Rim Light

            if (_L_F_RL.floatValue == 1)
            {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                Rect r_rimlight = EditorGUILayout.BeginVertical("Button");
                ShowRimLight = EditorGUILayout.Foldout(ShowRimLight, "(Rim Light)", true, EditorStyles.foldout);

                if (ShowRimLight)
                {
                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_RimLightUnfill, _RimLightUnfill.displayName);
                    materialEditor.ShaderProperty(_RimLightSoftness, _RimLightSoftness.displayName);

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_LightAffectRimLightColor, _LightAffectRimLightColor.displayName);

                    GUILayout.Space(10);

                    materialEditor.ShaderProperty(_RimLightColor, _RimLightColor.displayName);
                    materialEditor.ShaderProperty(_RimLightColorPower, _RimLightColorPower.displayName);

                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_RimLightInLight, _RimLightInLight.displayName);

                }

                EditorGUILayout.EndVertical();

            }

            #endregion

            //See Through

            #region See Through

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            Rect r_seethrough = EditorGUILayout.BeginVertical("Button");
            ShowSeeThrough = EditorGUILayout.Foldout(ShowSeeThrough, "(See Through)", true, EditorStyles.foldout);

            if (ShowSeeThrough)
            {
                GUILayout.Space(10);

                materialEditor.ShaderProperty(_RefVal, _RefVal.displayName);
                materialEditor.ShaderProperty(_Oper, _Oper.displayName);
                materialEditor.ShaderProperty(_Compa, _Compa.displayName);

            }

            EditorGUILayout.EndVertical();

            #endregion


            GUILayout.Space(20);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            //Disable/Enable Features

            #region Disable/Enable Features

            Rect r_disableenablefeature = EditorGUILayout.BeginVertical("Button");
            ShowDisableEnable = EditorGUILayout.Foldout(ShowDisableEnable, "(Disable/Enable Features)", true, EditorStyles.foldout);

            if (ShowDisableEnable)
            {
                Rect r_mc = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_MC, _L_F_MC.displayName);
                EditorGUILayout.EndVertical();

                Rect r_nm = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_NM, _L_F_NM.displayName);
                EditorGUILayout.EndVertical();

                if (shader_name == "lite_d")
                {
                    Rect r_co = EditorGUILayout.BeginVertical("HelpBox");

                    EditorGUI.BeginChangeCheck();

                    materialEditor.ShaderProperty(_L_F_O, _L_F_O.displayName);

                    if (EditorGUI.EndChangeCheck())
                    {
                        int f_o_int = (int)_L_F_O.floatValue;

                        foreach (Material m in materialEditor.targets)
                        {

                            switch (f_o_int)
                            {
                                case 0:
                                    m.SetShaderPassEnabled("Always", false);
                                    break;
                                case 1:
                                    m.SetShaderPassEnabled("Always", true);
                                    break;
                                default:
                                    break;
                            }

                        }

                    }

                    EditorGUILayout.EndVertical();
                }
                else
                {
                    _L_F_O = null;
                }

                Rect r_ca = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_SL, _L_F_SL.displayName);
                EditorGUILayout.EndVertical();

                Rect r_o = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_GLO, _L_F_GLO.displayName);
                EditorGUILayout.EndVertical();

                Rect r_sl = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_GLOT, _L_F_GLOT.displayName);
                EditorGUILayout.EndVertical();

                Rect r_glo = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_SS, _L_F_SS.displayName);
                EditorGUILayout.EndVertical();

                Rect r_glot = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_SON, _L_F_SON.displayName);
                EditorGUILayout.EndVertical();

                if (_L_F_SS.floatValue == 0)
                {
                    _L_F_SON.floatValue = 0;
                }

                Rect r_ss = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_SCT, _L_F_SCT.displayName);
                EditorGUILayout.EndVertical();

                Rect r_son = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_ST, _L_F_ST.displayName);
                EditorGUILayout.EndVertical();

                Rect r_sct = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_PT, _L_F_PT.displayName);
                EditorGUILayout.EndVertical();

                Rect r_st = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_CLD, _L_F_CLD.displayName);
                EditorGUILayout.EndVertical();

                Rect r_spt = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_FR, _L_F_FR.displayName);
                EditorGUILayout.EndVertical();

                Rect r_cld = EditorGUILayout.BeginVertical("HelpBox");
                materialEditor.ShaderProperty(_L_F_RL, _L_F_RL.displayName);
                EditorGUILayout.EndVertical();

            }

            EditorGUILayout.EndVertical();

            #endregion

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Space(10);

            if (shader_name == "lite_d")
            {
                materialEditor.ShaderProperty(_L_F_HPSS, _L_F_HPSS.displayName);
            }
            else if (shader_name == "lite_ft")
            {
                materialEditor.ShaderProperty(_ZWrite, _ZWrite.displayName);
            }


            GUILayout.Space(10);

            materialEditor.RenderQueueField();
            materialEditor.EnableInstancingField();

            GUILayout.Space(10);

        }

        //Footbar
        #region Footbar

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        Rect r_footbar = EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Video Tutorials", "Toolbar"))
        {
            Application.OpenURL("www.youtube.com/playlist?list=PL0M1m9smMVPJ4qEkJnZObqJE5mU9uz6SY");
        }

        GUILayout.Space(5);

        if (GUILayout.Button("RealToon (User Guide).pdf", "Toolbar"))
        {
            Application.OpenURL(Application.dataPath + "/RealToon/RealToon (User Guide).pdf");
        }

        GUILayout.Space(5);

        if (GUILayout.Button(ShowUIString + " (Global)", "Toolbar"))
        {
            if (ShowUI == false)
            {
                ShowUI = true;
                ShowUIString = "Hide UI";
            }
            else
            {
                ShowUI = false;
                ShowUIString = "Show UI";
            }
        }

        EditorGUILayout.EndHorizontal();

        #endregion

        #endregion;

    }

}

#endif