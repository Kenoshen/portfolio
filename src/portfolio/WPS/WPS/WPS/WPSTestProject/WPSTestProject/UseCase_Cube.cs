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
    class UseCase_Cube : UseCase
    {
        ParticleSystem ps;
        float side;
        float timeStep;
        int emit;
        float pSize;
        
        public UseCase_Cube(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Cube===\nNumPad 1-9 to pull particles to corresponding locations on cube\nNumPad 0 to stop particles" + helpMessage;

            emit = 70000;
            timeStep = 0.5f;
            float maxAge = 200;
            pSize = 0.3f;
            side = 20f;
            float dampening = 1f;

            ps = new ParticleSystem(75000, maxAge, ParticleSystemPreSetType.SMOKE, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            ps.AddActionForThisFrame(new SourceAction(GraphicsDevice, Content, new BoxDomain(Vector3.Up * (side / 2), side, side, side), null, pSize, emit));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));

            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, new Vector3(0, side, 0), Vector3.Up, dampening, timeStep));
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, new Vector3(0, 0, 0), Vector3.Down, dampening, timeStep));
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, new Vector3(side / 2, side, 0), Vector3.Right, dampening, timeStep));
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, new Vector3(-side / 2, side, 0), Vector3.Left, dampening, timeStep));
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, new Vector3(0, side / 2, -side / 2), Vector3.Forward, dampening, timeStep));
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, new Vector3(0, side / 2, side / 2), Vector3.Backward, dampening, timeStep));
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

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
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
