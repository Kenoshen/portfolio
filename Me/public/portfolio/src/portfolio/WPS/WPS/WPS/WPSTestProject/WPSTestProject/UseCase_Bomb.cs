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
    class UseCase_Bomb : UseCase
    {
        ParticleSystem building_1;
        SourceAction buildingSource_1;
        Vector3 buildingPosition_1 = new Vector3(30, 0, 0);
        ParticleSystem building_2;
        SourceAction buildingSource_2;
        Vector3 buildingPosition_2 = new Vector3(30, 0, -20);
        int buildingPartiNum = 2000;
        float buildingPartiAge = 20;
        float buildingPartiSize = 1;
        float buildingHeight = 20;
        float buildingThickness = 5;

        OrbitPointAction forceWave;
        Vector3 forceWavePosition = new Vector3(0, 0, 0);
        float forceWaveMagnitude = 0;
        float forceWaveMagnitudeChange = 0.005f;
        float forceWaveRange = 1;
        float forceWaveRangeChange = 0.4f;
        float forceWaveEpsilon = 1;
        float forceWaveEpsilonChange = 15f;
        bool forceWaveStarted = false;

        GravityAction gravity;
        Vector3 gravityForce = new Vector3(0, -0.01f, 0);
        bool gravityAdded_1 = false;
        bool gravityAdded_2 = false;

        AgeAction aging;
        FadeAction fading;

        PlaneBounceAction ground;

        ParticleSystem explosion;
        SourceAction explosionSource;

        float timeCounter = 0;
        float timeStep = 0.5f;
        
        public UseCase_Bomb(bool active = false)
        {
            this.Active = active;
        }

        public override void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Debugger_WPS debugger, Effect floorEffect, Texture2D floorTexture, Quad floor)
        {
            base.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);

            helpMessage = "\nPress ENTER";


            building_1 = new ParticleSystem(buildingPartiNum, buildingPartiAge, ParticleSystemPreSetType.SMOKE, ParticleSystemVisability.OPAQUE, GraphicsDevice, Content);
            buildingSource_1 = new SourceAction(GraphicsDevice, Content, 
                new BoxDomain(buildingPosition_1 + (Vector3.Up * buildingHeight / 2), buildingThickness, buildingHeight, buildingThickness), 
                null, buildingPartiSize, buildingPartiNum, timeStep);
            building_1.AddActionForThisFrame(buildingSource_1);
            building_1.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));

            building_2 = new ParticleSystem(buildingPartiNum, buildingPartiAge, ParticleSystemPreSetType.SMOKE, ParticleSystemVisability.OPAQUE, GraphicsDevice, Content);
            buildingSource_2 = new SourceAction(GraphicsDevice, Content,
                new BoxDomain(buildingPosition_2 + (Vector3.Up * buildingHeight / 2), buildingThickness, buildingHeight, buildingThickness),
                null, buildingPartiSize, buildingPartiNum, timeStep);
            building_2.AddActionForThisFrame(buildingSource_2);
            building_2.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep));

            explosion = new ParticleSystem(10000, 100, ParticleSystemPreSetType.FIRE, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            explosion.AddPermanentAction(new MoveAction(GraphicsDevice, Content, timeStep / 3));
            explosion.AddPermanentAction(new PlaneBounceAction(GraphicsDevice, Content, Vector3.Down, Vector3.Up));
            explosion.AddPermanentAction(new AgeAction(GraphicsDevice, Content, timeStep));
            explosion.AddPermanentAction(new FadeAction(GraphicsDevice, Content));
            
            explosionSource = new SourceAction(GraphicsDevice, Content, new SphereDomain(forceWavePosition, 0.5f), null, 1, 5000);
            ground = new PlaneBounceAction(GraphicsDevice, Content, new Vector3(0, 0, 0), Vector3.Up, 0.1f, timeStep);
            aging = new AgeAction(GraphicsDevice, Content, timeStep);
            fading = new FadeAction(GraphicsDevice, Content);
            forceWave = new OrbitPointAction(GraphicsDevice, Content, forceWavePosition, forceWaveRange, forceWaveMagnitude, forceWaveEpsilon, timeStep);
            gravity = new GravityAction(GraphicsDevice, Content, gravityForce, timeStep); 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (keyboard.IsKeyBeingPressed(Keys.Space))
                UpdateCamera();
            else
                cam.Update();


            if (forceWaveStarted)
            {
                timeCounter += timeStep;

                forceWave.Magnitude = -1 / (forceWaveMagnitudeChange * timeCounter);
                forceWave.MaxRange += forceWaveRangeChange;
                forceWave.Epsilon += forceWaveEpsilonChange;

                if (Vector3.Distance(forceWavePosition, buildingPosition_1) < forceWave.MaxRange && !gravityAdded_1)
                {
                    gravityAdded_1 = true;
                    building_1.AddPermanentAction(gravity);
                    building_1.AddPermanentAction(ground);
                    building_1.AddPermanentAction(aging);
                    building_1.AddPermanentAction(fading);
                }

                if (Vector3.Distance(forceWavePosition, buildingPosition_2) < forceWave.MaxRange && !gravityAdded_2)
                {
                    gravityAdded_2 = true;
                    building_2.AddPermanentAction(gravity);
                    building_2.AddPermanentAction(ground);
                    building_2.AddPermanentAction(aging);
                    building_2.AddPermanentAction(fading);
                }
            }


            if (keyboard.IsKeyJustPressed(Keys.Enter) && !forceWaveStarted)
            {
                forceWaveStarted = true;
                timeCounter = 25;
                helpMessage = "\nPress R to reset";
                building_1.AddPermanentAction(forceWave);
                building_2.AddPermanentAction(forceWave);
                explosion.AddActionForThisFrame(explosionSource);
                explosion.AddPermanentAction(forceWave);
            }

            if (keyboard.IsKeyJustPressed(Keys.R))
            {
                building_1.RemovePermanentAction(forceWave);
                building_1.RemovePermanentAction(gravity);
                building_1.RemovePermanentAction(ground);
                building_1.RemovePermanentAction(aging);
                building_1.RemovePermanentAction(fading);
                building_1.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
                building_1.AddActionForThisFrame(buildingSource_1);


                building_2.RemovePermanentAction(forceWave);
                building_2.RemovePermanentAction(gravity);
                building_2.RemovePermanentAction(ground);
                building_2.RemovePermanentAction(aging);
                building_2.RemovePermanentAction(fading);
                building_2.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
                building_2.AddActionForThisFrame(buildingSource_2);

                explosion.RemovePermanentAction(forceWave);
                explosion.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));

                forceWave = new OrbitPointAction(GraphicsDevice, Content, forceWavePosition, forceWaveRange, forceWaveMagnitude, forceWaveEpsilon, timeStep);
                forceWaveStarted = false;
                gravityAdded_1 = false;
                gravityAdded_2 = false;
                helpMessage = "\nPress ENTER";
                timeCounter = 0;
            }

            if (keyboard.IsKeyJustPressed(Keys.H))
                showHelp = !showHelp;

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            building_1.ApplyActions();
            building_2.ApplyActions();
            explosion.ApplyActions();

            GraphicsDevice.Clear(Color.Black);

            DrawFloor();
            spriteBatch.Begin();
            DrawDebugger_FPS(gameTime, Vector2.UnitY * 5);
            DrawHelpMessage(Vector2.UnitY * 30);
            spriteBatch.End();

            building_1.DrawParticles(cam.View, cam.Projection, cam.Up);
            building_2.DrawParticles(cam.View, cam.Projection, cam.Up);
            explosion.DrawParticles(cam.View, cam.Projection, cam.Up);
        }
    }
}
