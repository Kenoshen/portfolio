using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Draw
{
    public class Draw3DLine
    {
        VertexPositionColor[] vertices = new VertexPositionColor[2];
        short[] indexes = new short[2];

        public Vector3 StartPos
        {
            get
            {
                return vertices[0].Position;
            }
            set
            {
                vertices[0].Position = value;
            }
        }

        public float StartPosX
        {
            get { return vertices[0].Position.X; }
            set { vertices[0].Position.X = value; }
        }
        public float StartPosY
        {
            get { return vertices[0].Position.Y; }
            set { vertices[0].Position.Y = value; }
        }
        public float StartPosZ
        {
            get { return vertices[0].Position.Z; }
            set { vertices[0].Position.Z = value; }
        }

        public Vector3 EndPos
        {
            get
            {
                return vertices[1].Position;
            }
            set
            {
                vertices[1].Position = value;
            }
        }

        public float EndPosX
        {
            get { return vertices[1].Position.X; }
            set { vertices[1].Position.X = value; }
        }
        public float EndPosY
        {
            get { return vertices[1].Position.Y; }
            set { vertices[1].Position.Y = value; }
        }
        public float EndPosZ
        {
            get { return vertices[1].Position.Z; }
            set { vertices[1].Position.Z = value; }
        }

        public Color Color
        {
            get
            {
                return vertices[0].Color;
            }
            set
            {
                vertices[0].Color = value;
                vertices[1].Color = value;
            }
        }

        public Draw3DLine(Vector3 start, Vector3 end, Color color)
        {
            Initialize();
            StartPos = start;
            EndPos = end;
            Color = color;
        }

        public Draw3DLine(Vector3 start, Vector3 end)
        {
            Initialize();
            StartPos = start;
            EndPos = end;
            Color = Color.White;
        }

        private void Initialize()
        {
            vertices[0] = new VertexPositionColor(Vector3.Zero, Color.White);
            indexes[0] = 0;
            vertices[1] = new VertexPositionColor(Vector3.Zero, Color.White);
            indexes[1] = 1;
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 2, indexes, 0, 1);
        }
    }
}
