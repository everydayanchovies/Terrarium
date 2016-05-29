// Compiled shader for Android, uncompressed size: 5.6KB

// Skipping shader variants that would not be included into build of current scene.

Shader "TreeShader" {
Properties {
 _MainTex ("Texture", 2D) = "white" { }
 _TransitionTex ("Transition Texture", 2D) = "white" { }
 _Color ("Screen Color", Color) = (1,1,1,1)
 _Cutoff ("Cutoff", Range(0,1)) = 0
[MaterialToggle]  _Distort ("Distort", Float) = 0
 _Fade ("Fade", Range(0,1)) = 0
}
SubShader { 


 // Stats for Vertex shader:
 //        gles : 10 math, 2 texture, 2 branch
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" }
  GpuProgramID 61794
Program "vp" {
SubProgram "gles " {
// Stats: 10 math, 2 textures, 2 branches
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform lowp vec4 glstate_lightmodel_ambient;
uniform lowp vec4 _LightColor0;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_COLOR0;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  highp vec4 tmpvar_2;
  tmpvar_2.w = 0.0;
  tmpvar_2.xyz = _glesNormal;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = (_LightColor0.xyz * max (0.0, dot (
    normalize((tmpvar_2 * _World2Object).xyz)
  , 
    normalize(_WorldSpaceLightPos0.xyz)
  )));
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_1;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR0 = (tmpvar_3 + (glstate_lightmodel_ambient * 2.0));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _TransitionTex;
uniform highp int _Distort;
uniform highp float _Fade;
uniform sampler2D _MainTex;
uniform highp float _Cutoff;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp vec4 col_2;
  lowp vec2 direction_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_TransitionTex, xlv_TEXCOORD1);
  direction_3 = vec2(0.0, 0.0);
  if (bool(_Distort)) {
    direction_3 = normalize(((tmpvar_4.xy - vec2(0.5, 0.5)) * vec2(2.0, 2.0)));
  };
  lowp vec4 tmpvar_5;
  highp vec2 P_6;
  P_6 = (xlv_TEXCOORD0 + (_Cutoff * direction_3));
  tmpvar_5 = texture2D (_MainTex, P_6);
  highp vec4 tmpvar_7;
  tmpvar_7 = (tmpvar_5 * xlv_COLOR0);
  col_2 = tmpvar_7;
  if ((tmpvar_4.z < _Cutoff)) {
    highp vec4 tmpvar_8;
    tmpvar_8 = mix (col_2, _Color, vec4(_Fade));
    col_2 = tmpvar_8;
    tmpvar_1 = tmpvar_8;
  } else {
    tmpvar_1 = col_2;
  };
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 _World2Object;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	lowp vec4 _LightColor0;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
highp  vec4 phase0_Output0_0;
out highp vec2 vs_TEXCOORD1;
out highp vec4 vs_COLOR0;
vec4 u_xlat0;
vec3 u_xlat1;
float u_xlat6;
void main()
{
    phase0_Output0_0 = in_TEXCOORD0.xyxy;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    u_xlat0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    u_xlat0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat6 = inversesqrt(u_xlat6);
    u_xlat0.xyz = vec3(u_xlat6) * u_xlat0.xyz;
    u_xlat6 = dot(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz);
    u_xlat6 = inversesqrt(u_xlat6);
    u_xlat1.xyz = vec3(u_xlat6) * _WorldSpaceLightPos0.xyz;
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat1.xyz);
    u_xlat0.x = max(u_xlat0.x, 0.0);
    u_xlat0.xyz = u_xlat0.xxx * _LightColor0.xyz;
    u_xlat0.w = 1.0;
    vs_COLOR0 = glstate_lightmodel_ambient * vec4(2.0, 2.0, 2.0, 2.0) + u_xlat0;
vs_TEXCOORD0 = phase0_Output0_0.xy;
vs_TEXCOORD1 = phase0_Output0_0.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	int _Distort;
uniform 	float _Fade;
uniform 	float _Cutoff;
uniform 	lowp vec4 _Color;
uniform lowp sampler2D _TransitionTex;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
in highp vec4 vs_COLOR0;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
mediump vec2 u_xlat16_0;
lowp vec3 u_xlat10_0;
bool u_xlatb0;
lowp vec4 u_xlat10_1;
vec4 u_xlat2;
void main()
{
    u_xlat10_0.xyz = texture(_TransitionTex, vs_TEXCOORD1.xy).xyz;
    u_xlat10_1.xy = u_xlat10_0.xy + vec2(-0.5, -0.5);
    u_xlat10_1.xy = u_xlat10_1.xy * vec2(2.0, 2.0);
    u_xlat16_0.x = dot(u_xlat10_1.xy, u_xlat10_1.xy);
    u_xlat16_0.x = inversesqrt(u_xlat16_0.x);
    u_xlat16_0.xy = u_xlat16_0.xx * u_xlat10_1.xy;
    u_xlat0.xy = u_xlat16_0.xy * vec2(vec2(_Cutoff, _Cutoff));
    u_xlat0.xy = (_Distort != 0) ? u_xlat0.xy : vec2(0.0, 0.0);
    u_xlat0.xy = u_xlat0.xy + vs_TEXCOORD0.xy;
    u_xlat10_1 = texture(_MainTex, u_xlat0.xy);
    u_xlat2 = u_xlat10_1 * vs_COLOR0;
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat10_0.z<_Cutoff);
#else
    u_xlatb0 = u_xlat10_0.z<_Cutoff;
#endif
    if(u_xlatb0){
        u_xlat0 = (-u_xlat10_1) * vs_COLOR0 + _Color;
        u_xlat0 = vec4(vec4(_Fade, _Fade, _Fade, _Fade)) * u_xlat0 + u_xlat2;
        SV_Target0 = u_xlat0;
        return;
    //ENDIF
    }
    SV_Target0 = u_xlat2;
    return;
}
#endif
"
}
}
Program "fp" {
SubProgram "gles " {
"// shader disassembly not supported on gles"
}
SubProgram "gles3 " {
"// shader disassembly not supported on gles3"
}
}
 }
}
}