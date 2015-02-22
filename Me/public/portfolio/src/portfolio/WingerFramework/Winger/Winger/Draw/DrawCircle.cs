using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Draw
{
    public class DrawCircle
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Texture2D Texture { get; private set; }

        private int _radius = 0;
        public int Radius
        {
            get { return _radius; }
            set
            {
                _radius = Math.Abs(value);
                TextureCenter = new Vector2(_radius + 1, _radius + 1);
            }
        }
        private Vector2 _center = Vector2.Zero;
        public Vector2 Center
        {
            get { return _center; }
            set
            {
                _center = value;
                _position = new Vector2(_center.X - _radius, _center.Y - _radius);
            }
        }
        private Vector2 _position = Vector2.Zero;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _center = new Vector2(_position.X + _radius, _position.Y + _radius);
            }
        }
        public float Rotation { get; set; }
        public Vector2 TextureCenter { get; private set; }

        private Color _outlineColor = Color.Black;
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set
            {
                _outlineColor = value;
            }
        }
        private Color _fillColor = Color.White;
        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
            }
        }
        private Color _rotationMarkerColor = Color.Black;
        public Color RotationMarkerColor
        {
            get { return _rotationMarkerColor; }
            set
            {
                _rotationMarkerColor = value;
            }
        }

        private bool _hasOutline = false;
        public bool HasOutline
        {
            get { return _hasOutline; }
            set
            {
                _hasOutline = value;
            }
        }
        private bool _hasFill = false;
        public bool HasFill
        {
            get { return _hasFill; }
            set
            {
                _hasFill = value;
            }
        }
        private bool _hasRotationMarker = false;
        public bool HasRotationMarker
        {
            get { return _hasRotationMarker; }
            set
            {
                _hasRotationMarker = value;
            }
        }

        public DrawCircle(int radius, bool hasOutline, bool hasFill, bool hasRotationMarker, GraphicsDevice gd)
        {
            _radius = radius;
            _hasOutline = hasOutline;
            _hasFill = hasFill;
            _hasRotationMarker = hasRotationMarker;

            TextureCenter = new Vector2(_radius + 1, _radius + 1);

            GraphicsDevice = gd;

            UpdateTexture();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, null, Color.White, Rotation, TextureCenter, 1, SpriteEffects.None, 1);
        }


        public void UpdateTexture()
        {
            int outerRadius = _radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            Vector2 center = new Vector2(outerRadius / 2 - 1, outerRadius / 2 - 1);
            if (HasFill || HasRotationMarker)
            {
                for (int x = 0; x < outerRadius; x++)
                {
                    for (int y = 0; y < outerRadius; y++)
                    {
                        // Draw the fill color
                        if (HasFill)
                        {
                            if (Vector2.Distance(center, new Vector2(x, y)) <= _radius)
                            {
                                data[y * outerRadius + x + 1] = FillColor;
                            }
                        }
                        // Draw the rotation marker
                        if (HasRotationMarker)
                        {
                            if ((int)center.X == x && (int)center.Y >= y)
                            {
                                data[y * outerRadius + x + 1] = RotationMarkerColor;
                            }
                        }
                    }
                }
            }

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / _radius;

            // Draw the outline
            if (HasOutline)
            {
                for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                {
                    // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                    int x = (int)Math.Round(_radius + _radius * Math.Cos(angle));
                    int y = (int)Math.Round(_radius + _radius * Math.Sin(angle));

                    data[y * outerRadius + x + 1] = OutlineColor;
                }
            }

            texture.SetData(data);
            Texture = texture;
        }
    }
}
