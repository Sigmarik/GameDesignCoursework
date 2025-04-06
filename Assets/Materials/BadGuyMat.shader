Shader "Custom/BadGuyMat"
{
    Properties
    {
        _Color("Color", Color) = (0, 0, 0, 1)
        _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower("Rim Power", Float) = 0.3
        _RimBalance("Balance", Float) = 1.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RimStrength ("Rim Strength", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldNormal;
        };

        fixed4 _Color;
        fixed4 _RimColor;
        float _RimPower;

        half _Glossiness;
        half _Metallic;
        float _RimBalance;

        half _RimStrength;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float borderHighlight(float3 view, float3 normal) {
            float projection = dot(view, normal) / length(view) / length(view) / length(normal) / length(normal);

            float pow4 = pow(abs(projection), _RimPower);

            return 1.0 - pow4;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;

            float rim_light = (borderHighlight(IN.worldNormal, IN.viewDir)
                * _RimStrength - _RimBalance) * _RimColor;

            o.Emission = rim_light;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
