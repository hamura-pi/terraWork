Shader "Custom/Burn"
{
        Properties
        {
                _MainTex ("Base (RGB)", 2D) = "white" {}
                _Cutoff("Cutoff", Range(0, 1)) = 0
                _BurnShift("BurnShift", Range(0, 0.1)) = 0.01
                _BurnColor("Cutoff", Color) = (1, 1, 0, 1)
        }
        SubShader
        {
                Tags { "RenderType"="Opaque" }
                LOD 200
               
                CGPROGRAM
                #pragma surface surf Lambert
                //#pragma only_renderers d3d9

                sampler2D _MainTex;
                fixed _Cutoff;
                fixed _BurnShift;
                fixed4 _BurnColor;

                struct Input
                {
                        float2 uv_MainTex;
                };

                void surf (Input IN, inout SurfaceOutput o)
                {
                        half4 c = tex2D (_MainTex, IN.uv_MainTex);

                        fixed pos = c.a - _Cutoff;
                                                fixed bPos = pos - _BurnShift;
                                                fixed isBurn = 1-step(-bPos, 0);

                        o.Albedo = lerp(c.rgb, _BurnColor.rgb, isBurn);
                        o.Emission = lerp(fixed3(0,0,0), _BurnColor.rgb, isBurn);

                                                fixed a = c.a-_Cutoff;
                        o.Alpha = a;
                        clip(a);
                }
                ENDCG
        }
        FallBack "Diffuse"
}