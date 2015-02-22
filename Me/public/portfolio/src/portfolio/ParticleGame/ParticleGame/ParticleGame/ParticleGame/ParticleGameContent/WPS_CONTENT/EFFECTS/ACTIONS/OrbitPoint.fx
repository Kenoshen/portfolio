float4 OrbitPoint;

float MaxRange;
float Magnitude;
float Epsilon;
float TimeStep;

texture Velocity;
texture Position;

sampler VelocitySampler = sampler_state
{
    Texture = (Velocity);
    
	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler PositionSampler = sampler_state
{
    Texture = (Position);
    
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
	float4 position = tex2D(PositionSampler, input.TexCoord);
	float currentRange = abs(distance(position, OrbitPoint));
	float force = 0;

	if (currentRange < MaxRange)
	{
		force = Magnitude / ((currentRange * currentRange) + Epsilon);
	}

	return tex2D(VelocitySampler, input.TexCoord) + ((OrbitPoint - position) * force);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
