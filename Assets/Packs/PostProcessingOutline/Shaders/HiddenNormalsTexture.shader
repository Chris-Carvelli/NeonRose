Shader "Hidden/Roystan/Normals Texture"
{
    Properties
    {
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque" 
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex     : SV_POSITION;
				float3 viewNormal : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _MaskTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
				//o.viewNormal = mul((float3x3)UNITY_MATRIX_M, v.normal);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                fixed mainAlpha = tex2D(_MainTex, i.uv).a;
                fixed maskAlpha = tex2D(_MaskTex, i.uv).a;
                // return float4(mainAlpha, 0, 0, 1);
                return float4(normalize(i.viewNormal) * 0.5 + 0.5, max(mainAlpha, maskAlpha));
            }
            ENDCG
        }
    }
}
