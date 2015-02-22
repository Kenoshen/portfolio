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
    class UseCase_Sprial : UseCase
    {
        ParticleSystem ps;

        OrbitAxisAction oaa;
        PointDomain pd;

        public UseCase_Sprial(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Spiral===\n+ to incrase gravity orb magnitude\n- to decrease gravity orb magnitude\n> to increase gravity orb range\n< to decrease gravity orb range\nNumPad 8, 2 change the Y position\nNumPad 7, 3 change the Z position" + helpMessage;

            float emit = 1;
            float timeStep = 1f;
            float maxAge = 2000;
            float pSize = 0.8f;
            Vector3 vel = new Vector3(1, -1, 0) * 0.3f;
            Vector3 pos = new Vector3(-35, 10, 0);
            float mag = 0.05f;

            oaa = new OrbitAxisAction(GraphicsDevice, Content, Vector3.Zero + pos, Vector3.Right + pos, 10, mag, 1, timeStep);
            pd = new PointDomain(new Vector3(1, 1, 3) + pos);

            ps = new ParticleSystem((int)(maxAge * emit) + 1000, maxAge, ParticleSystemPreSetType.FIRE, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            ps.AddPermanentAction(new SourceAction(GraphicsDevice, Content, pd, new LineDomain(vel * 1f, vel * 1.0f), pSize, emit, timeStep));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new AgeAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(oaa);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();

            Vector3 pos = pd.GetPoint();
            if (keyboard.IsKeyBeingPressed(Keys.NumPad8))
                pos.Y += 0.1f;
            if (keyboard.IsKeyBeingPressed(Keys.NumPad2))
                pos.Y -= 0.1f;
            if (keyboard.IsKeyBeingPressed(Keys.NumPad7))
                pos.Z -= 0.1f;
            if (keyboard.IsKeyBeingPressed(Keys.NumPad3))
                pos.Z += 0.1f;
            if (keyboard.IsKeyBeingPressed(Keys.NumPad4))
                pos.X -= 0.1f;
            if (keyboard.IsKeyBeingPressed(Keys.NumPad6))
                pos.X += 0.1f;
            pd.SetPoint(pos);

            if (keyboard.IsKeyBeingPressed(Keys.OemPlus))
                oaa.Magnitude += 0.001f;
            if (keyboard.IsKeyBeingPressed(Keys.OemMinus))
                oaa.Magnitude -= 0.001f;

            if (keyboard.IsKeyBeingPressed(Keys.OemPeriod))
                oaa.MaxRange += 0.01f;
            if (keyboard.IsKeyBeingPressed(Keys.OemComma))
                oaa.MaxRange -= 0.01f;

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
                oaa.Magnitude = 0.05f;
                oaa.MaxRange = 10;
                pd.SetPoint(new Vector3(-34, 11, 3));
                ps.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
            }

            if (keyboard.IsKeyJustPressed(Keys.H))
                showHelp = !showHelp;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            ps.ApplyActions();

            GraphicsDevice.Clear(Color.Black);

            DrawFloor();
            spriteBatch.Begin();
            DrawDebugger_FPS(gameTime, Vector2.UnitY * 5);
            DrawDebugger_ParticleCount(ps, Vector2.UnitY * 30);
            DrawHelpMessage(Vector2.UnitY * 55);
            spriteBatch.End();

            ps.DrawParticles(cam.View, cam.Projection, cam.Up);
        }
    }
}
