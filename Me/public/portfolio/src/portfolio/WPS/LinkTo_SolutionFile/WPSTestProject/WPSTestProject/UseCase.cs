using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WPS;
using WPS.Camera;
using WPS.CustomModel;
using WPS.Input;

namespace WPSTestProject
{
    public class UseCase
    {
        protected GraphicsDeviceManager graphics;
        protected GraphicsDevice GraphicsDevice;
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected Random rnd;
        protected CKeyboard keyboard;
        protected CMouse mouse;
        protected ArcBallCamera cam;
        protected Effect floorEffect;
        protected Texture2D floorTexture;
        protected Quad floor;
        protected Debugger_WPS debugger;
        protected string helpMessage = "\nPage Up/Down to switch demo\nR to reset\nHold SPACE and move mouse to move camera\nTo toggle help press 'H'";
        protected bool showHelp = false;

        public bool Active { get; set; }

        public virtual void Initialize(GraphicsDeviceManager graphics, Random rnd, CKeyboard keyboard, CMouse mouse, ArcBallCamera cam)
        {
            this.rnd = rnd;
            this.keyboard = keyboard;
            this.mouse = mouse;
            this.cam = cam;
            this.graphics = graphics;
            this.GraphicsDevice = graphics.GraphicsDevice;
        }

        public virtual void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            this.debugger = debugger;
            this.floorEffect = floorEffect;
            this.floorTexture = floorTexture;
            this.floor = floor;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public void UpdateCamera()
        {
            ArcBallCamera camera = cam as ArcBallCamera;

            Vector2 mouseDiff = -mouse.GetPositionDifference();
            camera.Rotate(mouseDiff.X * 0.003f, mouseDiff.Y * 0.003f);
            camera.Move(-mouse.GetScrollDifference() * 0.3f);
            if (keyboard.IsKeyBeingPressed(Keys.Up))
                camera.Move(-1);
            if (keyboard.IsKeyBeingPressed(Keys.Down))
                camera.Move(1);
            camera.Update();

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
        }

        public void DrawFloor()
        {
            floorEffect.Parameters["Texture"].SetValue(floorTexture);
            floorEffect.Parameters["View"].SetValue(cam.View);
            floorEffect.Parameters["Projection"].SetValue(cam.Projection);
            floor.RenderQuad(floorEffect,
                new Vector3(-10, -0, -10),
                new Vector3(10, -0, -10),
                new Vector3(10, -0, 10),
                new Vector3(-10, -0, 10));
        }

        public void DrawDebugger_FPS(GameTime gameTime, Vector2 pos)
        {
            debugger.DisplayFPSCounter(spriteBatch, gameTime, pos);
        }

        public void DrawDebugger_ParticleCount(ParticleSystem tps, Vector2 pos)
        {
            debugger.DisplayParticleCount(spriteBatch, tps, pos);
        }

        public void DrawDebugger_ParticleCount(ParticleSystemCPU tps, Vector2 pos)
        {
            debugger.DisplayParticleCount(spriteBatch, tps, pos);
        }

        public void DrawHelpMessage(Vector2 pos)
        {
            if (showHelp)
                spriteBatch.DrawString(debugger.Font, helpMessage, pos, debugger.Color);
            else
                spriteBatch.DrawString(debugger.Font, "Press H for help.", pos, debugger.Color);
        }
    }
}