float Inverted;
texture ScreenTexture;

sampler ScreenTextureSampler = sampler_state
{
    Texture = (ScreenTexture);
    
	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
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

	output.Position = input.Position;
	if (Inverted == 1)
	{
		output.TexCoord = float2(1 - input.TexCoord.x, input.TexCoord.y);
	}
	else
	{
		output.TexCoord = input.TexCoord;
	}

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(ScreenTextureSampler, input.TexCoord);

	if (Inverted == 1)
	{
		color.x = 1 - color.x;
		color.y = 1 - color.y;
		color.z = 1 - color.z;
	}

	color.w = 1;

	return color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
