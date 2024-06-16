Shader "Custom/Water"
{
    Properties
    {
        _WaterColor("Water Color", Color) = (1, 1, 1, 1)
        _Cube("Cube", Cube) = "" {}
        _RefractionStrength("Refraction Strength", Range(0, 10)) = 0.5
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            GrabPass {}

            CGPROGRAM
            #pragma surface surf Lambert vertex:vert
            #pragma target 3.0

            sampler2D _GrabTexture;
            samplerCUBE _Cube;
            fixed4 _WaterColor;
            float _RefractionStrength;

            struct Input
            {
                float2 uv_GrabTexture;
                float3 worldRefl;
                float3 viewDir;
                float4 screenPos;
                INTERNAL_DATA
            };

            void vert(inout appdata_full v)
            {
                float movement;
                movement = sin(abs((v.texcoord.x * 2 - 1) * 12) + _Time.y) * 0.1;
                movement += sin(abs((v.texcoord.y * 2 - 1) * 12) + _Time.y) * 0.1;
                v.vertex.y += movement / 2;
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                o.Albedo = _WaterColor.rgb;
                o.Alpha = _WaterColor.a;

                float3 refcolor = texCUBE(_Cube, reflect(-IN.viewDir, o.Normal));

                float3 screenUV = IN.screenPos.rgb / IN.screenPos.a;
                float3 refraction = tex2D(_GrabTexture, screenUV + o.Normal.xy * 0.1);
                refraction *= _RefractionStrength;

                float rim = saturate(dot(o.Normal, IN.viewDir));
                rim = pow(1 - rim, 1.5);

                o.Emission = (refcolor * rim + refraction) * 0.5;

                // 물체 반사
                o.Normal = reflect(IN.viewDir, o.Normal);
            }

            ENDCG
        }

            FallBack "Legacy Shaders/Transparent/VertexLit"
}