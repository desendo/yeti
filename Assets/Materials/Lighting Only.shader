Shader "Custom/Lighting Only"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Blend SrcAlpha One
		ZWrite Off
		Tags
	{
		Queue = Transparent
	}
		ColorMask RGB

		CGPROGRAM

#pragma surface surf MyLight alpha finalcolor:mycolor

		sampler2D _MainTex;
	fixed4 _Color;

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = tex.rgb;
		o.Alpha = tex.a;
	}

	float lightPower;
	fixed4 LightingMyLight(SurfaceOutput s, half3 lightDir, fixed atten)
	{
		lightPower = 1 - saturate(dot(normalize(s.Normal), lightDir));
		return fixed4((s.Albedo * _LightColor0.rgb) * (lightPower * atten * 2), 1);
	}

	void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
	{
		color.a = o.Alpha * lightPower;
	}

	ENDCG
	}
		Fallback "VertexLit", 2
}
