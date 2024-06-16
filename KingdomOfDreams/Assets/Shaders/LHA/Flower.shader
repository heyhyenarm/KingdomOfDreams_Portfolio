Shader "Custom/Flower" {
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _BloomTex("Bloom Texture", 2D) = "white" {}
        //_BlinkInterval("Blink Interval", Range(0, 1)) = 0.5
        _BloomIntensity("Bloom Intensity", Range(0, 1)) = 0.5
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                sampler2D _BloomTex;
                float _BlinkInterval;
                float _BloomIntensity;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // 불규칙한 깜빡임 효과를 적용할 시간 계산
                    //float blinkTime = _Time.y % (_BlinkInterval * tex2D(_MainTex, i.uv).r);
                    //float blink = step(_BlinkInterval * 0.5, blinkTime);

                    // 원본 텍스쳐에 블룸 효과를 교차적으로 적용
                    float4 mainTex = tex2D(_MainTex, i.uv);
                    float4 bloomTex = tex2D(_BloomTex, i.uv);
                    float4 finalColor = mainTex + (bloomTex * _BloomIntensity);

                    return finalColor;
                }
                ENDCG
            }
        }
}