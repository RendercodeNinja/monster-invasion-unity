Shader "EverCG(Sethu)/Sprites/Unlit" 
{	
	Properties
	{
		//The main texture
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}		
		//The main color
		_Color("Tint", Color) = (1,1,1,1)
		
	}		

	SubShader
	{	
		//Shader Tags
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

			//Set blending mode to alpha blending
			Blend SrcAlpha OneMinusSrcAlpha
			//Enable Depth writing
			ZWrite Off
			//Disable back face culling
			Cull Off
			//Disable Lighting
			Lighting Off

		//Single Pass
		Pass
		{
			SetTexture[_MainTex]
			{
				constantColor[_Color]
				Combine texture * constant, texture * constant
			}		
		}	
		
	}

	
	FallBack "Unlit/Texture"
}
