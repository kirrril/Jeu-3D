Shader "Custom/ThirstyDeathShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.2, 0.2, 0.2, 1) // Gris
        _Color2 ("Color 2", Color) = (1, 1, 1, 1) // Blanc
        _Color3 ("Color 3", Color) = (0, 0, 0, 0) // Transparent
        _Offset ("Offset", Range(-1, 1)) = 1 // Pour animer le dégradé
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
            };

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = (1.0 - i.uv.y) + _Offset; // Inverse i.uv.y
                t = saturate(t);

                // Dégradé : gris -> blanc -> transparent (car t va de 1 en bas à 0 en haut)
                if (t < 0.5)
                    return lerp(_Color1, _Color2, t / 0.5); // De gris à blanc
                else
                    return lerp(_Color2, _Color3, (t - 0.5) / 0.5); // De blanc à transparent
            }
            ENDCG
        }
    }
}