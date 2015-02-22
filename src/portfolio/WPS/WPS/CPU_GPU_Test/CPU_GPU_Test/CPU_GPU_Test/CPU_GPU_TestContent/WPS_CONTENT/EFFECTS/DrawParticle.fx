float4x4 View;
float4x4 Projection;
float3 Normal;
float2 PositionTextureCoord;

float Width;
float Height;

texture PositionTexture;
texture Texture;

float Alpha;

sampler Sampler = sampler_state
{
    Texture = (Texture);

	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};
sampler PositionSampler = sampler_state
{
    Texture = (PositionTexture);
	
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

	float3 pos = tex2Dlod(PositionSampler, float4(PositionTextureCoord.xy, 0, 1));
	float3 position = float3(pos.x, pos.y, pos.z);
	position = position * 1;

	position += rightVector * (input.TexCoord.x - 0.5) * Width;
	position += Normal * (0.5 - input.TexCoord.y) * Height;

	float4 viewPosition = mul(float4(position, 1), View);

	output.Position = mul(viewPosition, Projection);
    output.TexCoord = input.TexCoord;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(Sampler, input.TexCoord);
	color.w = Alpha;
	return color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
