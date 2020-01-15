Shader "Hidden/LensFlareCreate" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
	SubShader {
		Pass {
			Blend One One, One One
			ZClip Off
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 18483
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD0_1;
					varying highp vec2 xlv_TEXCOORD0_2;
					varying highp vec2 xlv_TEXCOORD0_3;
					void main ()
					{
					  highp vec2 tmpvar_1;
					  highp vec2 tmpvar_2;
					  highp vec2 tmpvar_3;
					  highp vec2 tmpvar_4;
					  mediump vec2 tmpvar_5;
					  tmpvar_5 = (_glesMultiTexCoord0.xy - 0.5);
					  tmpvar_1 = ((tmpvar_5 * -0.85) + 0.5);
					  tmpvar_2 = ((tmpvar_5 * -1.45) + 0.5);
					  tmpvar_3 = ((tmpvar_5 * -2.55) + 0.5);
					  tmpvar_4 = ((tmpvar_5 * -4.15) + 0.5);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD0_1 = tmpvar_2;
					  xlv_TEXCOORD0_2 = tmpvar_3;
					  xlv_TEXCOORD0_3 = tmpvar_4;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 colorA;
					uniform lowp vec4 colorB;
					uniform lowp vec4 colorC;
					uniform lowp vec4 colorD;
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD0_1;
					varying highp vec2 xlv_TEXCOORD0_2;
					varying highp vec2 xlv_TEXCOORD0_3;
					void main ()
					{
					  lowp vec4 color_1;
					  color_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * colorA);
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_1) * colorB));
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_2) * colorC));
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_3) * colorD));
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles hw_tier01 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD0_1;
					varying highp vec2 xlv_TEXCOORD0_2;
					varying highp vec2 xlv_TEXCOORD0_3;
					void main ()
					{
					  highp vec2 tmpvar_1;
					  highp vec2 tmpvar_2;
					  highp vec2 tmpvar_3;
					  highp vec2 tmpvar_4;
					  mediump vec2 tmpvar_5;
					  tmpvar_5 = (_glesMultiTexCoord0.xy - 0.5);
					  tmpvar_1 = ((tmpvar_5 * -0.85) + 0.5);
					  tmpvar_2 = ((tmpvar_5 * -1.45) + 0.5);
					  tmpvar_3 = ((tmpvar_5 * -2.55) + 0.5);
					  tmpvar_4 = ((tmpvar_5 * -4.15) + 0.5);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD0_1 = tmpvar_2;
					  xlv_TEXCOORD0_2 = tmpvar_3;
					  xlv_TEXCOORD0_3 = tmpvar_4;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 colorA;
					uniform lowp vec4 colorB;
					uniform lowp vec4 colorC;
					uniform lowp vec4 colorD;
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD0_1;
					varying highp vec2 xlv_TEXCOORD0_2;
					varying highp vec2 xlv_TEXCOORD0_3;
					void main ()
					{
					  lowp vec4 color_1;
					  color_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * colorA);
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_1) * colorB));
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_2) * colorC));
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_3) * colorD));
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles hw_tier02 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD0_1;
					varying highp vec2 xlv_TEXCOORD0_2;
					varying highp vec2 xlv_TEXCOORD0_3;
					void main ()
					{
					  highp vec2 tmpvar_1;
					  highp vec2 tmpvar_2;
					  highp vec2 tmpvar_3;
					  highp vec2 tmpvar_4;
					  mediump vec2 tmpvar_5;
					  tmpvar_5 = (_glesMultiTexCoord0.xy - 0.5);
					  tmpvar_1 = ((tmpvar_5 * -0.85) + 0.5);
					  tmpvar_2 = ((tmpvar_5 * -1.45) + 0.5);
					  tmpvar_3 = ((tmpvar_5 * -2.55) + 0.5);
					  tmpvar_4 = ((tmpvar_5 * -4.15) + 0.5);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD0_1 = tmpvar_2;
					  xlv_TEXCOORD0_2 = tmpvar_3;
					  xlv_TEXCOORD0_3 = tmpvar_4;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 colorA;
					uniform lowp vec4 colorB;
					uniform lowp vec4 colorC;
					uniform lowp vec4 colorD;
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD0_1;
					varying highp vec2 xlv_TEXCOORD0_2;
					varying highp vec2 xlv_TEXCOORD0_3;
					void main ()
					{
					  lowp vec4 color_1;
					  color_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * colorA);
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_1) * colorB));
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_2) * colorC));
					  color_1 = (color_1 + (texture2D (_MainTex, xlv_TEXCOORD0_3) * colorD));
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec2 vs_TEXCOORD2;
					out highp vec2 vs_TEXCOORD3;
					vec4 u_xlat0;
					mediump vec4 u_xlat16_0;
					mediump vec4 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    gl_Position = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat16_0 = in_TEXCOORD0.xyxy + vec4(-0.5, -0.5, -0.5, -0.5);
					    u_xlat16_1 = u_xlat16_0.zwzw * vec4(-0.850000024, -0.850000024, -1.45000005, -1.45000005) + vec4(0.5, 0.5, 0.5, 0.5);
					    u_xlat16_0 = u_xlat16_0 * vec4(-2.54999995, -2.54999995, -4.1500001, -4.1500001) + vec4(0.5, 0.5, 0.5, 0.5);
					    vs_TEXCOORD0.xy = u_xlat16_1.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.zw;
					    vs_TEXCOORD2.xy = u_xlat16_0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_0.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	lowp vec4 colorA;
					uniform 	lowp vec4 colorB;
					uniform 	lowp vec4 colorC;
					uniform 	lowp vec4 colorD;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					in highp vec2 vs_TEXCOORD2;
					in highp vec2 vs_TEXCOORD3;
					layout(location = 0) out lowp vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * colorB;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_0 = u_xlat10_1 * colorA + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat10_1 * colorC + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat10_1 * colorD + u_xlat16_0;
					    SV_Target0 = u_xlat16_0;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec2 vs_TEXCOORD2;
					out highp vec2 vs_TEXCOORD3;
					vec4 u_xlat0;
					mediump vec4 u_xlat16_0;
					mediump vec4 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    gl_Position = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat16_0 = in_TEXCOORD0.xyxy + vec4(-0.5, -0.5, -0.5, -0.5);
					    u_xlat16_1 = u_xlat16_0.zwzw * vec4(-0.850000024, -0.850000024, -1.45000005, -1.45000005) + vec4(0.5, 0.5, 0.5, 0.5);
					    u_xlat16_0 = u_xlat16_0 * vec4(-2.54999995, -2.54999995, -4.1500001, -4.1500001) + vec4(0.5, 0.5, 0.5, 0.5);
					    vs_TEXCOORD0.xy = u_xlat16_1.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.zw;
					    vs_TEXCOORD2.xy = u_xlat16_0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_0.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	lowp vec4 colorA;
					uniform 	lowp vec4 colorB;
					uniform 	lowp vec4 colorC;
					uniform 	lowp vec4 colorD;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					in highp vec2 vs_TEXCOORD2;
					in highp vec2 vs_TEXCOORD3;
					layout(location = 0) out lowp vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * colorB;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_0 = u_xlat10_1 * colorA + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat10_1 * colorC + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat10_1 * colorD + u_xlat16_0;
					    SV_Target0 = u_xlat16_0;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out highp vec2 vs_TEXCOORD0;
					out highp vec2 vs_TEXCOORD1;
					out highp vec2 vs_TEXCOORD2;
					out highp vec2 vs_TEXCOORD3;
					vec4 u_xlat0;
					mediump vec4 u_xlat16_0;
					mediump vec4 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    gl_Position = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat16_0 = in_TEXCOORD0.xyxy + vec4(-0.5, -0.5, -0.5, -0.5);
					    u_xlat16_1 = u_xlat16_0.zwzw * vec4(-0.850000024, -0.850000024, -1.45000005, -1.45000005) + vec4(0.5, 0.5, 0.5, 0.5);
					    u_xlat16_0 = u_xlat16_0 * vec4(-2.54999995, -2.54999995, -4.1500001, -4.1500001) + vec4(0.5, 0.5, 0.5, 0.5);
					    vs_TEXCOORD0.xy = u_xlat16_1.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.zw;
					    vs_TEXCOORD2.xy = u_xlat16_0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_0.zw;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	lowp vec4 colorA;
					uniform 	lowp vec4 colorB;
					uniform 	lowp vec4 colorC;
					uniform 	lowp vec4 colorD;
					uniform lowp sampler2D _MainTex;
					in highp vec2 vs_TEXCOORD0;
					in highp vec2 vs_TEXCOORD1;
					in highp vec2 vs_TEXCOORD2;
					in highp vec2 vs_TEXCOORD3;
					layout(location = 0) out lowp vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * colorB;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_0 = u_xlat10_1 * colorA + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat10_1 * colorC + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat10_1 * colorD + u_xlat16_0;
					    SV_Target0 = u_xlat16_0;
					    return;
					}
					
					#endif"
				}
			}
			Program "fp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES"
				}
				SubProgram "gles hw_tier01 " {
					"!!GLES"
				}
				SubProgram "gles hw_tier02 " {
					"!!GLES"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!GLES3"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!GLES3"
				}
			}
		}
	}
}