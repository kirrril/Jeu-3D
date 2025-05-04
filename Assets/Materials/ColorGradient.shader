Shader "Custom/ColorGradient"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1) // Rouge
        _Color2 ("Color 2", Color) = (1, 1, 0, 1) // Jaune
        _Color3 ("Color 3", Color) = (0, 1, 0, 1) // Vert
        _Offset ("Offset", Range(-1, 1)) = 0 // Pour animer le dégradé
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
                float t = (1.0 - i.uv.y) + _Offset; // Utilise la coordonnée Y pour le dégradé, décalée par _Offset
                t = saturate(t); // Limite t entre 0 et 1

                // Dégradé en 4 étapes
                if (t < 0.33)
                    return lerp(_Color1, _Color2, t / 0.33);
                else if (t < 0.66)
                    return lerp(_Color2, _Color3, (t - 0.33) / 0.33);
                else
                    return lerp(_Color3, _Color1, (t - 0.66) / 0.33);
            }
            ENDCG
        }
    }
}