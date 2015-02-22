float4x4 View;
float4x4 Projection;
float3 Normal;
float MaxAge;

texture PositionTexture;
texture DataTexture;
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
sampler PositionSampler = sampler_state
{
    Texture = (PositionTexture);
	
	MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};
sampler DataSampler = sampler_state
{
    Texture = (DataTexture);
	
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
	float3 PositionTextureCoord : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
	float3 PositionTextureCoord: NORMAL0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	float3 viewDirection = View._m02_m12_m22;
	float3 rightVector = normalize(cross(viewDirection, Normal));

	float3 position = tex2Dlod(PositionSampler, float4(input.PositionTextureCoord.xyz, 0));

	float4 data = tex2Dlod(DataSampler, float4(input.PositionTextureCoord.xyz, 0));

	float3 rotRightVector = rightVector;
	float3 rotNormal = Normal;
	if (data.w < 0 || 0 < data.w)
	{
		float3 u = cross(rightVector, Normal);
		float rotate = data.w * ((data.z / MaxAge) * 2*3.14);
		float cS = cos(rotate);
		float sN = sin(rotate);

		float3x3 rotMatrix = {	(cS+u.x*u.x*(1-cS)),	(u.x*u.y*(1-cS)-u.z*sN),	(u.x*u.z*(1-cS)+u.y*sN),
								(u.y*u.x*(1-cS)+u.z*sN),	(cS+u.y*u.y*(1-cS)),	(u.y*u.z*(1-cS)-u.x*sN),
								(u.z*u.x*(1-cS)-u.y*sN),	(u.z*u.y*(1-cS)+u.x*sN),	(cS+u.z*u.z*(1-cS))
							 };

		rotRightVector = mul(rightVector, rotMatrix);
		rotNormal = mul(Normal, rotMatrix);
	}
	position += rotRightVector * (input.TexCoord.x - 0.5) * data.y;
	position += rotNormal * (0.5 - input.TexCoord.y) * data.y;

	float4 viewPosition = mul(float4(position, 1), View);

	output.Position = mul(viewPosition, Projection);
    output.TexCoord = input.TexCoord;
	output.PositionTextureCoord = input.PositionTextureCoord;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 data = tex2D(DataSampler, input.PositionTextureCoord.xy);
	if (data.z != -1)
	{
		float4 color = tex2D(Sampler, input.TexCoord);
		color.w = color.w * data.x;
		
		// this is extra stuff to make stuff in background lighter than stuff in foreground
		//float4 position = tex2D(PositionSampler, input.PositionTextureCoord.xy);
		//if (position.z < 0)
		//{
		//	float zValue = ((-position.z / 100));
		//	color.x  = zValue;
		//	color.y  = zValue;
		//	color.z  = zValue;
		//
		//	return color;
		//}
		// ok back to normal stuff

		return color;
	}
	else
	{
		//discard;
	}
	return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
