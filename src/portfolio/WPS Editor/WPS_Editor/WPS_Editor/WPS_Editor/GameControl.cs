using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS;
using WPS.Camera;
using WPS.Input;
using WPS.CustomModel;

namespace WPS_Editor
{
    class GameControl : GraphicsDeviceControl
    {
        BasicEffect effect;
        Stopwatch timer;
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        public Color GD_Clear_Color;
        public ArcBallCamera cam;
        CMouse mouse;
        CKeyboard keyboard;

        Animator a;

        // Vertex positions and colors used to display a spinning triangle.
        public readonly VertexPositionColor[] Vertices =
        {
            new VertexPositionColor(new Vector3(0,   0, -10), Color.Gray),
            new VertexPositionColor(new Vector3(-10, 0, 10), Color.Gray),
            new VertexPositionColor(new Vector3(10,  0, 10), Color.Gray),
        };


        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            cam = new ArcBallCamera(new Vector3(0, 0, 0), 0, 0, -100, 100, 20, 1, 500, GraphicsDevice);
            mouse = new CMouse();
            keyboard = new CKeyboard();
            content = new ContentManager(Services, "Content");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = content.Load<SpriteFont>("hudFont");

            // Create our effect.
            effect = new BasicEffect(GraphicsDevice);

            effect.VertexColorEnabled = true;

            // Start the animation timer.
            timer = Stopwatch.StartNew();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            GD_Clear_Color = Color.Black;
        }

        public void LoadNewAnimator(string filename, string contentProjectName)
        {
            a = new Animator(filename, contentProjectName, GraphicsDevice, content);
        }

        public void LoadNewAnimator(string absoluteFilePath)
        {
            a = new Animator(absoluteFilePath, GraphicsDevice, content);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            mouse.BeginUpdate();
            keyboard.BeginUpdate();
            UpdateCamera();
            GraphicsDevice.Clear(GD_Clear_Color);

            if (a != null)
                a.ApplyActions();

            DrawFloor();
            if (a != null)
                a.DrawAnimator(cam.View, cam.Projection, cam.Up);

            if (keyboard.IsKeyJustPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                Application.Exit();
            mouse.EndUpdate();
            keyboard.EndUpdate();
        }

        private void UpdateCamera()
        {
            if (keyboard.IsKeyBeingPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Vector2 posDiff = mouse.GetPositionDifference() * -0.005f;
                float zoomDiff = mouse.GetScrollDifference() * 10;

                if (zoomDiff == 0)
                {
                    if (keyboard.IsKeyBeingPressed(Microsoft.Xna.Framework.Input.Keys.Up))
                        zoomDiff -= 0.1f;
                    if (keyboard.IsKeyBeingPressed(Microsoft.Xna.Framework.Input.Keys.Down))
                        zoomDiff += 0.1f;
                }

                cam.Rotate(posDiff.X, posDiff.Y);
                cam.Move(zoomDiff);

                Microsoft.Xna.Framework.Input.Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            }

            cam.Update();
        }

        private void DrawFloor()
        {
            effect.World = Matrix.Identity;

            effect.View = cam.View;

            effect.Projection = cam.Projection;

            // Set renderstates.
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                              Vertices, 0, 1);
        }

        private void DrawSpinningTriangle()
        {
            //// Spin the triangle according to how much time has passed.
            //float time = (float)timer.Elapsed.TotalSeconds;

            //float yaw = time * 0.7f;
            //float pitch = time * 0.8f;
            //float roll = time * 0.9f;

            //// Set transform matrices.
            //float aspect = GraphicsDevice.Viewport.AspectRatio;

            //effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);

            //effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -8),
            //                                  Vector3.Zero, Vector3.Up);

            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

            //// Set renderstates.
            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            //// Draw the triangle.
            //effect.CurrentTechnique.Passes[0].Apply();

            //GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
            //                                  Vertices, 0, 1);
        }
    }
}
