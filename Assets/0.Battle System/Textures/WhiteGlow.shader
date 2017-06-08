Shader "Custom/WhiteGlow" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Override ("Texture Override", Range (0.00, 1)) = 0
		_Slice ("Slice", Range (0, 2.5)) = 2.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
//		#pragma surface surf BlinnPhong
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Override;
//		half _Slice;

		void surf (Input IN, inout SurfaceOutput o) {
//			half dist = (_Slice - IN.worldPos.y)/IN.worldPos.y;
//          	clip(dist);
			fixed4 _Color = fixed4(1,1,1,1);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*(1 - _Override)*_Color + _Color*_Override;
			o.Albedo = c.rgb;
			o.Emission = c.rgb*_Override;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
