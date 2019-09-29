using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIParticleEnums
{
    public enum UIParticleMaskRenderMode
    {
        JustDepth, CullingMask
    };

    public enum UIParticleMaskSourceMode
    {
        Image, RawImage, MaskTexture
    };

    public enum UIParticleMaskAlphaMode
    {
        AlphaTest, Dithering, NoAlpha, Translucency
    }
}