Shader "Custom/LandscapeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        H ("Hue", Range(0.0, 6.28)) = 0
        S ("Saturation", Range(0.0, 1.0)) = 1.0
        V ("Value", Range(0.0, 1.0)) = 1.0
    }

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 BaseColor;
 
            float H;
            float S;
            float V;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            // Ones for every Pixel
            fixed4 frag (v2f i) : SV_Target {

                //cos and sin was on (H*PI/180)
                float VSU = float(V*S*cos(H));
                float VSW = float(V*S*sin(H));
 
                float4 BaseColor = tex2D(_MainTex, i.uv) *_Color;
               
                float retr = float((0.299*V+0.701*VSU+0.168*VSW)*BaseColor.r + (0.587*V-0.587*VSU+0.330*VSW)*BaseColor.g + (0.114*V-0.114*VSU-0.497*VSW)*BaseColor.b);
                float retg = float((0.299*V-0.299*VSU-0.328*VSW)*BaseColor.r + (0.587*V+0.413*VSU+0.035*VSW)*BaseColor.g + (0.114*V-0.114*VSU+0.292*VSW)*BaseColor.b);
                float retb = float((0.299*V-0.3*VSU+1.25*VSW)*BaseColor.r + (0.587*V-.588*VSU-1.05*VSW)*BaseColor.g + (0.114*V+.886*VSU-0.203*VSW)*BaseColor.b);
 
                fixed4 col = fixed4 (retr, retg, retb, 1);
 
                UNITY_APPLY_FOG(i.fogCoord, col);

                // just invert the colors
                // fixed4 col = tex2D(_MainTex, i.uv);
                // col.r = 1 - col.r;
                // col.g = 1 - col.g;
                // col.b = 1 - col.b;

                //col.rgb = 1 - col.rgb;
                return col;
            }

            ENDCG
        }
    }
}
