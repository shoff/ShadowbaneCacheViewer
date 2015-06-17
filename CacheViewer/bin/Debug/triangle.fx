
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

struct VS_INPUT
{
	float4 Pos : POSITION;
};

struct PS_INPUT
{
	float4 Pos : SV_POSITION;
};

float4 Vertex_Shader(float4 inPos : POSITION) : SV_POSITION
{
	return inPos;
}

PS_INPUT VS2(VS_INPUT input)
{
	PS_INPUT output = (PS_INPUT)0;
	output.Pos = mul(input.Pos, World);
	output.Pos = mul(output.Pos, View);
	output.Pos = mul(output.Pos, Projection);

	return output;
}

float4 Pixel_Shader() : SV_TARGET
{
	return float4(0.0f, 0.0f, 1.0f, 1.0f);
}
