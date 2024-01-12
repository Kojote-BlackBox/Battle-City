Shader "Custom/LandscapeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        H ("Hue", Range(0.0, 6.28)) = 0
        S ("Saturation", Range(0.0, 1.0)) = 1.0
        V ("Value", Range(0.0, 1.0)) = 1.0

        // Values for some darker Theme
        // H = 0.35
        // S = 0.5
        // V = 0.75
    }

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        // Blend-Modi Settings für Transparenz support

        // Normal Transparent
        //Blend SrcAlpha OneMinusSrcAlpha
       
        // Cutout-Transparenz
        Blend SrcAlpha OneMinusSrcAlpha ZWrite Off

        //AlphaTest Greater 0.5
        
        // Opaque / Transparent / TransparentCutout
        Tags { "RenderType"="TransparentCutout" }
        
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                // UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                
                // Hinzugefügt für Optimierung (Berechnung in Vertex start fragment shader)
                float VSU : TEXCOORD1;  
                float VSW : TEXCOORD2;  
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 BaseColor;
 
            float H, S, V;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // UNITY_TRANSFER_FOG(o,o.vertex);

                // Berechnung hier im Vertex Shader
                o.VSU = V * S * cos(H);
                o.VSW = V * S * sin(H);

                return o;
            }
            
            // Ones for every Pixel
            fixed4 frag (v2f i) : SV_Target {
                float4 BaseColor = tex2D(_MainTex, i.uv) *_Color;
                
                // Verwenden Sie i.VSU und i.VSW, die aus dem Vertex Shader übergeben wurden
                float retr = (0.299 * V + 0.701 * i.VSU + 0.168 * i.VSW) * BaseColor.r 
                           + (0.587 * V - 0.587 * i.VSU + 0.330 * i.VSW) * BaseColor.g 
                           + (0.114 * V - 0.114 * i.VSU - 0.497 * i.VSW) * BaseColor.b;
    
                float retg = (0.299 * V - 0.299 * i.VSU - 0.328 * i.VSW) * BaseColor.r 
                           + (0.587 * V + 0.413 * i.VSU + 0.035 * i.VSW) * BaseColor.g 
                           + (0.114 * V - 0.114 * i.VSU + 0.292 * i.VSW) * BaseColor.b;

                float retb = (0.299 * V - 0.3 * i.VSU + 1.25 * i.VSW) * BaseColor.r 
                           + (0.587 * V - 0.588 * i.VSU - 1.05 * i.VSW) * BaseColor.g 
                           + (0.114 * V + 0.886 * i.VSU - 0.203 * i.VSW) * BaseColor.b;

                float alpha = BaseColor.a;
            
                return fixed4 (retr, retg, retb, alpha);
            }

            ENDCG
        }
    }
}
