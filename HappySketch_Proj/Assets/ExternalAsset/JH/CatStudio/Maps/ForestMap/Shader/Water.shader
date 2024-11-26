// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CatStudio/ForestMap/Water"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,0)
		[Normal]_NormalTexture1("NormalTexture1", 2D) = "bump" {}
		_Normal1Power("Normal1Power", Float) = 1
		_Normal1Noise("Normal1Noise", Range( 0 , 1)) = 0.01
		_U_Speed1("U_Speed1", Float) = -0.001
		_V_Speed1("V_Speed1", Float) = 0.003
		[Normal]_NormalTexture2("NormalTexture2", 2D) = "bump" {}
		_Normal2Power("Normal2Power", Float) = 1
		_Normal2Noize("Normal2Noize", Range( 0 , 1)) = 0.01
		_U_Speed2("U_Speed2", Float) = 0.003
		_V_Speed2("V_Speed2", Float) = 0.001
		_Metallic("Metallic", Float) = 0.9
		_Smoothness("Smoothness", Float) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Overlay+0" }
		Cull Back
		Blend One DstColor
		BlendOp Add
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalTexture1;
		uniform float _U_Speed1;
		uniform float _V_Speed1;
		uniform float _Normal1Noise;
		uniform float4 _NormalTexture1_ST;
		uniform float _Normal1Power;
		uniform sampler2D _NormalTexture2;
		uniform float _U_Speed2;
		uniform float _V_Speed2;
		uniform float _Normal2Noize;
		uniform float4 _NormalTexture2_ST;
		uniform float _Normal2Power;
		uniform float4 _Color;
		uniform float _Metallic;
		uniform float _Smoothness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 appendResult35 = (float4(_U_Speed1 , _V_Speed1 , 0.0 , 0.0));
			float2 uv_TexCoord52 = i.uv_texcoord * float2( 20,20 );
			float simplePerlin2D43 = snoise( uv_TexCoord52 );
			float2 uv_NormalTexture1 = i.uv_texcoord * _NormalTexture1_ST.xy + _NormalTexture1_ST.zw;
			float2 panner20 = ( 1.0 * _Time.y * appendResult35.xy + ( ( simplePerlin2D43 * _Normal1Noise ) + uv_NormalTexture1 ));
			float3 lerpResult60 = lerp( float3(0,0,1) , UnpackNormal( tex2D( _NormalTexture1, panner20 ) ) , _Normal1Power);
			float4 appendResult38 = (float4(_U_Speed2 , _V_Speed2 , 0.0 , 0.0));
			float2 uv_NormalTexture2 = i.uv_texcoord * _NormalTexture2_ST.xy + _NormalTexture2_ST.zw;
			float2 panner34 = ( 1.0 * _Time.y * appendResult38.xy + ( ( simplePerlin2D43 * _Normal2Noize ) + uv_NormalTexture2 ));
			float3 lerpResult62 = lerp( float3(0,0,1) , UnpackNormal( tex2D( _NormalTexture2, panner34 ) ) , _Normal2Power);
			o.Normal = BlendNormals( lerpResult60 , lerpResult62 );
			o.Albedo = _Color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = _Color.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
590;174;1593;1152;1059.705;401.9727;1.3;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-1698.219,-523.0034;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;20,20;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;-1423.361,-399.9732;Float;False;Property;_Normal1Noise;Normal1Noise;4;0;Create;True;0;0;0;False;0;False;0.01;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;43;-1385.6,-501.917;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1345.248,346.4499;Float;False;Property;_Normal2Noize;Normal2Noize;9;0;Create;True;0;0;0;False;0;False;0.01;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1144.963,-140.208;Float;False;Property;_U_Speed1;U_Speed1;5;0;Create;True;0;0;0;False;0;False;-0.001;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1151.219,-54.54056;Float;False;Property;_V_Speed1;V_Speed1;6;0;Create;True;0;0;0;False;0;False;0.003;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1019.785,262.2801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1051.071,564.884;Float;False;Property;_U_Speed2;U_Speed2;10;0;Create;True;0;0;0;False;0;False;0.003;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1048.303,681.7482;Float;False;Property;_V_Speed2;V_Speed2;11;0;Create;True;0;0;0;False;0;False;0.001;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1696.656,430.4361;Inherit;False;0;30;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-1697.934,-328.1323;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1097.898,-484.1434;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-952.1285,-127.0513;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-880.3038,572.9485;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;-901.5684,412.0349;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-979.6808,-334.3882;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-692.8584,-37.57306;Float;True;Property;_NormalTexture1;NormalTexture1;2;1;[Normal];Create;True;0;0;0;False;0;False;None;None;False;bump;LockedToTexture2D;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;34;-677.7723,426.1851;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;30;-702.3974,185.23;Float;True;Property;_NormalTexture2;NormalTexture2;7;1;[Normal];Create;True;0;0;0;False;0;False;None;None;False;bump;LockedToTexture2D;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;20;-804.9769,-221.6783;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;61;-234.0697,410.6458;Float;False;Constant;_Vector1;Vector 1;10;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;29;-337.5318,170.8902;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-230.5452,-146.2974;Float;False;Property;_Normal1Power;Normal1Power;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-363.5233,-44.39183;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;59;-235.8391,-310.9595;Float;False;Constant;_Vector0;Vector 0;10;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;63;-228.7758,575.3079;Float;False;Property;_Normal2Power;Normal2Power;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;62;15.17035,491.0461;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;60;13.40097,-230.5592;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;10;235.2978,619.5222;Float;False;Property;_Smoothness;Smoothness;13;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;208.002,-94.90602;Float;False;Property;_Color0;Color 0;14;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;39;181.7298,83.21294;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;9;235.1977,514.2219;Float;False;Property;_Metallic;Metallic;12;0;Create;True;0;0;0;False;0;False;0.9;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;221.8695,-405.0763;Float;False;Property;_Color;Color;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;468.3613,34.38048;Float;False;True;-1;0;ASEMaterialInspector;0;0;Standard;CatStudio/ForestMap/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Opaque;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;1;1;False;-1;2;False;-1;0;5;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;0;52;0
WireConnection;67;0;43;0
WireConnection;67;1;66;0
WireConnection;54;0;43;0
WireConnection;54;1;53;0
WireConnection;35;0;17;0
WireConnection;35;1;36;0
WireConnection;38;0;32;0
WireConnection;38;1;37;0
WireConnection;68;0;67;0
WireConnection;68;1;33;0
WireConnection;55;0;54;0
WireConnection;55;1;28;0
WireConnection;34;0;68;0
WireConnection;34;2;38;0
WireConnection;20;0;55;0
WireConnection;20;2;35;0
WireConnection;29;0;30;0
WireConnection;29;1;34;0
WireConnection;5;0;4;0
WireConnection;5;1;20;0
WireConnection;62;0;61;0
WireConnection;62;1;29;0
WireConnection;62;2;63;0
WireConnection;60;0;59;0
WireConnection;60;1;5;0
WireConnection;60;2;57;0
WireConnection;39;0;60;0
WireConnection;39;1;62;0
WireConnection;0;0;7;0
WireConnection;0;1;39;0
WireConnection;0;3;9;0
WireConnection;0;4;10;0
WireConnection;0;9;7;4
ASEEND*/
//CHKSM=AEB4C17DB5033FC32A339BF84C7A2D9B49775813