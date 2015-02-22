float4 PlanePoint;
float4 Normal;

float Width;
float Height;
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
	
	
	if (dot(Normal, velocity * TimeStep) == 0)
	{
		return velocity;
	}

	float s1 = dot(-Normal, position - PlanePoint) / dot(Normal, velocity * TimeStep);

	if (s1 < 0 || 1 < s1)
	{
		return velocity;
	}

	float4 secondVect = float4(0, 1, 0, 0);
	if (length(secondVect - Normal) == 0)
	{
		secondVect = float4(1, 0, 0, 0);
	}
	float4 right = float4(cross(Normal, secondVect).xyz, 0);
	float4 up = float4(cross(Normal, right).xyz, 0);
	float hW = Width / 2;
	float hH = Height / 2;

	float4 topRight = (right * hW) + (up * hH);
	float4 topLeft = (right * -hW) + (up * hH);
	float4 botRight = (right * hW) + (up * -hH);
	float4 botLeft = (right * -hW) + (up * -hH);

	float4 v0 = topRight;
	float4 v1 = topLeft;
	float4 v2 = botRight;
	float4 p0 = position;
	float4 p1 = position + (velocity * TimeStep);
	float4 pi = position + (velocity * TimeStep * s1);
	float4 n = Normal;
	float4 u = v1 - v0;
	float4 v = v2 - v0;
	float4 w = pi - v0;

	float si = ((dot(u, v) * dot(w, v)) - (dot(v, v) * dot(w, u))) / ((dot(u, v) * dot(u, v)) - (dot(u, u) * dot(v, v)));
	float ti = ((dot(u, v) * dot(w, u)) - (dot(u, u) * dot(w, v))) / ((dot(u, v) * dot(u, v)) - (dot(u, u) * dot(v, v)));

	if (si < 0 || ti < 0 || si + ti > 1)
	{
		v0 = topLeft;
		v1 = botRight;
		v2 = botLeft;
		u = v1 - v0;
		v = v2 - v0;
		w = pi - v0;

		si = ((dot(u, v) * dot(w, v)) - (dot(v, v) * dot(w, u))) / ((dot(u, v) * dot(u, v)) - (dot(u, u) * dot(v, v)));
		ti = ((dot(u, v) * dot(w, u)) - (dot(u, u) * dot(w, v))) / ((dot(u, v) * dot(u, v)) - (dot(u, u) * dot(v, v)));

		if (si < 0 || ti < 0 || si + ti > 1)
		{
			return velocity;
		}
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
