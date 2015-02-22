texture Velocity;
texture Data;

float3 Normal;
float VectorRadius;
float RandomRotation;
float Force;
float AgeStep;

float3x3 I = {
			1, 0, 0,
			0, 1, 0,
			0, 0, 1
			};

sampler VelocitySampler = sampler_state
{
    Texture = (Velocity);
    
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
	float4 velVector = tex2D(VelocitySampler, input.TexCoord);
	float4 dataVector = tex2D(DataSampler, input.TexCoord);

	if (dataVector.z == AgeStep)
	{
		float3x3 tensorProd = {
							Normal.x * Normal.x, Normal.x * Normal.y, Normal.x * Normal.z,
							Normal.x * Normal.y, Normal.y * Normal.y, Normal.y * Normal.z,
							Normal.x * Normal.z, Normal.y * Normal.z, Normal.z * Normal.z
							};

		float3x3 crossProd = {
							0, -Normal.z, Normal.y,
							Normal.z, 0, -Normal.x,
							-Normal.y, Normal.x, 0
							};

		float3x3 rotMatrix = I * cos(RandomRotation * input.TexCoord.x * input.TexCoord.y) + sin(RandomRotation) * crossProd + (1 - cos(RandomRotation)) * tensorProd;

		float3 crossVect = cross(Normal, float3(0, 1, 0));
		crossVect = normalize(crossVect) * VectorRadius;

		float3 newVelVector = (mul(crossVect, rotMatrix) - Normal) * Force;

		velVector = float4(newVelVector.x, newVelVector.y, newVelVector.z, 0);
	}

	return velVector;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
