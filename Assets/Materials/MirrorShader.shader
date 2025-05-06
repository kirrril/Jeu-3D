Shader "Custom/MirrorPerspectiveCorrected"
{
    Properties
    {
        _MainTex ("Render Texture", 2D) = "white" {}
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)
        _PlayerForward ("Player Forward", Vector) = (0, 0, 1, 0)
        _MirrorPos ("Mirror Position", Vector) = (0, 0, 0, 0)
        _MirrorNormal ("Mirror Normal", Vector) = (0, 0, 1, 0)
        _QuadWidth ("Quad Width", Float) = 10
        _QuadHeight ("Quad Height", Float) = 3
        _Compression ("Compression Factor", Float) = 0
        _CompressionSide ("Compression Side", Float) = 0
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
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _PlayerPos;
            float4 _PlayerForward;
            float4 _MirrorPos;
            float4 _MirrorNormal;
            float _QuadWidth;
            float _QuadHeight;
            float _Compression;
            float _CompressionSide;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculer la position du fragment dans l'espace monde
                float3 worldPos = i.worldPos.xyz;

                // Calculer la direction du joueur vers le fragment
                float3 playerToFragment = normalize(worldPos - _PlayerPos.xyz);

                // Calculer la réflexion du rayon du joueur par rapport au plan du miroir
                float3 mirrorNormal = normalize(_MirrorNormal.xyz);
                float3 mirrorPos = _MirrorPos.xyz;
                float3 reflectedDir = reflect(playerToFragment, mirrorNormal);

                // Calculer le point d'intersection avec le plan du miroir
                float3 rayOrigin = _PlayerPos.xyz;
                float3 rayDir = reflectedDir;
                float t = dot(mirrorNormal, mirrorPos - rayOrigin) / dot(mirrorNormal, rayDir);
                float3 reflectedPoint = rayOrigin + t * rayDir;

                // Projeter le point réfléchi sur le plan du quad
                float3 quadRight = normalize(cross(mirrorNormal, float3(0, 1, 0)));
                float3 quadUp = normalize(cross(quadRight, mirrorNormal));
                float3 toReflected = reflectedPoint - mirrorPos;
                float u = dot(toReflected, quadRight) / _QuadWidth + 0.5;
                float v = dot(toReflected, quadUp) / _QuadHeight + 0.5;

                // Ajuster les UVs pour la réflexion
                float2 correctedUV = float2(1.0 - u, v); // Inversion horizontale seulement

                // Appliquer la compression
                float compression = _Compression * (1.0 - abs(correctedUV.x - 0.5) * 2.0 * _CompressionSide);
                correctedUV.y = 0.5 + (correctedUV.y - 0.5) * (1.0 - compression);

                // Échantillonner la Render Texture
                fixed4 col = tex2D(_MainTex, correctedUV);

                // Gestion des bords (noir si hors du quad)
                if (u < 0 || u > 1 || v < 0 || v > 1)
                {
                    col = fixed4(0, 0, 0, 1);
                }

                return col;
            }
            ENDCG
        }
    }
}