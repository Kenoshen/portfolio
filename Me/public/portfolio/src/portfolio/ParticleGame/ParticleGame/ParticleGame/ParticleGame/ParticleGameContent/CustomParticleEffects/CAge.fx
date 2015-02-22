float MaxAge;
float AgeStep;
float4 PlanePoint;
float4 Normal;

texture Position;
texture Data;

sampler PositionSampler = sampler_state
{
    Texture = (Position);
    
	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler DataSampler = sampler_state
{
    Texture = (Data);
    
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
    output.TexCoord = input.TexCoord;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 dataVector = tex2D(DataSampler, input.TexCoord);

	if (dataVector.z == 0)
	{
		float4 posVector = tex2D(PositionSampler, input.TexCoord);
		float4 modPos = normalize(posVector - PlanePoint);
		if (dot(Normal, modPos) < 0)
		{	
			dataVector.z = dataVector.z + AgeStep;
		}
	}
	else if (dataVector.z > 0)
	{
		dataVector.z = dataVector.z + AgeStep;
	}


	if (dataVector.z >= MaxAge)
	{
		dataVector.z = -1;
	}

	return dataVector;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
