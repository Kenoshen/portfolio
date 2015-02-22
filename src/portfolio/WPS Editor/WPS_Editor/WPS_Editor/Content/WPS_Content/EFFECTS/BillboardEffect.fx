float4x4 View;
float4x4 Projection;
float3 Normal;

float Width;
float Height;

texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);
	
	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
	float3 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	float3 viewDirection = View._m02_m12_m22;
	float3 rightVector = normalize(cross(viewDirection, Normal));
	float3 position = input.Position;

	position += rightVector * (input.TexCoord.x - 0.5) * Width;
	position += Normal * (0.5 - input.TexCoord.y) * Height;

	float4 viewPosition = mul(float4(position, 1), View);

	output.Position = mul(viewPosition, Projection);
    output.TexCoord = input.TexCoord;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return tex2D(Sampler, input.TexCoord);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
