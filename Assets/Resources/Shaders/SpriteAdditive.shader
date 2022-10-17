Shader "EverCG(Sethu)/Sprites/Additive" 
{	
	Properties
	{
		//The main texture
		[PerRendererData] _MainTex("Texture", 2D) = "white" {}
	}

	Category
	{
		//Shader Tags
		Tags
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"			
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		//Set blending mode to additive
		Blend One One
		//Disable depth writing
		ZWrite Off
		//Cull back
		Cull back
		//Disable Lighting
		Lighting Off
		
		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

		SubShader
		{
			//Single pass
			Pass
			{
				SetTexture[_MainTex] { combine texture * primary }
			}
		}
	}

	FallBack "Mobile/Particles/Additive"
}
