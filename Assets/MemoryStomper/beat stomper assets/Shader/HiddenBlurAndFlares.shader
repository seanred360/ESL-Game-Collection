Shader "Hidden/BlurAndFlares" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
		_NonBlurredTex ("Base (RGB)", 2D) = "" {}
	}
	SubShader {
		Pass {
			ZClip Off
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 28356
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  gl_FragData[0] = (color_1 / (1.5 + dot (color_1.xyz, vec3(0.22, 0.707, 0.071))));
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
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  gl_FragData[0] = (color_1 / (1.5 + dot (color_1.xyz, vec3(0.22, 0.707, 0.071))));
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
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  gl_FragData[0] = (color_1 / (1.5 + dot (color_1.xyz, vec3(0.22, 0.707, 0.071))));
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
					out mediump vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_1 = dot(u_xlat10_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_1 = u_xlat16_1 + 1.5;
					    SV_Target0 = u_xlat10_0 / vec4(u_xlat16_1);
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
					out mediump vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_1 = dot(u_xlat10_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_1 = u_xlat16_1 + 1.5;
					    SV_Target0 = u_xlat10_0 / vec4(u_xlat16_1);
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
					out mediump vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					layout(location = 0) out mediump vec4 SV_Target0;
					lowp vec4 u_xlat10_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_1 = dot(u_xlat10_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_1 = u_xlat16_1 + 1.5;
					    SV_Target0 = u_xlat10_0 / vec4(u_xlat16_1);
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
		Pass {
			ZClip Off
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 85760
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform mediump vec4 _Offsets;
					uniform mediump float _StretchWidth;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump float tmpvar_1;
					  tmpvar_1 = (_StretchWidth * 2.0);
					  mediump float tmpvar_2;
					  tmpvar_2 = (_StretchWidth * 4.0);
					  mediump float tmpvar_3;
					  tmpvar_3 = (_StretchWidth * 6.0);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  mediump vec4 tmpvar_9;
					  tmpvar_9 = max (max (max (color_1, tmpvar_3), max (tmpvar_4, tmpvar_5)), max (max (tmpvar_6, tmpvar_7), tmpvar_8));
					  color_1 = tmpvar_9;
					  gl_FragData[0] = tmpvar_9;
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
					uniform mediump vec4 _Offsets;
					uniform mediump float _StretchWidth;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump float tmpvar_1;
					  tmpvar_1 = (_StretchWidth * 2.0);
					  mediump float tmpvar_2;
					  tmpvar_2 = (_StretchWidth * 4.0);
					  mediump float tmpvar_3;
					  tmpvar_3 = (_StretchWidth * 6.0);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  mediump vec4 tmpvar_9;
					  tmpvar_9 = max (max (max (color_1, tmpvar_3), max (tmpvar_4, tmpvar_5)), max (max (tmpvar_6, tmpvar_7), tmpvar_8));
					  color_1 = tmpvar_9;
					  gl_FragData[0] = tmpvar_9;
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
					uniform mediump vec4 _Offsets;
					uniform mediump float _StretchWidth;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump float tmpvar_1;
					  tmpvar_1 = (_StretchWidth * 2.0);
					  mediump float tmpvar_2;
					  tmpvar_2 = (_StretchWidth * 4.0);
					  mediump float tmpvar_3;
					  tmpvar_3 = (_StretchWidth * 6.0);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  mediump vec4 tmpvar_9;
					  tmpvar_9 = max (max (max (color_1, tmpvar_3), max (tmpvar_4, tmpvar_5)), max (max (tmpvar_6, tmpvar_7), tmpvar_8));
					  color_1 = tmpvar_9;
					  gl_FragData[0] = tmpvar_9;
					}
					
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump float _StretchWidth;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec4 u_xlat16_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1 = _StretchWidth + _StretchWidth;
					    vs_TEXCOORD1.xy = vec2(u_xlat16_1) * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-vec2(u_xlat16_1)) * _Offsets.xy + in_TEXCOORD0.xy;
					    u_xlat16_0 = vec4(_StretchWidth) * vec4(4.0, 4.0, 6.0, 6.0);
					    vs_TEXCOORD3.xy = u_xlat16_0.xy * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_0.xy) * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_0.zw * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_0.zw) * _Offsets.xy + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = max(u_xlat10_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
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
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump float _StretchWidth;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec4 u_xlat16_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1 = _StretchWidth + _StretchWidth;
					    vs_TEXCOORD1.xy = vec2(u_xlat16_1) * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-vec2(u_xlat16_1)) * _Offsets.xy + in_TEXCOORD0.xy;
					    u_xlat16_0 = vec4(_StretchWidth) * vec4(4.0, 4.0, 6.0, 6.0);
					    vs_TEXCOORD3.xy = u_xlat16_0.xy * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_0.xy) * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_0.zw * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_0.zw) * _Offsets.xy + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = max(u_xlat10_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
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
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump float _StretchWidth;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec4 u_xlat16_0;
					mediump float u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1 = _StretchWidth + _StretchWidth;
					    vs_TEXCOORD1.xy = vec2(u_xlat16_1) * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-vec2(u_xlat16_1)) * _Offsets.xy + in_TEXCOORD0.xy;
					    u_xlat16_0 = vec4(_StretchWidth) * vec4(4.0, 4.0, 6.0, 6.0);
					    vs_TEXCOORD3.xy = u_xlat16_0.xy * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_0.xy) * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_0.zw * _Offsets.xy + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_0.zw) * _Offsets.xy + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = max(u_xlat10_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = max(u_xlat16_0, u_xlat10_1);
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
		Pass {
			ZClip Off
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 171256
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform mediump vec4 _Offsets;
					uniform mediump vec4 _MainTex_TexelSize;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = (0.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_2;
					  tmpvar_2 = (1.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (2.5 * _MainTex_TexelSize.xy);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform mediump vec4 _TintColor;
					uniform mediump vec2 _Threshhold;
					uniform mediump float _Saturation;
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  color_1 = (color_1 + tmpvar_3);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  color_1 = (color_1 + tmpvar_4);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  color_1 = (color_1 + tmpvar_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  color_1 = (color_1 + tmpvar_6);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  color_1 = (color_1 + tmpvar_7);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  color_1 = (color_1 + tmpvar_8);
					  mediump vec4 tmpvar_9;
					  tmpvar_9 = max (((color_1 / 7.0) - _Threshhold.xxxx), vec4(0.0, 0.0, 0.0, 0.0));
					  color_1.w = tmpvar_9.w;
					  color_1.xyz = (mix (vec3(dot (tmpvar_9.xyz, vec3(0.22, 0.707, 0.071))), tmpvar_9.xyz, vec3(_Saturation)) * _TintColor.xyz);
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
					uniform mediump vec4 _Offsets;
					uniform mediump vec4 _MainTex_TexelSize;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = (0.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_2;
					  tmpvar_2 = (1.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (2.5 * _MainTex_TexelSize.xy);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform mediump vec4 _TintColor;
					uniform mediump vec2 _Threshhold;
					uniform mediump float _Saturation;
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  color_1 = (color_1 + tmpvar_3);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  color_1 = (color_1 + tmpvar_4);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  color_1 = (color_1 + tmpvar_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  color_1 = (color_1 + tmpvar_6);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  color_1 = (color_1 + tmpvar_7);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  color_1 = (color_1 + tmpvar_8);
					  mediump vec4 tmpvar_9;
					  tmpvar_9 = max (((color_1 / 7.0) - _Threshhold.xxxx), vec4(0.0, 0.0, 0.0, 0.0));
					  color_1.w = tmpvar_9.w;
					  color_1.xyz = (mix (vec3(dot (tmpvar_9.xyz, vec3(0.22, 0.707, 0.071))), tmpvar_9.xyz, vec3(_Saturation)) * _TintColor.xyz);
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
					uniform mediump vec4 _Offsets;
					uniform mediump vec4 _MainTex_TexelSize;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = (0.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_2;
					  tmpvar_2 = (1.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (2.5 * _MainTex_TexelSize.xy);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform mediump vec4 _TintColor;
					uniform mediump vec2 _Threshhold;
					uniform mediump float _Saturation;
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  color_1 = (color_1 + tmpvar_3);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  color_1 = (color_1 + tmpvar_4);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  color_1 = (color_1 + tmpvar_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  color_1 = (color_1 + tmpvar_6);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  color_1 = (color_1 + tmpvar_7);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  color_1 = (color_1 + tmpvar_8);
					  mediump vec4 tmpvar_9;
					  tmpvar_9 = max (((color_1 / 7.0) - _Threshhold.xxxx), vec4(0.0, 0.0, 0.0, 0.0));
					  color_1.w = tmpvar_9.w;
					  color_1.xyz = (mix (vec3(dot (tmpvar_9.xyz, vec3(0.22, 0.707, 0.071))), tmpvar_9.xyz, vec3(_Saturation)) * _TintColor.xyz);
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump vec4 _MainTex_TexelSize;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec2 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1.xy = _Offsets.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.xy * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-u_xlat16_1.xy) * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_1.xy * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_1.xy) * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_1.xy * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_1.xy) * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump vec4 _TintColor;
					uniform 	mediump vec2 _Threshhold;
					uniform 	mediump float _Saturation;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					mediump vec3 u_xlat16_2;
					mediump vec3 u_xlat16_5;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat16_0 = u_xlat16_0 * vec4(0.142857149, 0.142857149, 0.142857149, 0.142857149) + (-_Threshhold.xxyx.yyyy);
					    u_xlat16_0 = max(u_xlat16_0, vec4(0.0, 0.0, 0.0, 0.0));
					    u_xlat16_2.x = dot(u_xlat16_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_5.xyz = u_xlat16_0.xyz + (-u_xlat16_2.xxx);
					    SV_Target0.w = u_xlat16_0.w;
					    u_xlat16_2.xyz = vec3(vec3(_Saturation, _Saturation, _Saturation)) * u_xlat16_5.xyz + u_xlat16_2.xxx;
					    SV_Target0.xyz = u_xlat16_2.xyz * _TintColor.xyz;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump vec4 _MainTex_TexelSize;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec2 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1.xy = _Offsets.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.xy * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-u_xlat16_1.xy) * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_1.xy * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_1.xy) * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_1.xy * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_1.xy) * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump vec4 _TintColor;
					uniform 	mediump vec2 _Threshhold;
					uniform 	mediump float _Saturation;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					mediump vec3 u_xlat16_2;
					mediump vec3 u_xlat16_5;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat16_0 = u_xlat16_0 * vec4(0.142857149, 0.142857149, 0.142857149, 0.142857149) + (-_Threshhold.xxyx.yyyy);
					    u_xlat16_0 = max(u_xlat16_0, vec4(0.0, 0.0, 0.0, 0.0));
					    u_xlat16_2.x = dot(u_xlat16_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_5.xyz = u_xlat16_0.xyz + (-u_xlat16_2.xxx);
					    SV_Target0.w = u_xlat16_0.w;
					    u_xlat16_2.xyz = vec3(vec3(_Saturation, _Saturation, _Saturation)) * u_xlat16_5.xyz + u_xlat16_2.xxx;
					    SV_Target0.xyz = u_xlat16_2.xyz * _TintColor.xyz;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump vec4 _MainTex_TexelSize;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec2 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1.xy = _Offsets.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.xy * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-u_xlat16_1.xy) * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_1.xy * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_1.xy) * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_1.xy * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_1.xy) * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	mediump vec4 _TintColor;
					uniform 	mediump vec2 _Threshhold;
					uniform 	mediump float _Saturation;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					mediump vec3 u_xlat16_2;
					mediump vec3 u_xlat16_5;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat16_0 = u_xlat16_0 * vec4(0.142857149, 0.142857149, 0.142857149, 0.142857149) + (-_Threshhold.xxyx.yyyy);
					    u_xlat16_0 = max(u_xlat16_0, vec4(0.0, 0.0, 0.0, 0.0));
					    u_xlat16_2.x = dot(u_xlat16_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_5.xyz = u_xlat16_0.xyz + (-u_xlat16_2.xxx);
					    SV_Target0.w = u_xlat16_0.w;
					    u_xlat16_2.xyz = vec3(vec3(_Saturation, _Saturation, _Saturation)) * u_xlat16_5.xyz + u_xlat16_2.xxx;
					    SV_Target0.xyz = u_xlat16_2.xyz * _TintColor.xyz;
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
		Pass {
			ZClip Off
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 216679
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform mediump vec4 _Offsets;
					uniform mediump vec4 _MainTex_TexelSize;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = (0.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_2;
					  tmpvar_2 = (1.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (2.5 * _MainTex_TexelSize.xy);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  color_1 = (color_1 + tmpvar_3);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  color_1 = (color_1 + tmpvar_4);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  color_1 = (color_1 + tmpvar_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  color_1 = (color_1 + tmpvar_6);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  color_1 = (color_1 + tmpvar_7);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  color_1 = (color_1 + tmpvar_8);
					  gl_FragData[0] = (color_1 / (7.5 + dot (color_1.xyz, vec3(0.22, 0.707, 0.071))));
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
					uniform mediump vec4 _Offsets;
					uniform mediump vec4 _MainTex_TexelSize;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = (0.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_2;
					  tmpvar_2 = (1.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (2.5 * _MainTex_TexelSize.xy);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  color_1 = (color_1 + tmpvar_3);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  color_1 = (color_1 + tmpvar_4);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  color_1 = (color_1 + tmpvar_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  color_1 = (color_1 + tmpvar_6);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  color_1 = (color_1 + tmpvar_7);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  color_1 = (color_1 + tmpvar_8);
					  gl_FragData[0] = (color_1 / (7.5 + dot (color_1.xyz, vec3(0.22, 0.707, 0.071))));
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
					uniform mediump vec4 _Offsets;
					uniform mediump vec4 _MainTex_TexelSize;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = (0.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_2;
					  tmpvar_2 = (1.5 * _MainTex_TexelSize.xy);
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (2.5 * _MainTex_TexelSize.xy);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD0_1 = (_glesMultiTexCoord0.xy + (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_2 = (_glesMultiTexCoord0.xy - (tmpvar_1 * _Offsets.xy));
					  xlv_TEXCOORD0_3 = (_glesMultiTexCoord0.xy + (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_4 = (_glesMultiTexCoord0.xy - (tmpvar_2 * _Offsets.xy));
					  xlv_TEXCOORD0_5 = (_glesMultiTexCoord0.xy + (tmpvar_3 * _Offsets.xy));
					  xlv_TEXCOORD0_6 = (_glesMultiTexCoord0.xy - (tmpvar_3 * _Offsets.xy));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD0_1;
					varying mediump vec2 xlv_TEXCOORD0_2;
					varying mediump vec2 xlv_TEXCOORD0_3;
					varying mediump vec2 xlv_TEXCOORD0_4;
					varying mediump vec2 xlv_TEXCOORD0_5;
					varying mediump vec2 xlv_TEXCOORD0_6;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = tmpvar_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0_1);
					  color_1 = (color_1 + tmpvar_3);
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0_2);
					  color_1 = (color_1 + tmpvar_4);
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0_3);
					  color_1 = (color_1 + tmpvar_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0_4);
					  color_1 = (color_1 + tmpvar_6);
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0_5);
					  color_1 = (color_1 + tmpvar_7);
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0_6);
					  color_1 = (color_1 + tmpvar_8);
					  gl_FragData[0] = (color_1 / (7.5 + dot (color_1.xyz, vec3(0.22, 0.707, 0.071))));
					}
					
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump vec4 _MainTex_TexelSize;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec2 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1.xy = _Offsets.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.xy * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-u_xlat16_1.xy) * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_1.xy * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_1.xy) * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_1.xy * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_1.xy) * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat16_2 = dot(u_xlat16_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_2 = u_xlat16_2 + 7.5;
					    SV_Target0 = u_xlat16_0 / vec4(u_xlat16_2);
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump vec4 _MainTex_TexelSize;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec2 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1.xy = _Offsets.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.xy * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-u_xlat16_1.xy) * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_1.xy * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_1.xy) * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_1.xy * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_1.xy) * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat16_2 = dot(u_xlat16_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_2 = u_xlat16_2 + 7.5;
					    SV_Target0 = u_xlat16_0 / vec4(u_xlat16_2);
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					uniform 	mediump vec4 _MainTex_TexelSize;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD1;
					out mediump vec2 vs_TEXCOORD2;
					out mediump vec2 vs_TEXCOORD3;
					out mediump vec2 vs_TEXCOORD4;
					out mediump vec2 vs_TEXCOORD5;
					out mediump vec2 vs_TEXCOORD6;
					vec4 u_xlat0;
					mediump vec2 u_xlat16_1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat16_1.xy = _Offsets.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat16_1.xy * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = (-u_xlat16_1.xy) * vec2(0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = u_xlat16_1.xy * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD4.xy = (-u_xlat16_1.xy) * vec2(1.5, 1.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD5.xy = u_xlat16_1.xy * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD6.xy = (-u_xlat16_1.xy) * vec2(2.5, 2.5) + in_TEXCOORD0.xy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec2 vs_TEXCOORD1;
					in mediump vec2 vs_TEXCOORD2;
					in mediump vec2 vs_TEXCOORD3;
					in mediump vec2 vs_TEXCOORD4;
					in mediump vec2 vs_TEXCOORD5;
					in mediump vec2 vs_TEXCOORD6;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					mediump float u_xlat16_2;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD5.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD6.xy);
					    u_xlat16_0 = u_xlat16_0 + u_xlat10_1;
					    u_xlat16_2 = dot(u_xlat16_0.xyz, vec3(0.219999999, 0.707000017, 0.0710000023));
					    u_xlat16_2 = u_xlat16_2 + 7.5;
					    SV_Target0 = u_xlat16_0 / vec4(u_xlat16_2);
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
		Pass {
			ZClip Off
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 304444
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform mediump vec4 _Offsets;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying mediump vec4 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  mediump vec4 tmpvar_1;
					  tmpvar_1 = (_Offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0));
					  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + tmpvar_1);
					  xlv_TEXCOORD2 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 2.0));
					  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 3.0));
					  xlv_TEXCOORD4 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 5.0));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying mediump vec4 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = (0.225 * tmpvar_2);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
					  color_1 = (color_1 + (0.15 * tmpvar_3));
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
					  color_1 = (color_1 + (0.15 * tmpvar_4));
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
					  color_1 = (color_1 + (0.11 * tmpvar_5));
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD2.zw);
					  color_1 = (color_1 + (0.11 * tmpvar_6));
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD3.xy);
					  color_1 = (color_1 + (0.075 * tmpvar_7));
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD3.zw);
					  color_1 = (color_1 + (0.075 * tmpvar_8));
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
					  color_1 = (color_1 + (0.0525 * tmpvar_9));
					  lowp vec4 tmpvar_10;
					  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD4.zw);
					  color_1 = (color_1 + (0.0525 * tmpvar_10));
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
					uniform mediump vec4 _Offsets;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying mediump vec4 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  mediump vec4 tmpvar_1;
					  tmpvar_1 = (_Offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0));
					  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + tmpvar_1);
					  xlv_TEXCOORD2 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 2.0));
					  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 3.0));
					  xlv_TEXCOORD4 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 5.0));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying mediump vec4 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = (0.225 * tmpvar_2);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
					  color_1 = (color_1 + (0.15 * tmpvar_3));
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
					  color_1 = (color_1 + (0.15 * tmpvar_4));
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
					  color_1 = (color_1 + (0.11 * tmpvar_5));
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD2.zw);
					  color_1 = (color_1 + (0.11 * tmpvar_6));
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD3.xy);
					  color_1 = (color_1 + (0.075 * tmpvar_7));
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD3.zw);
					  color_1 = (color_1 + (0.075 * tmpvar_8));
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
					  color_1 = (color_1 + (0.0525 * tmpvar_9));
					  lowp vec4 tmpvar_10;
					  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD4.zw);
					  color_1 = (color_1 + (0.0525 * tmpvar_10));
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
					uniform mediump vec4 _Offsets;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying mediump vec4 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  mediump vec4 tmpvar_1;
					  tmpvar_1 = (_Offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0));
					  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + tmpvar_1);
					  xlv_TEXCOORD2 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 2.0));
					  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 3.0));
					  xlv_TEXCOORD4 = (_glesMultiTexCoord0.xyxy + (tmpvar_1 * 5.0));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying mediump vec4 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 color_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  color_1 = (0.225 * tmpvar_2);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
					  color_1 = (color_1 + (0.15 * tmpvar_3));
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
					  color_1 = (color_1 + (0.15 * tmpvar_4));
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
					  color_1 = (color_1 + (0.11 * tmpvar_5));
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD2.zw);
					  color_1 = (color_1 + (0.11 * tmpvar_6));
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD3.xy);
					  color_1 = (color_1 + (0.075 * tmpvar_7));
					  lowp vec4 tmpvar_8;
					  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD3.zw);
					  color_1 = (color_1 + (0.075 * tmpvar_8));
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
					  color_1 = (color_1 + (0.0525 * tmpvar_9));
					  lowp vec4 tmpvar_10;
					  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD4.zw);
					  color_1 = (color_1 + (0.0525 * tmpvar_10));
					  gl_FragData[0] = color_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 hlslcc_mtx4x4glstate_matrix_mvp[4];
					uniform 	mediump vec4 _Offsets;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					out mediump vec4 vs_TEXCOORD3;
					out mediump vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_TEXCOORD1 = _Offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = _Offsets.xyxy * vec4(2.0, 2.0, -2.0, -2.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = _Offsets.xyxy * vec4(3.0, 3.0, -3.0, -3.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD4 = _Offsets.xyxy * vec4(5.0, 5.0, -5.0, -5.0) + in_TEXCOORD0.xyxy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					in mediump vec4 vs_TEXCOORD2;
					in mediump vec4 vs_TEXCOORD3;
					in mediump vec4 vs_TEXCOORD4;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * vec4(0.150000006, 0.150000006, 0.150000006, 0.150000006);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.224999994, 0.224999994, 0.224999994, 0.224999994) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.150000006, 0.150000006, 0.150000006, 0.150000006) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.109999999, 0.109999999, 0.109999999, 0.109999999) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.109999999, 0.109999999, 0.109999999, 0.109999999) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.075000003, 0.075000003, 0.075000003, 0.075000003) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.075000003, 0.075000003, 0.075000003, 0.075000003) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.0524999984, 0.0524999984, 0.0524999984, 0.0524999984) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.0524999984, 0.0524999984, 0.0524999984, 0.0524999984) + u_xlat16_0;
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
					uniform 	mediump vec4 _Offsets;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					out mediump vec4 vs_TEXCOORD3;
					out mediump vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_TEXCOORD1 = _Offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = _Offsets.xyxy * vec4(2.0, 2.0, -2.0, -2.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = _Offsets.xyxy * vec4(3.0, 3.0, -3.0, -3.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD4 = _Offsets.xyxy * vec4(5.0, 5.0, -5.0, -5.0) + in_TEXCOORD0.xyxy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					in mediump vec4 vs_TEXCOORD2;
					in mediump vec4 vs_TEXCOORD3;
					in mediump vec4 vs_TEXCOORD4;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * vec4(0.150000006, 0.150000006, 0.150000006, 0.150000006);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.224999994, 0.224999994, 0.224999994, 0.224999994) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.150000006, 0.150000006, 0.150000006, 0.150000006) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.109999999, 0.109999999, 0.109999999, 0.109999999) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.109999999, 0.109999999, 0.109999999, 0.109999999) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.075000003, 0.075000003, 0.075000003, 0.075000003) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.075000003, 0.075000003, 0.075000003, 0.075000003) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.0524999984, 0.0524999984, 0.0524999984, 0.0524999984) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.0524999984, 0.0524999984, 0.0524999984, 0.0524999984) + u_xlat16_0;
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
					uniform 	mediump vec4 _Offsets;
					in highp vec4 in_POSITION0;
					in mediump vec2 in_TEXCOORD0;
					out mediump vec2 vs_TEXCOORD0;
					out mediump vec4 vs_TEXCOORD1;
					out mediump vec4 vs_TEXCOORD2;
					out mediump vec4 vs_TEXCOORD3;
					out mediump vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4glstate_matrix_mvp[1];
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_TEXCOORD1 = _Offsets.xyxy * vec4(1.0, 1.0, -1.0, -1.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = _Offsets.xyxy * vec4(2.0, 2.0, -2.0, -2.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = _Offsets.xyxy * vec4(3.0, 3.0, -3.0, -3.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD4 = _Offsets.xyxy * vec4(5.0, 5.0, -5.0, -5.0) + in_TEXCOORD0.xyxy;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform lowp sampler2D _MainTex;
					in mediump vec2 vs_TEXCOORD0;
					in mediump vec4 vs_TEXCOORD1;
					in mediump vec4 vs_TEXCOORD2;
					in mediump vec4 vs_TEXCOORD3;
					in mediump vec4 vs_TEXCOORD4;
					layout(location = 0) out mediump vec4 SV_Target0;
					mediump vec4 u_xlat16_0;
					lowp vec4 u_xlat10_0;
					lowp vec4 u_xlat10_1;
					void main()
					{
					    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat16_0 = u_xlat10_0 * vec4(0.150000006, 0.150000006, 0.150000006, 0.150000006);
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.224999994, 0.224999994, 0.224999994, 0.224999994) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD1.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.150000006, 0.150000006, 0.150000006, 0.150000006) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.109999999, 0.109999999, 0.109999999, 0.109999999) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD2.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.109999999, 0.109999999, 0.109999999, 0.109999999) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.075000003, 0.075000003, 0.075000003, 0.075000003) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD3.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.075000003, 0.075000003, 0.075000003, 0.075000003) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.xy);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.0524999984, 0.0524999984, 0.0524999984, 0.0524999984) + u_xlat16_0;
					    u_xlat10_1 = texture(_MainTex, vs_TEXCOORD4.zw);
					    u_xlat16_0 = u_xlat10_1 * vec4(0.0524999984, 0.0524999984, 0.0524999984, 0.0524999984) + u_xlat16_0;
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