Shader "Custom/NPC"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainTex2 ("Albedo (RGB)", 2D) = "white" {}
        _Gradation ("gradation", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;
        float _Gradation;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D (_MainTex, IN.uv_MainTex);
            float4 d = tex2D (_MainTex2, IN.uv_MainTex2);
            o.Emission = lerp(c, (c.r + c.g + c.b)/3, 1-_Gradation);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
