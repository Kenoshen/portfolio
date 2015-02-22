float3 Center;
float3 Normal;
float Radius;

float MaxRange;
float Magnitude;
float Epsilon;
float TimeStep;

texture Position;
texture Velocity;

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
	float4 pos = tex2D(PositionSampler, input.TexCoord);
	float3 pPosition = float3(pos.x, pos.y, pos.z);
	float4 velocity = tex2D(VelocitySampler, input.TexCoord);

	float3 pPos = pPosition - Center;

	float3 crossP = cross(pPos, Normal);
	float3 pnt = cross(Normal, crossP);
	normalize(pnt);

	pnt = pnt * Radius;

	float curRange = distance(pPosition, pnt);
	float force = 0;

	if (curRange <= MaxRange)
	{
		force = Magnitude / ((curRange * curRange) + Epsilon);
	}

	float3 addVel = ((Center + pnt) - pPosition) * force;
	
	velocity = velocity + float4(addVel.x, addVel.y, addVel.z, 0);

	return velocity;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
