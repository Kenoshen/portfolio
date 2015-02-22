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
    class UseCase_Snow : UseCase
    {
        ParticleSystem ps;

        DiscDomain source;

        public UseCase_Snow(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===Snow===\n " + helpMessage;

            int emit = 40;
            float timeStep = 1;
            float maxAge = 260;
            float pSize = 1;

            ps = new ParticleSystem(((int)maxAge * emit) + 1000, maxAge, ParticleSystemPreSetType.WATER, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            source = new DiscDomain(Vector3.Up * 30, Vector3.Up, 200);

            ps.AddPermanentAction(new SourceAction(GraphicsDevice, Content, source, null, pSize, emit, timeStep));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new AgeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new GravityAction(GraphicsDevice, Content, new Vector3(0, -0.005f, 0)));
            ps.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, Vector3.Zero, Vector3.Up, 0.01f, timeStep));

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();

            Vector3 camPos = cam.Position;
            Vector3 sourcePos = source.GetCenter();
            sourcePos = camPos;
            sourcePos.Y += 50;
            source.SetCenter(sourcePos);

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
            DrawHelpMessage(Vector2.UnitY * 80);
            spriteBatch.End();

            ps.DrawParticles(cam.View, cam.Projection, cam.Up);
        }
    }
}
