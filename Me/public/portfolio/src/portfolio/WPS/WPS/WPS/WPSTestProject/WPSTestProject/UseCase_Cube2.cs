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
    class UseCase_Cube2 : UseCase
    {
        ParticleSystem ps;
        float side;
        float timeStep;
        int emit;
        float pSize;
        OrbitPointAction opa;

        public UseCase_Cube2(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Cube 2===\n+ to incrase gravity orb magnitude\n- to decrease gravity orb magnitude\n> to increase gravity orb range\n< to decrease gravity orb range\nArrow keys to move gravity orb\nNumPad 1-9 to pull particles to corresponding locations on cube\nNumPad 0 to stop particles" + helpMessage;

            emit = 70000;
            timeStep = 0.5f;
            float maxAge = 200;
            pSize = 0.3f;
            side = 20f;

            ps = new ParticleSystem(75000, maxAge, ParticleSystemPreSetType.FIRE, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            opa = new OrbitPointAction(GraphicsDevice, Content, new Vector3(0, 10, 0), 5, 0f, 0.5f, 1);

            ps.AddActionForThisFrame(new SourceAction(GraphicsDevice, Content, new BoxDomain(Vector3.Up * (side / 2), side, side, side), null, pSize, emit, timeStep));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(opa);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();

            if (keyboard.IsKeyJustPressed(Keys.NumPad1))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((-side / 2), 0, (side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad2))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((side / 2), 0, (side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad3))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((side / 2), 0, (-side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad4))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((-side / 2), 0, (-side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad5))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, Vector3.Up * (side / 2), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad6))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((-side / 2), side, (side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad7))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((side / 2), side, (side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad8))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((side / 2), side, (-side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad9))
                ps.AddActionForThisFrame(new OrbitPointAction(GraphicsDevice, Content, new Vector3((-side / 2), side, (-side / 2)), 10000, 1, 100, timeStep));
            if (keyboard.IsKeyJustPressed(Keys.NumPad0))
                ps.AddActionForThisFrame(new ZeroVelAction(GraphicsDevice, Content));

            float movement = 0.05f;
            float range = 0.2f;

            if (keyboard.IsKeyBeingPressed(Keys.Up))
                opa.OrbitPoint.Y += movement;
            if (keyboard.IsKeyBeingPressed(Keys.Down))
                opa.OrbitPoint.Y -= movement;
            if (keyboard.IsKeyBeingPressed(Keys.Right))
                opa.OrbitPoint.X += movement;
            if (keyboard.IsKeyBeingPressed(Keys.Left))
                opa.OrbitPoint.X -= movement;

            if (keyboard.IsKeyBeingPressed(Keys.OemPeriod))
                opa.MaxRange += range;
            if (keyboard.IsKeyBeingPressed(Keys.OemComma))
                opa.MaxRange -= range;

            if (keyboard.IsKeyJustPressed(Keys.OemPlus))
                opa.Magnitude += 0.1f;
            if (keyboard.IsKeyJustPressed(Keys.OemMinus))
                opa.Magnitude -= 0.1f;

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
                opa.Magnitude = 0;
                opa.MaxRange = 5;
                opa.OrbitPoint = new Vector3(0, 10, 0);
                ps.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
                ps.AddActionForThisFrame(new SourceAction(GraphicsDevice, Content, new BoxDomain(Vector3.Up * (side / 2), side, side, side), null, pSize, emit, timeStep));
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
