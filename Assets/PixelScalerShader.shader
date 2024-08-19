Shader "Custom/PixelArtShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Obtener las coordenadas UV originales
                float2 uv = i.uv;

                // Suavizar las coordenadas UV al centro del píxel más cercano en la textura
                uv = floor(uv * 100.0) / 100.0;  // Ajusta 100.0 dependiendo del tamaño del sprite

                // Muestrear la textura
                fixed4 col = tex2D(_MainTex, uv);

                // Devuelve el color de la textura
                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
