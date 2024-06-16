Shader "Custom/Water" {
    Properties{
        _ReflectionIntensity("Reflection Intensity", Float) = 0.5
        _Skybox("Skybox", Cube) = "_Skybox"
        _WavesAmplitude("Waves Amplitude", Float) = 0.01
        _WavesAmount("Waves Amount", Float) = 8.87
    }

        SubShader{
            Tags { "RenderType" = "Opaque" "Queue" = "Transparent+0" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert vertex:vert

            float _ReflectionIntensity;
            samplerCUBE _Skybox;
            float _WavesAmplitude;
            float _WavesAmount;

            struct Input {
                float2 uv_MainTex;
                float3 worldRefl;
            };

            void vert(inout appdata_full v) {
                float3 ase_vertex3Pos = v.vertex.xyz;
                float3 ase_vertexNormal = v.normal.xyz;
                v.vertex.xyz += ((sin(((_WavesAmount * ase_vertex3Pos.z) + _Time.y)) * ase_vertexNormal) * _WavesAmplitude);
            }

            void surf(Input IN, inout SurfaceOutput o) {
                // 기존 물 색상 계산
                // ...

                // 스카이박스 반사 효과 계산
                fixed3 reflectedColor = texCUBE(_Skybox, IN.worldRefl).rgb;
                o.Albedo += reflectedColor * _ReflectionIntensity;
            }
            ENDCG
    }

        FallBack "Diffuse"
}