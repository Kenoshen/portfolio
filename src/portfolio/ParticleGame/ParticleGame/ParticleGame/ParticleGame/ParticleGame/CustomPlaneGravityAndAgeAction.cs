using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WPS;

namespace ParticleGame
{
    public class CustomPlaneGravityAndAgeAction : WPSAction
    {
        public Vector3 PlanePoint;
        public Vector3 Normal;
        public float Force;
        public float VectorRadius;
        public float AgeStep;
        private Random rnd;
        private Effect gravEffect;

        public CustomPlaneGravityAndAgeAction(GraphicsDevice GraphicsDevice, ContentManager Content, Vector3 planePoint, Vector3 normal, float force, float vectorRadius, float ageStep = 1f)
        {
            PlanePoint = planePoint;
            Normal = normal;
            Force = force;
            VectorRadius = vectorRadius;
            AgeStep = ageStep;

            rnd = new Random();

            effect = Content.Load<Effect>("CustomParticleEffects/CAge");
            gravEffect = Content.Load<Effect>("CustomParticleEffects/CGravity");
        }

        public override void ApplyAction(DataTexture position, DataTexture velocity, DataTexture data, float maxAge, WPS.CustomModel.Quad quad)
        {
            Normal.Normalize();
            effect.Parameters["Position"].SetValue(position.CurrentTexture);
            effect.Parameters["Data"].SetValue(data.CurrentTexture);
            effect.Parameters["MaxAge"].SetValue(maxAge);
            effect.Parameters["AgeStep"].SetValue(AgeStep);
            effect.Parameters["PlanePoint"].SetValue(new Vector4(PlanePoint, 0));
            effect.Parameters["Normal"].SetValue(new Vector4(Normal, 0));
            data.DrawDataToTexture(effect, quad);

            gravEffect.Parameters["Velocity"].SetValue(velocity.CurrentTexture);
            gravEffect.Parameters["Data"].SetValue(data.CurrentTexture);
            gravEffect.Parameters["Normal"].SetValue(Normal);
            gravEffect.Parameters["VectorRadius"].SetValue(VectorRadius);
            gravEffect.Parameters["RandomRotation"].SetValue(rnd.Next(0, 1000));
            gravEffect.Parameters["Force"].SetValue(Force);
            gravEffect.Parameters["AgeStep"].SetValue(AgeStep);
            velocity.DrawDataToTexture(gravEffect, quad);
        }
    }
}
