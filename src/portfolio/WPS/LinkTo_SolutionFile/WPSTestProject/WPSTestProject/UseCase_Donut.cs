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
    class UseCase_Donut : UseCase
    {
        ParticleSystem ps;

        SourceAction source;
        OrbitRingAction orbit;

        public UseCase_Donut(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Donut===\nPress E to spawn ring of particles\n" + helpMessage;

            int emit = 100;
            float timeStep = 1f;
            float maxAge = 1000;
            float pSize = 0.8f;

            source = new SourceAction(GraphicsDevice, Content, new RingDomain(Vector3.Up * 4, Vector3.Up, 10), new PointDomain(Vector3.Up * 0.3f), pSize, emit, timeStep);
            orbit = new OrbitRingAction(GraphicsDevice, Content, Vector3.Up * 5, Vector3.Up, 8f, 1000f, 1f, 1f, timeStep);

            ps = new ParticleSystem(10000, maxAge, ParticleSystemPreSetType.FIRE, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            //ps.AddPermanentAction(source);
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new AgeAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new RotateAction(GraphicsDevice, Content, -1, 1));
            ps.AddPermanentAction(orbit);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();

            float incr = 0.1f;
            if (keyboard.IsKeyBeingPressed(Keys.OemMinus))
                orbit.Radius -= incr;
            if (keyboard.IsKeyBeingPressed(Keys.OemPlus))
                orbit.Radius += incr;

            if (keyboard.IsKeyBeingPressed(Keys.OemOpenBrackets))
                orbit.Magnitude -= incr;
            if (keyboard.IsKeyBeingPressed(Keys.OemCloseBrackets))
                orbit.Magnitude += incr;

            if (keyboard.IsKeyBeingPressed(Keys.OemSemicolon))
                orbit.MaxRange -= incr;
            if (keyboard.IsKeyBeingPressed(Keys.OemQuotes))
                orbit.MaxRange += incr;

            if (keyboard.IsKeyJustPressed(Keys.E))
                ps.AddActionForThisFrame(source);

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
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
            //spriteBatch.DrawString(debugger.Font, "Radius: " + orbit.Radius + "\nMagnitude: " + orbit.Magnitude + "\nMaxRange: " + orbit.MaxRange, Vector2.UnitY * 80, debugger.Color);
            spriteBatch.End();

            ps.DrawParticles(cam.View, cam.Projection, cam.Up);
        }
    }
}
