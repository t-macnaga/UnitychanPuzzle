Shader "Custom/Dissolve" 
{
    Properties 
    {
        _MainTex ("Main Texture", 2D)            = "grey" {}
        _DistortionTex ("Distortion Texture(RG)", 2D)            = "grey" {}
        _DistortionPower ("Distortion Power", Range(0, 1))     = 0
        _Width("Width",Range(0,1))=0.001
        _CutOff("CutOff Range",Range(0,1))=0
    }
    
    SubShader 
    {
        Tags {
            "Queue"="Transparent"
			"LightMode"="UniversalForward"
            "RenderType"="Transparent"
            "RenderPipeline" = "LightweightPipeline"
        }
        
        Cull Back 
        // ZWrite On
        // ZTest LEqual
        // ColorMask RGB

        // BlendOp Add
        Blend SrcAlpha OneMinusSrcAlpha
        // GrabPass { "_GrabPassTexture" }

        Pass {

            CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
            
           #include "UnityCG.cginc"

            struct appdata {
                half4 vertex                : POSITION;
                half4 texcoord              : TEXCOORD0;
            };
                
            struct v2f {
                half4 vertex                : SV_POSITION;
                half2 uv                    : TEXCOORD0;
                half4 grabPos               : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            half4 _MainTex_ST;
            sampler2D _DistortionTex;
            half4 _DistortionTex_ST;
            // sampler2D _GrabPassTexture;
            half _DistortionPower;
            fixed _CutOff;
            fixed _Width;

            v2f vert (appdata v)
            {
                v2f o                   = (v2f)0;
                
                o.vertex                = UnityObjectToClipPos(v.vertex);
                o.uv                    = TRANSFORM_TEX(v.texcoord, _DistortionTex);
                o.grabPos               = ComputeGrabScreenPos(o.vertex);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // half2 uv            = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);
                fixed a = tex2D(_DistortionTex, i.uv).r;

                // fixed b = smoothstep(_CutOff,_CutOff + _Width,a);

                fixed4 col = tex2D(_MainTex, i.uv);
                // fixed4 col = tex2D(_GrabPassTexture, uv);
                // fixed4 col = tex2D(_GrabPassTexture, i.uv);//grabPos);
                fixed b2 = step(a,_CutOff);// + _Width * 2.0);
                col.a = b2;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Hidden/InternalErrorShader"
}