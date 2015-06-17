
cbuffer cbPerResize : register(b0)
{
	matrix Projection;
};

cbuffer cbPerFrame : register(b1)
{
	matrix View;
};

cbuffer cbPerObject : register(b2)
{
	matrix World;
};

Texture2D ObjTexture;
SamplerState ObjSamplerState;

struct VS_INPUT
{
	float4 Pos : POSITION;
	float4 Color : COLOR;
	float2 TexCoord: TEXCOORD;
};

struct PS_INPUT
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR;
	float2 TexCoord: TEXCOORD;
};

PS_INPUT Vertex_Shader(VS_INPUT input)
{
	PS_INPUT output = (PS_INPUT)0;

	output.Color = input.Color;
	output.TexCoord = input.TexCoord;

	output.Pos = mul(input.Pos, World);
	output.Pos = mul(output.Pos, View);
	output.Pos = mul(output.Pos, Projection);

	return output;
}


float4 Pixel_Shader_Blue(PS_INPUT input) : SV_TARGET
{
	return float4(0.0f, 0.0f, 1.0f, 1.0f);
}

float4 Pixel_Shader_Color(PS_INPUT input) : SV_TARGET
{
	return input.Color;
}

float4 Pixel_Shader_Texture(PS_INPUT input) : SV_TARGET
{
	return ObjTexture.Sample(ObjSamplerState, input.TexCoord);
}
