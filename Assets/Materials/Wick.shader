Shader "Custom/Wick"
{
    Properties
    {
        _IntactA ("Intact Color 1", Color) = (1, 1, 0, 1) // Yellow
        _IntactB ("Intact Color 2", Color) = (1, 0, 1, 1) // Pink
        _BurntA ("Burnt Color 1", Color) = (0.3, 0.3, 0, 1)
        _BurntB ("Burnt Color 2", Color) = (0, 0.3, 0.3, 1)
        _TurnsRemaining ("Turns Remaining", Int) = 4
        _RoundDuration ("Round Duration", Int) = 7
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _IntactA;
            fixed4 _IntactB;
            fixed4 _BurntA;
            fixed4 _BurntB;
            int _TurnsRemaining;
            int _RoundDuration;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float Noise(float2 uv)
            {
                float _NoiseScale = 1.0;
                // A simple pseudo-random noise function
                return frac(sin(dot(uv * _NoiseScale, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Get the UV Y coordinate
                float uvY = i.uv.y;
                
                fixed4 even = _IntactA;
                fixed4 odd = _IntactB;

                float border = float(_TurnsRemaining) / _RoundDuration;
                float delta = uvY - border;
                float width = 0.016;
                if (delta > (sin(_Time.y) - 2) * width / 2 && delta < (sin(_Time.y * 1.345123 + 7) + 2) * width / 2) {
                    float _FlickerSpeed = 10;
                    float _FlickerIntensity = 0.6;
                    float flicker1 = (sin(_Time.y * _FlickerSpeed) + 1.0) * 0.5; // Base flicker
                    float flicker2 = (sin(_Time.y * _FlickerSpeed * 1.5 + i.uv.y * 10.0) + 1.0) * 0.5; // Secondary flicker
                    // float noise = Noise(i.uv + _Time.y * 0.5) * _FlickerIntensity; // Add noise
                    float flicker = flicker1 + flicker2;
                    // flicker = saturate(flicker); // Clamp to [0, 1]
                    float4 flameA = float4(1.0, 0.647, 0.0, 1.0); // Bright Orange
                    float4 flameB = float4(1.0, 1.0, 0.4, 1.0); // Soft Yellow
                    return lerp(flameA, flameB, flicker);
                }
                
                int turnId = floor(uvY * _RoundDuration);

                if (turnId >= _TurnsRemaining) {
                    even = _BurntA;
                    odd = _BurntB;
                }

                return turnId % 2 == 0 ? even : odd;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
