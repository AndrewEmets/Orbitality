Shader "Unlit/SunShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Frequency ("_Frequency", float) = 1
        _Speed("Speed", float) = 1
        _Color("Color", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 vertPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;
            float _Frequency;
            fixed3 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertPos = v.vertex;
                return o;
            }
            
            float rand11(float x)
            {
                return frac(sin(x * 67.9384) * 827.284);
            }
            
            float rand31(float3 co)
            {
                return frac(sin(dot(co,float3(65.9898,78.233, 29.3471))) * 1537.5497);
            }
            
            float3 rand33(float3 v)
            {
                float3 r;
            
                r.x = rand31(v);
                r.y = rand11(r.x);
                r.z = rand11(r.y);
            
                return r;
            }
            
            float n(float3 uv)
            {
                float3 cell = floor(uv);
                float3 luv = frac(uv);
            
                float d[8];
            
                for(int x = 0; x <= 1; x++)
                for(int y = 0; y <= 1; y++)
                for(int z = 0; z <= 1; z++)
                {
                    int ind = 1*z+2*y+4*x;
                    float3 p = cell + float3(x,y,z);
            
                    float3 dir = rand33(p)*2.-1.;
                    float3 delta = luv - float3(x,y,z);
            
                    d[ind] = dot(delta, dir);
                }
            
                for(int k = 0; k < 3; k++)
                    luv[k] = smoothstep(0,1,luv[k]);
            
                float4 rx;
            
                for(int k = 0; k < 4; k++)
                    rx[k] = lerp(d[k], d[k+4], luv.x);
            
                float2 rxy;
            
                for(int k = 0; k < 2; k++)
                    rxy[k] = lerp(rx[k], rx[k+2], luv.y);
            
                float rxyz = lerp(rxy[0], rxy[1], luv.z);
            
                return rxyz;
            }
            
            float noise(float3 uv, int steps)
            {
                float res = 0.;
                float p = 1.;
                float d = 0.;
                float amp = 0;
                
                for(int i = 0; i < steps; i++)
                {
                    res += n(uv * p) / p;
                    
                    amp += 1 / p;
                    p *= 2.;
                }
            
                return frac(res * 0.5 + 0.5);
            }

            fixed4 frag (v2f i) : SV_Target
            { 
                fixed3 col = noise(i.vertPos * _Frequency - float3(0,0,_Time.z * _Speed), 1) * _Color;
                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
