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
    class UseCase_RingOfFire : UseCase
    {
        ParticleSystemCPU ps;

        RingDomain source;

        public UseCase_RingOfFire(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            #region Unimportant code
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\n===RingOfFire===\n" + helpMessage;

            int emit = 30;
            float timeStep = 0.5f;
            float maxAge = 35;
            float pSize = 0.8f;
            #endregion

            #region Important code

            source = new RingDomain(Vector3.Up * 10, Vector3.Forward, 7);

            ps = new ParticleSystemCPU(((int)maxAge * emit) + 1000, maxAge, ParticleSystemPreSetType.FIRE, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            ps.AddPermanentAction(new SourceAction(GraphicsDevice, Content, source, null, pSize, emit, timeStep));
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new AgeAction(GraphicsDevice, Content, timeStep));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new GravityAction(GraphicsDevice, Content, new Vector3(0, 0.01f, 0), timeStep));
            ps.AddPermanentAction(new RotateAction(GraphicsDevice, Content, -1, 1));
            ps.AddPermanentAction(new ScaleAction(GraphicsDevice, Content, 5));

            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();

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
            spriteBatch.End();

            ps.DrawParticles(cam.View, cam.Projection, cam.Up);
        }
    }
}
