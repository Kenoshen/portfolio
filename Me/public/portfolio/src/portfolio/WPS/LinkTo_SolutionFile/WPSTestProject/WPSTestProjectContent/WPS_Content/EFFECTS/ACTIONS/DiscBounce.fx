float4 Center;
float4 Normal;

float Radius;
float Dampening;
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
	float4 velocity = tex2D(VelocitySampler, input.TexCoord);
	float4 position = tex2D(PositionSampler, input.TexCoord);

	if (distance(position, Center) > Radius)
	{
		return velocity;
	}

	if (dot(Normal, velocity * TimeStep) == 0)
	{
		return velocity;
	}
	
	float s1 = (dot(-Normal, position - Center)) / (dot(Normal, velocity * TimeStep));

	if (s1 < 0 || 1 < s1)
	{
		return velocity;
	}

	float4 reflectedVelocity = (-2 * dot(Normal, velocity * TimeStep) * Normal) + (velocity * TimeStep);

	return reflectedVelocity * Dampening;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
