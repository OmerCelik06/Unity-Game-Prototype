Shader"Custom/LaplacianEdge"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeColor ("Kenar Rengi", Color) = (1,1,1,1)
        _BgColor ("Arkaplan Rengi", Color) = (0,0,0,1)
        _Threshold ("Hassasiyet", Range(0.001, 1.0)) = 0.05
        _SampleDist ("Cizgi Kalinligi", Range(1.0, 15.0)) = 2.0 
        
        _Radius ("Fener Yaricapi", Range(0.0, 1.0)) = 0.4
        _Softness ("Fener Yumusakligi", Range(0.01, 1.0)) = 0.2
        
        _FadeStart ("Sis Baslangic (Metre)", Float) = 10.0
        _FadeEnd ("Sis Bitis (Metre)", Float) = 30.0
    }
    SubShader
    {
Cull Off

ZWrite Off

ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
float4 _MainTex_TexelSize;
sampler2D _CameraDepthTexture;

float4 _EdgeColor;
float4 _BgColor;
float _Threshold;
float _SampleDist;
            
float _Radius;
float _Softness;
float _FadeStart;
float _FadeEnd;

float ColorToGray(float4 color)
{
    return dot(color.rgb, float3(0.299, 0.587, 0.114));
}

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float2 uv = i.uv;

    float2 texel = _MainTex_TexelSize.xy * _SampleDist;
                
    float c = ColorToGray(tex2D(_MainTex, uv));
    float u = ColorToGray(tex2D(_MainTex, uv + float2(0, texel.y)));
    float d = ColorToGray(tex2D(_MainTex, uv + float2(0, -texel.y)));
    float l = ColorToGray(tex2D(_MainTex, uv + float2(-texel.x, 0)));
    float r = ColorToGray(tex2D(_MainTex, uv + float2(texel.x, 0)));

    float diff = 4.0 * c - u - d - l - r;
    float edge = abs(diff);
    edge = step(_Threshold, edge);

    float dist = distance(uv, float2(0.5, 0.5));
    float mask = 1.0 - smoothstep(_Radius - _Softness, _Radius, dist);
    edge *= mask;

    float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
    float linearDepth = LinearEyeDepth(rawDepth);
                
    float depthFade = 1.0 - smoothstep(_FadeStart, _FadeEnd, linearDepth);
 
    depthFade = pow(depthFade, 4.0);
                
                
    float hardCut = step(linearDepth, _FadeEnd - 0.1);
                
    edge *= depthFade * hardCut;

                
    float darknessFade = saturate(c * 5.0);
    edge *= darknessFade;

    return lerp(_BgColor, _EdgeColor, edge);
}
            ENDCG
        }
    }
}