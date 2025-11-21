Shader "UI/Glow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowStrength ("Base Glow Strength", Range(0,5)) = 1

        _PulseSpeed ("Pulse Speed", Range(0,10)) = 2
        _PulseAmount ("Pulse Amount", Range(0,5)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;

            float4 _GlowColor;
            float _GlowStrength;

            float _PulseSpeed;
            float _PulseAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Texture + tint
                fixed4 col = tex2D(_MainTex, i.uv);

                // IMPORTANTE: respetar color real del sprite
                col *= _Color;

                // Si el pixel es totalmente transparente, no hacer glow
                if (col.a <= 0.001)
                    return col;

                // Animación del glow
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                float glow = _GlowStrength + pulse * _PulseAmount;
                 // Luminosidad del pixel (para evitar saturaciones)
                float brightness = dot(col.rgb, float3(0.299, 0.587, 0.114));

                // Factor anti-saturación (si el pixel ya es muy claro menos glow)
                float antiSaturate = saturate(1.0 - brightness);

                // Aplicar glow suavizado por alpha + anti-saturación
                col.rgb += _GlowColor.rgb * glow * col.a * antiSaturate;

                return col;
            }
            ENDCG
        }
    }
}
