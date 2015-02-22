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
    class UseCase_DualSprayBounce : UseCase
    {
        ParticleSystem ps;
        ParticleSystem ps2;

        OrbitPointAction opa;

        public UseCase_DualSprayBounce(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Dual Spray Bounce===\n+ to incrase gravity orb magnitude\n- to decrease gravity orb magnitude\n> to increase gravity orb range\n< to decrease gravity orb range\nArrow keys to move gravity orb" + helpMessage;

            int emit = 5;
            float timeStep = 1;
            float maxAge = 160;
            float pSize = 2;

            ps = new ParticleSystem(((int)maxAge * emit) + 1000, maxAge, ParticleSystemPreSetType.EXPLOSION, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            ps2 = new ParticleSystem(((int)maxAge * emit) + 1000, maxAge, ParticleSystemPreSetType.WATER, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            opa = new OrbitPointAction(GraphicsDevice, Content, new Vector3(0, 3, 0), 15, 0f, 0.5f, 1);
            DiscBounceAction dba = new DiscBounceAction(GraphicsDevice, Content, Vector3.Zero, Vector3.Up, 10, 0.5f);

            ps.AddPermanentAction(new SourceAction(GraphicsDevice, Content, new SquareDomain(new Vector3(-30, 7, 0), new Vector3(1, 1, 0), 5, 5), new LineDomain(new Vector3(1, 1, 0) * 0.3f, new Vector3(1, 1, 0) * 0.3f), pSize, emit, timeStep));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new AgeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new GravityAction(GraphicsDevice, Content, new Vector3(0, -0.01f, 0)));
            ps.AddPermanentAction(opa);
            ps.AddPermanentAction(dba);

            ps2.AddPermanentAction(new SourceAction(GraphicsDevice, Content, new SquareDomain(new Vector3(30, 7, 0), new Vector3(-1, 1, 0), 5, 5), new LineDomain(new Vector3(-1, 1, 0) * 0.3f, new Vector3(-1, 1, 0) * 0.3f), pSize, emit, timeStep));
            ps2.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps2.AddPermanentAction(new AgeAction(GraphicsDevice, Content));
            ps2.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps2.AddPermanentAction(new GravityAction(GraphicsDevice, Content, new Vector3(0, -0.01f, 0)));
            ps2.AddPermanentAction(opa);
            ps2.AddPermanentAction(dba);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();

            float movement = 0.1f;

            if (keyboard.IsKeyBeingPressed(Keys.Up))
                opa.OrbitPoint.Y += movement;
            if (keyboard.IsKeyBeingPressed(Keys.Down))
                opa.OrbitPoint.Y -= movement;
            if (keyboard.IsKeyBeingPressed(Keys.Right))
                opa.OrbitPoint.X += movement;
            if (keyboard.IsKeyBeingPressed(Keys.Left))
                opa.OrbitPoint.X -= movement;

            if (keyboard.IsKeyJustPressed(Keys.OemPlus))
                opa.Magnitude += 0.1f;
            if (keyboard.IsKeyJustPressed(Keys.OemMinus))
                opa.Magnitude -= 0.1f;

            if (keyboard.IsKeyBeingPressed(Keys.OemPeriod))
                opa.MaxRange += 0.2f;
            if (keyboard.IsKeyBeingPressed(Keys.OemComma))
                opa.MaxRange -= 0.2f;

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
                opa.MaxRange = 15;
                opa.Magnitude = 0;
                opa.OrbitPoint = new Vector3(0, 3, 0);
                ps.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
                ps2.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
            }

            if (keyboard.IsKeyJustPressed(Keys.H))
                showHelp = !showHelp;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            ps.ApplyActions();
            ps2.ApplyActions();

            GraphicsDevice.Clear(Color.Black);

            DrawFloor();
            spriteBatch.Begin();
            DrawDebugger_FPS(gameTime, Vector2.UnitY * 5);
            DrawDebugger_ParticleCount(ps, Vector2.UnitY * 30);
            DrawDebugger_ParticleCount(ps2, Vector2.UnitY * 55);
            DrawHelpMessage(Vector2.UnitY * 80);
            spriteBatch.End();

            ps.DrawParticles(cam.View, cam.Projection, cam.Up);
            ps2.DrawParticles(cam.View, cam.Projection, cam.Up);
        }
    }
}
