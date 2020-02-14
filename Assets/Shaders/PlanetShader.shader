Shader "Custom/PlanetShader"
{
    Properties
    {
        _MainTex("_MainTex", 2D) = "white" {}
        _SpecularPower("Specular power", float) = 1
        _Gloss("Gloss", float) = 0
        _FresnelPower("Fresnel power", float) = 0
        _FresnelColor("Fresnel color", Color) = (1,1,1,1)
        _RandomSeed ("Random seed", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull off

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        
        sampler2D _MainTex;
        float _SpecularPower;
        float _RandomSeed;
        float _FresnelPower;
        float _Gloss;
        fixed4 _FresnelColor;

        struct Input
        {
            float2 uv_MainTex;
            float4 vertColor : COLOR;
            float3 viewDir;
            float3 worldNormal;
        };

        
        float noise11(float x)
        {
            return frac(sin(x) * 43758.5453123);
        }

        
        float3 noise13(in float x)
        {
            float3 res;
            res.x = noise11(x);
            res.y = noise11(res.x);
            res.z = noise11(res.y);
            
            return res;
        }
        
        float3 getColor(float x)
        {
            float3 dcOffset = (noise13(_RandomSeed) * 2 - 1) * 0.25;
            float3 amplitude = noise13(_RandomSeed + 13.1241) * 2 - 1;            
            float3 frequency = noise13(_RandomSeed + 235.123) * 10;
            float3 phase = noise13(_RandomSeed - 731.434) * 100;
            
            float3 res = (cos(x * frequency + phase) * amplitude) * 0.5 + 0.5 + dcOffset;
            
            //res = res * 0.6 + 0.4;
            
            return res;            
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = getColor(IN.uv_MainTex.x);
            //o.Metallic = (1-smoothstep(0,0.1,IN.uv_MainTex.x)) * _Gloss;
            
            float fresnel = pow(1 - saturate(dot(IN.viewDir, IN.worldNormal)*0.5+0.5), _FresnelPower);
            o.Emission = fresnel * _FresnelColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
