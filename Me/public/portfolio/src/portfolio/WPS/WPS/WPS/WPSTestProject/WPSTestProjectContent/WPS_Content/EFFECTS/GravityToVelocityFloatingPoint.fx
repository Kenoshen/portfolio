texture Velocity;
texture Position;

float4 Gravity;

float4 PointGravity;
float PointGravityScale;

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
	float4 positionVector = tex2D(PositionSampler, input.TexCoord);
	float4 pointGrav = normalize(PointGravity - positionVector) * PointGravityScale;
	
	float4 velocityVector = tex2D(VelocitySampler, input.TexCoord) + Gravity + pointGrav;

	return velocityVector;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}


// A trick to get a greater precision than 8 bits out of the standard 
// R8 G8 B8 A8 for a render target that only holds one piece of information.
// 
// Converts one float to a float4 so less information is lost when converted to a 32 bit color.
// 
// I need to find a better mathematical way that gives better than 4x improvement 
// because HLSL doesn't support bitwise operations.
float4 PackAsColor(float value)
{
	value *= 4;

	float4 packed = float4(0,0,0,0);
	
	packed.r = value;
	packed.g = value - 1;
	packed.b = value - 2;
	packed.a = value - 3;

	packed = saturate(packed);

	return packed;
}

// Unpacks a packed float4 to something close to the original float value
float UnpackColor(float4 packed)
{
	float unpacked = packed.r + packed.g + packed.b + packed.a;

	return unpacked / 4;
}
