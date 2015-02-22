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
    public class CustomSourceAction : WPSAction
    {
        public Vector3 BasePosition;
        public Vector3 BaseVelocity;
        public List<Vector4> Positions;
        public float Size;
        GraphicsDevice GraphicsDevice;
        ContentManager Content;

        public CustomSourceAction(GraphicsDevice GraphicsDevice, ContentManager Content, List<Vector4> newPositions, Vector3 basePoint, Vector3 baseVel, float size = 1)
        {
            Positions = newPositions;
            BasePosition = basePoint;
            BaseVelocity = baseVel;
            Size = size;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "Source");

            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content;
        }

        public override void ApplyAction(DataTexture position, DataTexture velocity, DataTexture data, float maxAge, WPS.CustomModel.Quad quad)
        {
            int numOfNewParticles = 0;
            RenderTarget2D tempTex = new RenderTarget2D(GraphicsDevice, position.CurrentTexture.Width, position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            Vector4[] tempVect = new Vector4[tempTex.Width * tempTex.Height];
            List<int> indexes = new List<int>();

            data.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < tempVect.Length; i++)
            {
                if (numOfNewParticles >= Positions.Count)
                    break;

                if (tempVect[i].Z == -1)
                {
                    indexes.Add(i);
                    tempVect[i].X = 0; // alpha
                    tempVect[i].Y = Size; // size
                    tempVect[i].Z = 0; // age
                    //tempVect[i].W = 0; // rotation

                    numOfNewParticles++;
                }
            }
            tempTex.SetData(tempVect);
            effect.Parameters["From"].SetValue(tempTex);
            data.DrawDataToTexture(effect, quad);

            tempTex = new RenderTarget2D(GraphicsDevice, position.CurrentTexture.Width, position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            position.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < indexes.Count; i++)
                tempVect[indexes[i]] = Positions[i] + new Vector4(BasePosition, 0);
            tempTex.SetData(tempVect);
            effect.Parameters["From"].SetValue(tempTex);
            position.DrawDataToTexture(effect, quad);

            tempTex = new RenderTarget2D(GraphicsDevice, velocity.CurrentTexture.Width, velocity.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            velocity.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < indexes.Count; i++)
                tempVect[indexes[i]] = new Vector4(BaseVelocity, 0);
            tempTex.SetData(tempVect);
            effect.Parameters["From"].SetValue(tempTex);
            velocity.DrawDataToTexture(effect, quad);
        }

        public override void ApplyActionCPU(Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {
            int numOfNewParticles = 0;
            List<int> indexes = new List<int>();

            for (int i = 0; i < data.Length; i++)
            {
                if (numOfNewParticles >= Positions.Count)
                    break;

                if (data[i].Z == -1)
                {
                    indexes.Add(i);
                    data[i].X = 0; // alpha
                    data[i].Y = Size; // size
                    data[i].Z = 0; // age
                    //data[i].W = 0; // rotation

                    numOfNewParticles++;
                }
            }

            for (int i = 0; i < indexes.Count; i++)
                position[indexes[i]] = Positions[i] + new Vector4(BasePosition, 0);

            for (int i = 0; i < indexes.Count; i++)
                velocity[indexes[i]] = new Vector4(BaseVelocity, 0);
        }
    }
}
