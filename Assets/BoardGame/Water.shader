// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33707,y:32572,varname:node_4013,prsc:2|diff-9343-RGB,voffset-7632-OUT;n:type:ShaderForge.SFN_TexCoord,id:5915,x:32244,y:32574,varname:node_5915,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:6374,x:32445,y:32574,varname:node_6374,prsc:2,spu:0,spv:0.1|UVIN-5915-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:5200,x:32651,y:32574,ptovrint:False,ptlb:DeformationTexture,ptin:_DeformationTexture,varname:node_5200,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-6374-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:42,x:32651,y:32364,varname:node_42,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:4242,x:32494,y:32773,ptovrint:False,ptlb:DeformationAmount,ptin:_DeformationAmount,varname:node_4242,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.014,max:0.1;n:type:ShaderForge.SFN_Lerp,id:8247,x:32887,y:32574,varname:node_8247,prsc:2|A-42-UVOUT,B-5200-R,T-4242-OUT;n:type:ShaderForge.SFN_Tex2d,id:9343,x:33433,y:32573,ptovrint:False,ptlb:WaterTexture,ptin:_WaterTexture,varname:node_9343,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bcd68033c0ebc104d92b57efcd2ccfbb,ntxv:0,isnm:False|UVIN-3804-OUT;n:type:ShaderForge.SFN_Slider,id:1245,x:32681,y:32891,ptovrint:False,ptlb:FlowSpeedX,ptin:_FlowSpeedX,varname:node_1245,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:4985,x:32681,y:32989,ptovrint:False,ptlb:FlowSpeedY,ptin:_FlowSpeedY,varname:node_4985,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:1,max:1;n:type:ShaderForge.SFN_Append,id:6973,x:33035,y:32905,varname:node_6973,prsc:2|A-1245-OUT,B-4985-OUT;n:type:ShaderForge.SFN_Add,id:3804,x:33216,y:32573,varname:node_3804,prsc:2|A-8247-OUT,B-4913-OUT;n:type:ShaderForge.SFN_Time,id:7626,x:33035,y:32776,varname:node_7626,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4913,x:33216,y:32776,varname:node_4913,prsc:2|A-7626-T,B-6973-OUT;n:type:ShaderForge.SFN_Color,id:9842,x:32776,y:32513,ptovrint:False,ptlb:DiffuseColor_copy,ptin:_DiffuseColor_copy,varname:_DiffuseColor_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Vector3,id:8932,x:33059,y:33097,varname:node_8932,prsc:2,v1:0,v2:10,v3:0;n:type:ShaderForge.SFN_Multiply,id:7632,x:33282,y:33097,varname:node_7632,prsc:2|A-8932-OUT,B-8342-RGB;n:type:ShaderForge.SFN_Tex2d,id:8342,x:32738,y:33338,ptovrint:False,ptlb:WaveTexture,ptin:_WaveTexture,varname:node_8342,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:31f5a8611c4ed1245b18456206e798dc,ntxv:0,isnm:False|UVIN-7468-UVOUT;n:type:ShaderForge.SFN_Panner,id:7468,x:32551,y:33338,varname:node_7468,prsc:2,spu:0,spv:1|UVIN-5915-UVOUT;proporder:5200-4242-9343-1245-4985-8342;pass:END;sub:END;*/

Shader "More Mountains/Water" {
    Properties {
        _DeformationTexture ("DeformationTexture", 2D) = "white" {}
        _DeformationAmount ("DeformationAmount", Range(0, 0.1)) = 0.014
        _WaterTexture ("WaterTexture", 2D) = "white" {}
        _FlowSpeedX ("FlowSpeedX", Range(-1, 1)) = 0
        _FlowSpeedY ("FlowSpeedY", Range(-1, 1)) = 1
        _WaveTexture ("WaveTexture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _DeformationTexture; uniform float4 _DeformationTexture_ST;
            uniform float _DeformationAmount;
            uniform sampler2D _WaterTexture; uniform float4 _WaterTexture_ST;
            uniform float _FlowSpeedX;
            uniform float _FlowSpeedY;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4708 = _Time + _TimeEditor;
                float2 node_7468 = (o.uv0+node_4708.g*float2(0,1));
                float4 _WaveTexture_var = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_7468, _WaveTexture),0.0,0));
                v.vertex.xyz += (float3(0,10,0)*_WaveTexture_var.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_4708 = _Time + _TimeEditor;
                float2 node_6374 = (i.uv0+node_4708.g*float2(0,0.1));
                float4 _DeformationTexture_var = tex2D(_DeformationTexture,TRANSFORM_TEX(node_6374, _DeformationTexture));
                float4 node_7626 = _Time + _TimeEditor;
                float2 node_3804 = (lerp(i.uv0,float2(_DeformationTexture_var.r,_DeformationTexture_var.r),_DeformationAmount)+(node_7626.g*float2(_FlowSpeedX,_FlowSpeedY)));
                float4 _WaterTexture_var = tex2D(_WaterTexture,TRANSFORM_TEX(node_3804, _WaterTexture));
                float3 diffuseColor = _WaterTexture_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _DeformationTexture; uniform float4 _DeformationTexture_ST;
            uniform float _DeformationAmount;
            uniform sampler2D _WaterTexture; uniform float4 _WaterTexture_ST;
            uniform float _FlowSpeedX;
            uniform float _FlowSpeedY;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_7309 = _Time + _TimeEditor;
                float2 node_7468 = (o.uv0+node_7309.g*float2(0,1));
                float4 _WaveTexture_var = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_7468, _WaveTexture),0.0,0));
                v.vertex.xyz += (float3(0,10,0)*_WaveTexture_var.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 attenColor = _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_7309 = _Time + _TimeEditor;
                float2 node_6374 = (i.uv0+node_7309.g*float2(0,0.1));
                float4 _DeformationTexture_var = tex2D(_DeformationTexture,TRANSFORM_TEX(node_6374, _DeformationTexture));
                float4 node_7626 = _Time + _TimeEditor;
                float2 node_3804 = (lerp(i.uv0,float2(_DeformationTexture_var.r,_DeformationTexture_var.r),_DeformationAmount)+(node_7626.g*float2(_FlowSpeedX,_FlowSpeedY)));
                float4 _WaterTexture_var = tex2D(_WaterTexture,TRANSFORM_TEX(node_3804, _WaterTexture));
                float3 diffuseColor = _WaterTexture_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_5136 = _Time + _TimeEditor;
                float2 node_7468 = (o.uv0+node_5136.g*float2(0,1));
                float4 _WaveTexture_var = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_7468, _WaveTexture),0.0,0));
                v.vertex.xyz += (float3(0,10,0)*_WaveTexture_var.rgb);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
