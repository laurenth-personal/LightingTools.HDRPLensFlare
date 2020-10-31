Shader "HDRenderPipeline/LensFlare (HDRP Premultiplied)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"RenderPipeline" = "HDRenderPipeline"
			"RenderType" = "HDUnlitShader"
			"Queue" = "Transparent+500"
		}
		Pass
		{
			Name "ForwardUnlit"
			Tags{ "LightMode" = "Forward"}

			Blend One OneMinusSrcAlpha
			ColorMask RGB
			ZWrite Off
			Cull Off
			ZTest Always

			HLSLPROGRAM

			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag	

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "LensFlareHDRPCommon.hlsl"

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				return col * i.color;
			}

			ENDHLSL
		}
			Pass
			{
				Name "SceneSelectionPass"
				Tags{ "LightMode" = "SceneSelectionPass" }

				Blend One One
				ZWrite Off
				Cull Off
				ZTest Always

				HLSLPROGRAM

#pragma target 4.5
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_instancing
#define SHADERPASS SHADERPASS_DEPTH_ONLY
#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
#define SCENESELECTIONPASS
#pragma editor_sync_compilation

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "LensFlareHDRPCommon.hlsl"

				float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				return saturate((col.a - 0.4f) * 100);
			}

				ENDHLSL
			}
	}
}
