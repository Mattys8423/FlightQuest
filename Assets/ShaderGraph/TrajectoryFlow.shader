Shader "FlightQuest/Trajectory Flow"
{
    Properties
    {
        _Texture2D ("Dot texture", 2D) = "white" {}
        _TilingAmount ("Dot count", Float) = 15
        _AnimationSpeed ("Dots per second", Float) = 2
        _IsAnimate ("Animate", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Texture2D;
            float _TilingAmount;
            float _AnimationSpeed;
            float _IsAnimate;

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            fixed4 frag(Varyings input) : SV_Target
            {
                float count = max(1.0, floor(_TilingAmount + 0.5));
                float along = saturate(input.uv.x) * count;
                if (_IsAnimate < 0.5 || count < 3.0)
                    return tex2D(_Texture2D, float2(frac(along), input.uv.y)) * input.color;

                float phase = frac(_Time.y * _AnimationSpeed);
                float movingUV = along - phase;
                float center = floor(movingUV) + 0.5 + phase;

                // Fade each WHOLE dot, using its center, before it reaches an edge.
                // This preserves the circle instead of clipping it with a spatial mask.
                float visibility = smoothstep(0.5, 1.0, center);
                visibility *= 1.0 - smoothstep(count - 1.8, count - 1.2, center);
                fixed4 moving = tex2D(_Texture2D, float2(frac(movingUV), input.uv.y));
                moving.a *= visibility;

                // The last tile stays fixed: a complete dot always marks the end.
                fixed4 tip = tex2D(_Texture2D, float2(saturate(along - count + 1.0), input.uv.y));
                tip.a *= step(count - 1.0, along);
                fixed4 color = moving.a > tip.a ? moving : tip;
                return color * input.color;
            }
            ENDCG
        }
    }
}
