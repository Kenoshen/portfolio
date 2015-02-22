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
    class UseCase_Fountain : UseCase
    {
        ParticleSystemCPU ps;

        LineDomain sourceVel;
        OrbitPointAction opa;

        float sourcePower = 0.1f;
        float orbitMag = -0.23f;

        public UseCase_Fountain(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Fountain===\n+ to increase spout velocity\n- to decrease spout velocity\n> to increase spout spread\n< to decrease spout spread" + helpMessage;

            int emit = 10;
            float timeStep = 0.5f;
            float maxAge = 200;
            float pSize = 0.8f;

            sourceVel = new LineDomain(Vector3.Up * sourcePower, Vector3.Up * sourcePower);
            opa = new OrbitPointAction(GraphicsDevice, Content, Vector3.Zero, 1f, orbitMag, 1, timeStep);

            ps = new ParticleSystemCPU(((int)maxAge * emit) + 1000, maxAge, ParticleSystemPreSetType.WATER, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            ps.AddPermanentAction(new SourceAction(GraphicsDevice, Content, new RingDomain(Vector3.Zero, Vector3.Up, 0.05f), sourceVel, pSize, emit, timeStep));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new AgeAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new GravityAction(GraphicsDevice, Content, new Vector3(0, -0.01f, 0), timeStep));
            ps.AddPermanentAction(opa);
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, Vector3.Zero, Vector3.Up, 0.8f, timeStep));
            ps.AddPermanentAction(new RotateAction(GraphicsDevice, Content, 5, 15));
            ps.AddPermanentAction(new ScaleAction(GraphicsDevice, Content, 5));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();


            if (keyboard.IsKeyBeingPressed(Keys.OemPlus))
            {
                sourcePower += 0.0001f;
                sourceVel.SetStartPoint(Vector3.Up * sourcePower);
                sourceVel.SetEndPoint(Vector3.Up * sourcePower);
            }
            if (keyboard.IsKeyBeingPressed(Keys.OemMinus))
            {
                sourcePower -= 0.0001f;
                sourceVel.SetStartPoint(Vector3.Up * sourcePower);
                sourceVel.SetEndPoint(Vector3.Up * sourcePower);
            }

            if (keyboard.IsKeyBeingPressed(Keys.OemPeriod))
                opa.Magnitude += 0.0001f;
            if (keyboard.IsKeyBeingPressed(Keys.OemComma))
                opa.Magnitude -= 0.0001f;

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
                sourcePower = 0.1f;
                sourceVel.SetStartPoint(Vector3.Up * sourcePower);
                sourceVel.SetEndPoint(Vector3.Up * sourcePower);
                opa.Magnitude = -0.23f;

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
