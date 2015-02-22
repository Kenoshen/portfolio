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
using fbDeprofiler;
using WPS;
using WPS.Camera;
using WPS.CustomModel;
using WPS.Input;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;


namespace ParticleGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //const float PLAYER_MOVE_SPEED = 0f;
        float PLAYER_MOVE_SPEED = 0.065f;
        const float PLAYER_MAX_SPEED = 0.13f;
        const float PLAYER_JUMP_FORCE = 3.8f;
        const float PLAYER_WIDTH = 2f;
        const float PLAYER_HEIGHT = 2f;
        const float PLAYER_SENSOR_DIFF = 0.1f;
        const float PLAYER_KILL_PLANE = -30f;
        const float GRAV_RANGE = 10f;
        const float GRAV_MAGNI = 0.5f;
        const float GRAV_EPSIL = 0.3f;
        const float SPAWN_RANGE_FROM_PLAYER = 35f;
        float END_OF_WORLD_X = -20f;
        float END_OF_WORLD_SPEED = 0.13f;
        float END_OF_WORLD_INC_SPEED = 0.00001f;
        const float END_OF_WORLD_INC_INC_SPEED = 0.00000005f;
        const float END_OF_WORLD_MAX_INC_SPEED = 0.01f;
        const float END_OF_WORLD_MAX_SPEED = 0.395f;
        const float END_OF_WORLD_RUBBER = 50f;
        const float WORLD_GRAV = 1f;
        float CAM_DIST = 30f;
        float CAM_TARGET_DIST = 1f;
        const float CAM_X_DIFF = 2f;
        const float CAM_MAX_DIST = 50f;
        const float CAM_MAX_HEIGHT = 20f;
        const int MAX_PARTICLES = 30000;
        const float WORLD_STEP_TIME = 0.08f;
        const float SIGN_SIZE = 0.2f;
        const float SCENERY_SIZE = 0.3f;

        #region Starting values
        const float STARTING_CAM_DIST = 30f;
        const float STARTING_PLAYER_X = -10f;
        const float STARTING_PLAYER_Y = 5f;
        #endregion

        #region Vars
        GameState gameState;
        ParticleSystem ps;
        Texture2D blockTexture;
        Effect sourceEffect;
        Quad fullScreenQuad;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        FreeCamera cam;
        CMouse mouse;
        CKeyboard keyboard;
        Random rnd;
        bool inverted = false;
        Effect invertEffect;
        RenderTarget2D invertTarget;
        Quad invertQuad;
        AnimationManager animation;

        Body playerBody;
        Billboard playerBillboard;
        Texture2D[] playerTexture;
        PlayerState currState;
        Fixture sensorFixture;
        Vector2 sensorPoint;
        Vector2 sensorNormal;
        float sensorFraction;
        bool midFootSensor = false;
        bool topRightSensor = false;
        bool midRightSensor = false;
        bool botRightSensor = false;
        bool topLeftSensor = false;
        bool midLeftSensor = false;
        bool botLeftSensor = false;
        Vector2 midFootSensorRay = Vector2.Zero;
        Vector2 topRightSensorRay = Vector2.Zero;
        Vector2 midRightSensorRay = Vector2.Zero;
        Vector2 botRightSensorRay =  Vector2.Zero;
        Vector2 topLeftSensorRay = Vector2.Zero;
        Vector2 midLeftSensorRay = Vector2.Zero;
        Vector2 botLeftSensorRay = Vector2.Zero;
        float sensorDiagDist;
        float sensorHoriDist;
        float sensorVertDist;

        List<Body> platformBodies;
        int spawnIndex = -10;
        int platformIndex = -20;
        float randomPlatformStarter = 0;
        List<List<Vector4>> platformVectors = new List<List<Vector4>>();
        int counter = 0;
        int minCounter = 33;
        int maxCounter = 33;
        int limit = 0;
        bool firstTime = true;
        Ledge ledge1;
        Ledge ledge2;
        Ledge ledge3;
        List<Vector4> goodsign;
        List<Vector4> oksign;
        List<Vector4> badsign;

        OrbitAxisAction pusher1;
        OrbitAxisAction pusher2;
        OrbitAxisAction pusher3;
        CustomPlaneGravityAndAgeAction psAger;
        List<List<Vector4>> particleVectors = new List<List<Vector4>>();
        float endoftheworldX = -20;
        float pusher1_Y = 0;
        float pusher2_Y = 0;
        float pusher3_Y = 0;

        MenuScreen menu;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fbDeprofiler.DeProfiler.Run();
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            sensorDiagDist = (float)Math.Sqrt((PLAYER_HEIGHT * PLAYER_HEIGHT) + (PLAYER_WIDTH / 2) * (PLAYER_WIDTH / 2)) + PLAYER_SENSOR_DIFF;
            sensorHoriDist = (PLAYER_WIDTH) + PLAYER_SENSOR_DIFF;
            sensorVertDist = (PLAYER_HEIGHT * 1.5f) + PLAYER_SENSOR_DIFF;

            midFootSensorRay =  new Vector2(0, -PLAYER_HEIGHT + PLAYER_SENSOR_DIFF);
            midRightSensorRay = new Vector2(PLAYER_WIDTH + PLAYER_SENSOR_DIFF, 0);
            midLeftSensorRay =  new Vector2(-(PLAYER_WIDTH + PLAYER_SENSOR_DIFF), 0);
            botRightSensorRay = new Vector2(PLAYER_WIDTH + PLAYER_SENSOR_DIFF, -(PLAYER_HEIGHT + PLAYER_SENSOR_DIFF));
            topRightSensorRay = new Vector2(PLAYER_WIDTH + PLAYER_SENSOR_DIFF, PLAYER_HEIGHT + PLAYER_SENSOR_DIFF);
            botLeftSensorRay =  new Vector2(-(PLAYER_WIDTH + PLAYER_SENSOR_DIFF), -(PLAYER_HEIGHT + PLAYER_SENSOR_DIFF));
            topLeftSensorRay =  new Vector2(-(PLAYER_WIDTH + PLAYER_SENSOR_DIFF), PLAYER_HEIGHT + PLAYER_SENSOR_DIFF);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            animation = new AnimationManager(Content);
            gameState = GameState.MENU;
            rnd = new Random();
            world = new World(new Vector2(0, -WORLD_GRAV));
            cam = new FreeCamera(new Vector3(0, 0, CAM_DIST), 0, 0, GraphicsDevice);
            cam.Position = new Vector3(0, 0, CAM_DIST);
            goodsign = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/Signs/good"), 0, 0, 0, SIGN_SIZE);
            oksign = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/Signs/ok"), 0, 0, 0, SIGN_SIZE);
            badsign = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/Signs/bad"), 0, 0, 0, SIGN_SIZE);
            blockTexture = Content.Load<Texture2D>("Art/block");
            ps = new ParticleSystem(MAX_PARTICLES, 100, blockTexture, ParticleSystemVisability.OPAQUE, GraphicsDevice, Content);
            psAger = new CustomPlaneGravityAndAgeAction(GraphicsDevice, Content, new Vector3(endoftheworldX, 0, 0), new Vector3(1, -0.5f, 0), 0.1f, 1, 1.5f);
            pusher1 = new OrbitAxisAction(GraphicsDevice, Content, new Vector3(endoftheworldX, 0, 0), new Vector3(endoftheworldX, 0, 1), GRAV_RANGE / 2, -GRAV_MAGNI * 1, 1);
            pusher2 = new OrbitAxisAction(GraphicsDevice, Content, new Vector3(endoftheworldX, 0, 0), new Vector3(endoftheworldX, 0, 1), GRAV_RANGE / 2, -GRAV_MAGNI * 1, 1);
            pusher3 = new OrbitAxisAction(GraphicsDevice, Content, new Vector3(endoftheworldX, 0, 0), new Vector3(endoftheworldX, 0, 1), GRAV_RANGE / 2, -GRAV_MAGNI * 1, 1);
            InitializeParticleSystem();
            sourceEffect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "Source");
            fullScreenQuad = new Quad(GraphicsDevice);
            mouse = new CMouse();
            keyboard = new CKeyboard();

            InitializePlayer();

            platformBodies = new List<Body>();
            InitializePlatforms();

            invertEffect = Content.Load<Effect>("Effects/Invert");
            invertTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            invertQuad = new Quad(GraphicsDevice);

            menu = new MenuScreen(mouse, cam, GraphicsDevice, Content);
            menu.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouse.BeginUpdate();
            keyboard.BeginUpdate();
            if (keyboard.IsKeyJustPressed(Keys.Escape))
                this.Exit();
            switch (gameState)
            {
                case GameState.PLAY:
                    if (menu.State != MenuScreenState.PLAY_OFF)
                    {
                        world.Step(WORLD_STEP_TIME);

                        UpdateParticleSystem();
                        UpdatePlatforms();
                        RemovePlatforms();
                        UpdatePlayer();
                        UpdateCamera();

                        if (playerBody.Position.Y < PLAYER_KILL_PLANE)
                        {
                            gameState = GameState.MENU;
                            Vector3 cP = cam.Position;
                            cP.Z = STARTING_CAM_DIST;
                            cam.Position = cP;
                            menu.TransitionOn();

                            ps.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
                        }
                    }
                    else
                    {
                        if (cam.Position.Y < playerBody.Position.Y)
                            menu.State = MenuScreenState.OFF;
                        else
                            cam.Move(Vector3.Down * 0.5f);
                        cam.Update();
                    }
                    break;

                case GameState.MENU:
                    menu.Update();
                    cam.Update();

                    if (menu.State == MenuScreenState.PLAY_OFF)
                    {
                        gameState = GameState.PLAY;

                        playerBody.Position = new Vector2(STARTING_PLAYER_X, STARTING_PLAYER_Y);
                        playerBody.LinearVelocity = Vector2.Zero;

                        for (int i = 0; i < platformBodies.Count; i++)
                        {
                            platformBodies[i].Dispose();
                            platformBodies.RemoveAt(i);
                            i--;
                        }

                        ledge1 = new Ledge(playerBody);
                        ledge2 = new Ledge(playerBody);
                        ledge3 = new Ledge(playerBody);

                        spawnIndex = -10;
                        platformIndex = -20;
                        randomPlatformStarter = 0;
                        counter = 0;

                        pusher1_Y = 0;
                        pusher2_Y = 0;
                        pusher3_Y = 0;
                        endoftheworldX = -40f;

                        CAM_DIST = STARTING_CAM_DIST;
                        CAM_TARGET_DIST = STARTING_CAM_DIST;

                        END_OF_WORLD_X = -40f;
                        END_OF_WORLD_SPEED = 0.13f;
                        END_OF_WORLD_INC_SPEED = 0.00001f;

                        PLAYER_MOVE_SPEED = 0.065f;
                        
                        randomPlatformStarter = rnd.Next(100, 100000);
                        SpawnPlatformsInWave(30);

                        UpdateParticleSystem();
                    }
                    break;
            }

            if (keyboard.IsKeyJustPressed(Keys.I))
                inverted = !inverted;

            mouse.EndUpdate();
            keyboard.EndUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: update particles
            switch(gameState)
            {
                case GameState.PLAY:
                    ps.ApplyActions();
                    break;

                case GameState.MENU:
                    menu.ApplyParticleActions();
                    break;
            }
            GraphicsDevice.Clear(Color.White);

            GraphicsDevice.SetRenderTarget(invertTarget);
            GraphicsDevice.Clear(Color.White);

            // TODO: draw other stuff
            switch (gameState)
            {
                case GameState.PLAY:
                    GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    playerBillboard.DrawBillboard(animation.GetCurrentTexture(), cam.View, cam.Projection, cam.Up);
                    GraphicsDevice.BlendState = BlendState.Opaque;
                    ps.DrawParticles(cam.View, cam.Projection, cam.Up);

                    animation.Update();
                    break;

                case GameState.MENU:
                    menu.Draw(cam.View, cam.Projection, cam.Up);
                    break;
            }

            if (inverted)
            {
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);

                invertEffect.Parameters["ScreenTexture"].SetValue(invertTarget);
                invertEffect.Parameters["Inverted"].SetValue(1);
                //invertQuad.RenderQuad(invertEffect,
                //    new Vector3(-invertTarget.Width, 0, 1),
                //    new Vector3(0, 0, 1),
                //    new Vector3(0, invertTarget.Height, 1),
                //    new Vector3(-invertTarget.Width, invertTarget.Height, 1));

                invertQuad.RenderFullScreenQuad(invertEffect, 150);
            }
            else
            {
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);

                invertEffect.Parameters["ScreenTexture"].SetValue(invertTarget);
                invertEffect.Parameters["Inverted"].SetValue(0);
                //invertQuad.RenderQuad(invertEffect,
                //    new Vector3(-invertTarget.Width, 0, 1),
                //    new Vector3(0, 0, 1),
                //    new Vector3(0, invertTarget.Height, 1),
                //    new Vector3(-invertTarget.Width, invertTarget.Height, 1));

                invertQuad.RenderFullScreenQuad(invertEffect, 150);
            }

            //GraphicsDevice.SetRenderTarget(null);
            //GraphicsDevice.Clear(Color.Black);

            //if (inverted)
            //    invertEffect.Parameters["Inverted"].SetValue(1);
            //else
            //    invertEffect.Parameters["Inverted"].SetValue(0);

            //invertEffect.Parameters["ScreenTexture"].SetValue(invertTarget);

            //invertQuad.RenderFullScreenQuad(invertEffect, 150);

            base.Draw(gameTime);
        }

        #region Camera
        void UpdateCamera()
        {
            if (keyboard.IsKeyBeingPressed(Keys.OemPlus))
                cam.Position = cam.Position - new Vector3(0, 0, 1f);
            if (keyboard.IsKeyBeingPressed(Keys.OemMinus))
                cam.Position = cam.Position + new Vector3(0, 0, 1f);

            if (CAM_TARGET_DIST != CAM_DIST)
            {
                CAM_DIST += (CAM_TARGET_DIST - CAM_DIST) * 0.01f;
            }

            if (CAM_MAX_DIST > (CAM_DIST - 5 + (playerBody.Position.Y) / 2) &&
                CAM_MAX_HEIGHT > (playerBody.Position.Y / 4 + 7))
                cam.Position = new Vector3(playerBody.Position.X - CAM_X_DIFF, playerBody.Position.Y / 4 + 7, CAM_DIST - 5 + (playerBody.Position.Y) / 2);
            else
                cam.Position = new Vector3(playerBody.Position.X - CAM_X_DIFF, cam.Position.Y, cam.Position.Z);

            cam.Update();
        }
        #endregion

        #region Player
        void InitializePlayer()
        {
            playerBody = BodyFactory.CreateCircle(world, (PLAYER_WIDTH + PLAYER_HEIGHT) / 4f, 0f);
            playerBody.Position = Vector2.UnitY * 5 - Vector2.UnitX * 10;
            playerBody.FixedRotation = true;
            playerBody.Friction = 0;
            playerBody.LinearDamping = 0.3f;
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.CollisionCategories = Category.Cat1;

            playerBillboard = new Billboard(new Vector3(playerBody.Position, 0), PLAYER_WIDTH, PLAYER_HEIGHT, GraphicsDevice, Content);

            currState = PlayerState.Running;
            playerTexture = new Texture2D[(int)PlayerState.COUNT];
            playerTexture[(int)PlayerState.Running] = Content.Load<Texture2D>("Guy/guy_running_1");
            playerTexture[(int)PlayerState.Dropping] = Content.Load<Texture2D>("Art/drop");
            playerTexture[(int)PlayerState.Jumping] = Content.Load<Texture2D>("Guy/guy_jumpingRight_1");
            playerTexture[(int)PlayerState.LeftWallGrab] = Content.Load<Texture2D>("Guy/guy_wallgrabLeft_1");
            playerTexture[(int)PlayerState.RightWallGrab] = Content.Load<Texture2D>("Guy/guy_wallgrabRight_1");
            playerTexture[(int)PlayerState.Falling] = Content.Load<Texture2D>("Guy/guy_fallingRight_1");
            playerTexture[(int)PlayerState.Standing] = Content.Load<Texture2D>("Art/drop");
            playerTexture[(int)PlayerState.DoubleJumpRight] = Content.Load<Texture2D>("Guy/guy_jumpingRight_1");
            playerTexture[(int)PlayerState.DoubleJumpLeft] = Content.Load<Texture2D>("Guy/guy_jumpingLeft_1");
            playerTexture[(int)PlayerState.DoubleFall] = Content.Load<Texture2D>("Guy/guy_fallingRight_1");

            ledge1 = new Ledge(playerBody);
            ledge2 = new Ledge(playerBody);
            ledge3 = new Ledge(playerBody);

            UpdatePlayer();

            currState = PlayerState.Running;
        }

        void UpdatePlayer()
        {
            CastRays();
            CastRightRay();
            CastLeftRay();

            Vector2 playerVel = playerBody.LinearVelocity;

            if (currState == PlayerState.Running || currState == PlayerState.Jumping || currState == PlayerState.Falling ||
                currState == PlayerState.DoubleJumpRight || currState == PlayerState.DoubleJumpLeft || currState == PlayerState.DoubleFall)
                playerBody.ApplyLinearImpulse(new Vector2(PLAYER_MOVE_SPEED, 0));

            if (playerVel.Y < 0)
            {
                switch (currState)
                {
                    case PlayerState.Jumping:
                        currState = PlayerState.Falling;
                        animation.TransitionToFalling();
                        break;

                    case PlayerState.DoubleJumpRight:
                        currState = PlayerState.DoubleFall;
                        animation.TransitionToFalling();
                        break;

                    case PlayerState.DoubleJumpLeft:
                        currState = PlayerState.DoubleFall;
                        animation.TransitionToFalling();
                        break;

                    //case PlayerState.LeftWallGrab:
                    //    currState = PlayerState.Dropping;
                    //    break;

                    //case PlayerState.RightWallGrab:
                    //    currState = PlayerState.Dropping;
                    //    break;

                    default:
                        break;
                }
            }

            if (playerVel.X >= 0)
            {
                if (currState == PlayerState.DoubleJumpLeft)
                {
                    currState = PlayerState.DoubleJumpRight;
                    animation.TransitionToJumping();
                }
            }

            if (keyboard.IsKeyJustPressed(Keys.Up) || mouse.LeftJustPressed())
            {
                if (currState == PlayerState.Running || currState == PlayerState.Standing)
                {
                    currState = PlayerState.Jumping;
                    playerBody.ApplyLinearImpulse(new Vector2(0, PLAYER_JUMP_FORCE));
                    animation.TransitionToJumping();
                }
                else if (currState == PlayerState.Falling || currState == PlayerState.Jumping ||
                    currState == PlayerState.LeftWallGrab || currState == PlayerState.RightWallGrab)
                {
                    switch (currState)
                    {
                        case PlayerState.LeftWallGrab:
                            playerBody.ApplyLinearImpulse(new Vector2(PLAYER_MOVE_SPEED * 50, PLAYER_JUMP_FORCE * 0.75f));
                            currState = PlayerState.DoubleJumpRight;
                            animation.TransitionToJumping();
                            break;

                        case PlayerState.RightWallGrab:
                            playerBody.ApplyLinearImpulse(new Vector2(-PLAYER_MOVE_SPEED * 15, PLAYER_JUMP_FORCE * 2f));
                            currState = PlayerState.DoubleJumpLeft;
                            animation.TransitionToJumping();
                            break;

                        case PlayerState.Falling:
                            playerBody.ApplyLinearImpulse(new Vector2(0, PLAYER_JUMP_FORCE));
                            currState = PlayerState.DoubleJumpRight;
                            animation.TransitionToJumping();
                            break;

                        case PlayerState.Jumping:
                            playerBody.LinearVelocity = new Vector2(playerBody.LinearVelocity.X, 0);
                            playerBody.ApplyLinearImpulse(new Vector2(0, PLAYER_JUMP_FORCE));
                            currState = PlayerState.DoubleJumpRight;
                            animation.TransitionToJumping();
                            break;
                    }
                }
            }
            else if (keyboard.IsKeyJustReleased(Keys.Up) || mouse.LeftJustReleased())
            {
                if (currState == PlayerState.Jumping || currState == PlayerState.DoubleJumpLeft || currState == PlayerState.DoubleJumpRight)
                {
                    playerBody.ApplyLinearImpulse(new Vector2(0, -playerBody.LinearVelocity.Y * 0.5f));
                }
            }

            playerBillboard.SetVertexCenter(new Vector3(playerBody.Position, 0));
            playerBody.Awake = true;
        }

        public void CastRays()
        {
            midFootSensor = false;
            midRightSensor = false;
            botRightSensor = false;
            topRightSensor = false;
            midLeftSensor = false;
            botLeftSensor = false;
            topLeftSensor = false;

            // midFootSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + midFootSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorVertDist)
                    midFootSensor = true;
            }
            NullOutSensor();

            // midRightSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + midRightSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorHoriDist)
                    midRightSensor = true;
            }
            NullOutSensor();

            // midLeftSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + midLeftSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorHoriDist)
                    midLeftSensor = true;
            }
            NullOutSensor();

            // botRightSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + botRightSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorDiagDist)
                    botRightSensor = true;
            }
            NullOutSensor();

            // topRightSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + topRightSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorDiagDist)
                    topRightSensor = true;
            }
            NullOutSensor();

            // botLeftSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + botLeftSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorDiagDist)
                    botLeftSensor = true;
            }
            NullOutSensor();

            // topLeftSensor
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + topLeftSensorRay);
            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < sensorDiagDist)
                    topLeftSensor = true;
            }
            NullOutSensor();

            if ((botLeftSensor && midFootSensor) || (midFootSensor && botRightSensor))
            //if (botLeftSensor || botRightSensor || midFootSensor)
            {
                if (currState == PlayerState.Dropping || currState == PlayerState.Falling || currState == PlayerState.DoubleFall)
                {
                    currState = PlayerState.Running;
                    animation.TransitionToRunning();
                }
                else if (currState == PlayerState.RightWallGrab || currState == PlayerState.LeftWallGrab)
                    currState = PlayerState.Standing;
            }
            else if ((botRightSensor && midRightSensor) || (midRightSensor && topRightSensor))
            {
                if (currState == PlayerState.Jumping || currState == PlayerState.DoubleJumpRight)
                {
                    //player_Body.LinearVelocity = new Vector2(0, player_Body.LinearVelocity.Y);
                    currState = PlayerState.RightWallGrab;
                    animation.TransitionToWallGrab();
                }
                else if (currState == PlayerState.Falling || currState == PlayerState.DoubleFall)
                {
                    playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                    currState = PlayerState.RightWallGrab;
                    animation.TransitionToWallGrab();
                }
                else if (currState == PlayerState.Running)
                {
                    playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                    currState = PlayerState.Standing;
                }
            }
            else if ((botLeftSensor && midLeftSensor) || (midLeftSensor && topLeftSensor))
            {
                if (currState == PlayerState.Jumping || currState == PlayerState.DoubleJumpLeft)
                {
                    //player_Body.LinearVelocity = new Vector2(0, player_Body.LinearVelocity.Y);
                    currState = PlayerState.LeftWallGrab;
                }
                else if (currState == PlayerState.Falling || currState == PlayerState.DoubleFall)
                {
                    playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                    currState = PlayerState.LeftWallGrab;
                }
                else if (currState == PlayerState.Running)
                {
                    playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                    currState = PlayerState.Standing;
                }
            }
            else
            {
                switch (currState)
                {
                    case PlayerState.Running:
                        currState = PlayerState.Falling;
                        animation.TransitionToFalling();
                        break;

                    case PlayerState.LeftWallGrab:
                        currState = PlayerState.Falling;
                        animation.TransitionToFalling();
                        break;

                    case PlayerState.RightWallGrab:
                        currState = PlayerState.Falling;
                        animation.TransitionToFalling();
                        break;

                }
            }
        }

        public void CastRightRay()
        {
            world.RayCast(RayTest, playerBody.Position, playerBody.Position + Vector2.UnitX);

            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < PLAYER_WIDTH + PLAYER_SENSOR_DIFF)
                {
                    if (currState == PlayerState.Jumping || currState == PlayerState.DoubleJumpRight)
                    {
                        //player_Body.LinearVelocity = new Vector2(0, player_Body.LinearVelocity.Y);
                        currState = PlayerState.RightWallGrab;
                    }
                    else if (currState == PlayerState.Falling || currState == PlayerState.DoubleFall)
                    {
                        playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                        currState = PlayerState.RightWallGrab;
                    }
                    else if (currState == PlayerState.Running)
                    {
                        playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                        currState = PlayerState.Standing;
                    }
                }
            }

            sensorFixture = null;
            sensorPoint = Vector2.Zero;
            sensorNormal = Vector2.Zero;
            sensorFraction = 0;
        }

        public void CastLeftRay()
        {
            world.RayCast(RayTest, playerBody.Position, playerBody.Position - Vector2.UnitX);

            if (sensorFixture != null)
            {
                if (sensorFixture.CollisionCategories == Category.Cat11 &&
                    Vector2.Distance(playerBody.Position, sensorPoint) < PLAYER_WIDTH + PLAYER_SENSOR_DIFF)
                {
                    if (currState == PlayerState.Jumping || currState == PlayerState.DoubleJumpLeft)
                    {
                        //player_Body.LinearVelocity = new Vector2(0, player_Body.LinearVelocity.Y);
                        currState = PlayerState.LeftWallGrab;
                    }
                    else if (currState == PlayerState.Falling || currState == PlayerState.DoubleFall)
                    {
                        playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                        currState = PlayerState.LeftWallGrab;
                    }
                    else if (currState == PlayerState.Running)
                    {
                        playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                        currState = PlayerState.Standing;
                    }
                }
            }

            sensorFixture = null;
            sensorPoint = Vector2.Zero;
            sensorNormal = Vector2.Zero;
            sensorFraction = 0;
        }

        public float RayTest(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            sensorFixture = fixture;
            sensorPoint = point;
            sensorNormal = normal;
            sensorFraction = fraction;

            if (fixture != null)
                return fraction;
            return -1;
        }

        public void NullOutSensor()
        {
            sensorFixture = null;
            sensorPoint = Vector2.Zero;
            sensorNormal = Vector2.Zero;
            sensorFraction = 0;
        }
        #endregion

        #region Particle System
        void InitializeParticleSystem()
        {
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content));
            ps.AddPermanentAction(psAger);
            ps.AddPermanentAction(pusher1);
            ps.AddPermanentAction(pusher2);
            ps.AddPermanentAction(pusher3);
            ps.AddPermanentAction(new RotateAction(GraphicsDevice, Content, -2, 2));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));

            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/brush_1"), 0, 0, -20, SCENERY_SIZE));
            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/grass_1"), 0, 0, -20, SCENERY_SIZE));
            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/tree_1"), 0, 0, -40, SCENERY_SIZE));
            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/cow_1"), 0, 0, -10, SCENERY_SIZE));
            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/bird_1"), 0, 50, -30, SCENERY_SIZE));
            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/hill_1"), 0, 0, -60, SCENERY_SIZE));
            particleVectors.Add(TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("SceneObjects/cloud_1"), 0, 100, -150, SCENERY_SIZE));
        }

        void UpdateParticleSystem()
        {
            if (END_OF_WORLD_INC_SPEED + END_OF_WORLD_INC_INC_SPEED <= END_OF_WORLD_MAX_INC_SPEED)
                END_OF_WORLD_INC_SPEED += END_OF_WORLD_INC_INC_SPEED;
            if (END_OF_WORLD_SPEED + END_OF_WORLD_INC_SPEED <= END_OF_WORLD_MAX_SPEED)
                END_OF_WORLD_SPEED += END_OF_WORLD_INC_SPEED;
            END_OF_WORLD_X += END_OF_WORLD_SPEED;
            if (playerBody.Position.X - END_OF_WORLD_X >= END_OF_WORLD_RUBBER)
                END_OF_WORLD_X = playerBody.Position.X - END_OF_WORLD_RUBBER;
            endoftheworldX = END_OF_WORLD_X;

            pusher1_Y = (float)Math.Sin(endoftheworldX) * 50;
            pusher2_Y = (float)Math.Sin(endoftheworldX) * 50;
            pusher3_Y = (float)Math.Sin(endoftheworldX) * 50;

            pusher1.StartPoint.X = endoftheworldX;
            pusher1.EndPoint.X = endoftheworldX;
            pusher1.StartPoint.Y = pusher3_Y;
            pusher1.EndPoint.Y = pusher2_Y;

            pusher2.StartPoint.X = endoftheworldX;
            pusher2.EndPoint.X = endoftheworldX;
            pusher2.StartPoint.Y = pusher1_Y;
            pusher2.EndPoint.Y = pusher3_Y;

            pusher3.StartPoint.X = endoftheworldX;
            pusher3.EndPoint.X = endoftheworldX;
            pusher3.StartPoint.Y = pusher2_Y;
            pusher3.EndPoint.Y = pusher1_Y;

            psAger.PlanePoint.X = endoftheworldX + 10;
            
            if (spawnIndex <= platformIndex)
            {
                //if (platformIndex % rnd.Next(10, 100) == 0)
                //    SpawnTree();

                //if (platformIndex % rnd.Next(10, 100) == 0)
                //    SpawnGrass();

                //if (platformIndex % rnd.Next(10, 100) == 0)
                //    SpawnHill();

                //if (platformIndex % rnd.Next(10, 100) == 0)
                //    SpawnCloud();

                if (platformIndex % 10 == 0)
                {
                    int rndIndex = 0;
                    if (platformIndex % 100 == 0)
                        rndIndex = rnd.Next(0, particleVectors.Count);
                    else if (platformIndex % 50 == 0)
                        rndIndex = rnd.Next(0, particleVectors.Count - 2);
                    else
                        rndIndex = rnd.Next(0, particleVectors.Count - 4);

                    float rndZ = rnd.Next(0, 50);
                    ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, particleVectors[rndIndex],
                        new Vector3(platformIndex + cam.Position.Z + rndZ + (-particleVectors[rndIndex][0].Z), 0, -rndZ), Vector3.Zero, particleVectors[rndIndex][0].W));
                }
                spawnIndex++;
            }

            if (playerBody.Position.X < END_OF_WORLD_X)
            {
                ledge1.State = LedgeState.USED;
                ledge2.State = LedgeState.USED;
                ledge3.State = LedgeState.USED;
            }

            if (ledge1.RightEdge.X < END_OF_WORLD_X)
                ledge1.State = LedgeState.USED;
            if (ledge2.RightEdge.X < END_OF_WORLD_X)
                ledge2.State = LedgeState.USED;
            if (ledge3.RightEdge.X < END_OF_WORLD_X)
                ledge3.State = LedgeState.USED;

            Vector3 signVel = new Vector3(0, 0.1f, 0.15f);
            if (ledge1.State == LedgeState.HIT_GOOD || ledge2.State == LedgeState.HIT_GOOD || ledge3.State == LedgeState.HIT_GOOD)
            {
                ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, goodsign, new Vector3(playerBody.Position, 0), signVel, SIGN_SIZE));
                if (PLAYER_MOVE_SPEED + 0.01f < PLAYER_MAX_SPEED)
                {
                    PLAYER_MOVE_SPEED += 0.01f;
                    CAM_TARGET_DIST += 3f;
                }
            }
            else if (ledge1.State == LedgeState.HIT_OK || ledge2.State == LedgeState.HIT_OK || ledge3.State == LedgeState.HIT_OK)
            {
                ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, oksign, new Vector3(playerBody.Position, 0), signVel, SIGN_SIZE));
            }
            else if (ledge1.State == LedgeState.HIT_BAD || ledge2.State == LedgeState.HIT_BAD || ledge3.State == LedgeState.HIT_BAD)
            {
                ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, badsign, new Vector3(playerBody.Position, 0), signVel, SIGN_SIZE));
                PLAYER_MOVE_SPEED -= 0.0066f;
                CAM_TARGET_DIST -= 2f;
            }
        }

        #region blah
        /*
        void CreateNewParticles(List<Vector4> newPositions, float size = 1)
        {
            int numOfNewParticles = 0;
            RenderTarget2D tempTex = new RenderTarget2D(GraphicsDevice, ps.Position.CurrentTexture.Width, ps.Position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            Vector4[] tempVect = new Vector4[tempTex.Width * tempTex.Height];
            List<int> indexes = new List<int>();

            ps.Data.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < tempVect.Length; i++)
            {
                if (numOfNewParticles >= newPositions.Count)
                    break;
                
                if (tempVect[i].Z == -1)
                {
                    indexes.Add(i);
                    tempVect[i].X = 0; // alpha
                    tempVect[i].Y = size; // size
                    tempVect[i].Z = 0; // age
                    //tempVect[i].W = 0; // rotation

                    numOfNewParticles++;
                }
            }
            tempTex.SetData(tempVect);
            sourceEffect.Parameters["From"].SetValue(tempTex);
            ps.Data.DrawDataToTexture(sourceEffect, fullScreenQuad);


            tempTex = new RenderTarget2D(GraphicsDevice, ps.Position.CurrentTexture.Width, ps.Position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            ps.Position.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < indexes.Count; i++)
                tempVect[indexes[i]] = newPositions[i];
            tempTex.SetData(tempVect);
            sourceEffect.Parameters["From"].SetValue(tempTex);
            ps.Position.DrawDataToTexture(sourceEffect, fullScreenQuad);

            tempTex = new RenderTarget2D(GraphicsDevice, ps.Velocity.CurrentTexture.Width, ps.Velocity.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            ps.Velocity.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < indexes.Count; i++)
                tempVect[indexes[i]] = Vector4.Zero;
            tempTex.SetData(tempVect);
            sourceEffect.Parameters["From"].SetValue(tempTex);
            ps.Velocity.DrawDataToTexture(sourceEffect, fullScreenQuad);
        }

        void CreateNewParticles(List<Vector4> newPositions, float baseX, float baseY, float baseZ, float size = 1)
        {
            int numOfNewParticles = 0;
            RenderTarget2D tempTex = new RenderTarget2D(GraphicsDevice, ps.Position.CurrentTexture.Width, ps.Position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            Vector4[] tempVect = new Vector4[tempTex.Width * tempTex.Height];
            List<int> indexes = new List<int>();

            ps.Data.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < tempVect.Length; i++)
            {
                if (numOfNewParticles >= newPositions.Count)
                    break;

                if (tempVect[i].Z == -1)
                {
                    indexes.Add(i);
                    tempVect[i].X = 0; // alpha
                    tempVect[i].Y = size; // size
                    tempVect[i].Z = 0; // age
                    //tempVect[i].W = 0; // rotation

                    numOfNewParticles++;
                }
            }
            tempTex.SetData(tempVect);
            sourceEffect.Parameters["From"].SetValue(tempTex);
            ps.Data.DrawDataToTexture(sourceEffect, fullScreenQuad);


            tempTex = new RenderTarget2D(GraphicsDevice, ps.Position.CurrentTexture.Width, ps.Position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            ps.Position.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < indexes.Count; i++)
                tempVect[indexes[i]] = newPositions[i] + new Vector4(baseX, baseY, baseZ, 0);
            tempTex.SetData(tempVect);
            sourceEffect.Parameters["From"].SetValue(tempTex);
            ps.Position.DrawDataToTexture(sourceEffect, fullScreenQuad);

            tempTex = new RenderTarget2D(GraphicsDevice, ps.Velocity.CurrentTexture.Width, ps.Velocity.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            ps.Velocity.CurrentTexture.GetData(tempVect);
            for (int i = 0; i < indexes.Count; i++)
                tempVect[indexes[i]] = Vector4.Zero;
            tempTex.SetData(tempVect);
            sourceEffect.Parameters["From"].SetValue(tempTex);
            ps.Velocity.DrawDataToTexture(sourceEffect, fullScreenQuad);
        }
        */
        #endregion

        void SpawnTree()
        {
            List<Vector4> tree = new List<Vector4>();

            Vector4 basePos = new Vector4(playerBody.Position.X + 40, 0, rnd.Next(-30, 0), 0);

            tree.Add(basePos);
            tree.Add(basePos + Vector4.UnitY);
            tree.Add(basePos + Vector4.UnitY * 2);
            tree.Add(basePos + Vector4.UnitY * 3);
            tree.Add(basePos + Vector4.UnitY * 4);
            tree.Add(basePos + Vector4.UnitY * 5); 
            tree.Add(basePos + Vector4.UnitY * 6);
            tree.Add(basePos + Vector4.UnitY * 7);
            tree.Add(basePos + Vector4.UnitY * 8);
            tree.Add(basePos + Vector4.UnitY * 9);
            tree.Add(basePos + Vector4.UnitY * 10);

            tree.Add(basePos + Vector4.UnitY * 5 + Vector4.UnitX * 1);
            tree.Add(basePos + Vector4.UnitY * 5 + Vector4.UnitX * -1);

            
            tree.Add(basePos + Vector4.UnitY * 6 + Vector4.UnitX * 1);
            tree.Add(basePos + Vector4.UnitY * 6 + Vector4.UnitX * -1);
            tree.Add(basePos + Vector4.UnitY * 6 + Vector4.UnitX * 2);
            tree.Add(basePos + Vector4.UnitY * 6 + Vector4.UnitX * -2);

            tree.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * 1);
            tree.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * -1);
            tree.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * 2);
            tree.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * -2);
            tree.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * 3);
            tree.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * -3);

            tree.Add(basePos + Vector4.UnitY * 8 + Vector4.UnitX * 1);
            tree.Add(basePos + Vector4.UnitY * 8 + Vector4.UnitX * -1);
            tree.Add(basePos + Vector4.UnitY * 8 + Vector4.UnitX * 2);
            tree.Add(basePos + Vector4.UnitY * 8 + Vector4.UnitX * -2);
            
            tree.Add(basePos + Vector4.UnitY * 9 + Vector4.UnitX * 1);
            tree.Add(basePos + Vector4.UnitY * 9 + Vector4.UnitX * -1);
            
            tree.Add(basePos + Vector4.UnitY * 10);

            //CreateNewParticles(tree, 1);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, tree, Vector3.Zero, Vector3.Zero, 1));
        }

        void SpawnHill()
        {
            List<Vector4> hill = new List<Vector4>();

            Vector4 basePos = new Vector4(playerBody.Position.X + 80, 0, rnd.Next(-80, -40), 0);

            hill.Add(basePos);
            hill.Add(basePos + Vector4.UnitY * 1 + Vector4.UnitX * 1);
            hill.Add(basePos + Vector4.UnitY * 2 + Vector4.UnitX * 2);
            hill.Add(basePos + Vector4.UnitY * 3 + Vector4.UnitX * 3);
            hill.Add(basePos + Vector4.UnitY * 4 + Vector4.UnitX * 4);
            hill.Add(basePos + Vector4.UnitY * 5 + Vector4.UnitX * 5);
            hill.Add(basePos + Vector4.UnitY * 6 + Vector4.UnitX * 6);
            hill.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * 7);
            hill.Add(basePos + Vector4.UnitY * 8 + Vector4.UnitX * 8);
            hill.Add(basePos + Vector4.UnitY * 9 + Vector4.UnitX * 9);
            hill.Add(basePos + Vector4.UnitY * 8 + Vector4.UnitX * 10);
            hill.Add(basePos + Vector4.UnitY * 7 + Vector4.UnitX * 11);
            hill.Add(basePos + Vector4.UnitY * 6 + Vector4.UnitX * 12);
            hill.Add(basePos + Vector4.UnitY * 5 + Vector4.UnitX * 13);
            hill.Add(basePos + Vector4.UnitY * 4 + Vector4.UnitX * 14);
            hill.Add(basePos + Vector4.UnitY * 3 + Vector4.UnitX * 15);
            hill.Add(basePos + Vector4.UnitY * 2 + Vector4.UnitX * 16);
            hill.Add(basePos + Vector4.UnitY * 1 + Vector4.UnitX * 17);
            hill.Add(basePos + Vector4.UnitX * 18);

            //CreateNewParticles(hill, 1);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, hill, Vector3.Zero, Vector3.Zero, 1));
        }

        void SpawnGrass()
        {
            List<Vector4> grass = new List<Vector4>();

            Vector4 basePos = new Vector4(playerBody.Position.X + 50, 0, rnd.Next(-50, 0), 0);
            float grassSize = 0.2f;

            grass.Add(basePos + (Vector4.UnitX * 3 + Vector4.UnitY * 0) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 3 + Vector4.UnitY * 1) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 2 + Vector4.UnitY * 2) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 2 + Vector4.UnitY * 3) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 1 + Vector4.UnitY * 4) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 4 + Vector4.UnitY * 0) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 5 + Vector4.UnitY * 1) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 5 + Vector4.UnitY * 2) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 6 + Vector4.UnitY * 3) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 7 + Vector4.UnitY * 4) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 4 + Vector4.UnitY * 1) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 4 + Vector4.UnitY * 2) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 4 + Vector4.UnitY * 3) * grassSize);
            grass.Add(basePos + (Vector4.UnitX * 5 + Vector4.UnitY * 4) * grassSize);


            //CreateNewParticles(grass, grassSize);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, grass, Vector3.Zero, Vector3.Zero, grassSize));
        }

        void SpawnCloud()
        {
            //List<Vector4> cloud = new List<Vector4>();

            //Vector4 basePos = new Vector4(player_Body.Position.X + 200, rnd.Next(30, 50), rnd.Next(-200, -100), 0);
            Vector4 basePos = new Vector4(playerBody.Position.X + 50, 0, -rnd.Next(30, 50), 0);
            float grassSize = 1f;

            //cloud.Add(basePos + (Vector4.UnitX * 0 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 0 + Vector4.UnitY * 1) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 1 + Vector4.UnitY * 2) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 2 + Vector4.UnitY * 3) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 3 + Vector4.UnitY * 4) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 4 + Vector4.UnitY * 4) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 5 + Vector4.UnitY * 3) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 6 + Vector4.UnitY * 4) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 7 + Vector4.UnitY * 5) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 8 + Vector4.UnitY * 5) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 9 + Vector4.UnitY * 5) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 10 + Vector4.UnitY * 4) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 11 + Vector4.UnitY * 3) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 11 + Vector4.UnitY * 2) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 10 + Vector4.UnitY * 1) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 9 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 8 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 7 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 6 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 5 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 4 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 3 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 2 + Vector4.UnitY * 0) * grassSize);
            //cloud.Add(basePos + (Vector4.UnitX * 1 + Vector4.UnitY * 0) * grassSize);

            //CreateNewParticles(cloud, grassSize);

            Texture2D temp = new Texture2D(GraphicsDevice, 10, 10);
            temp = Content.Load<Texture2D>("Art/testload");
            //CreateNewParticles(TextureObjectBuilder.BuildSceneObjectFromTexture(temp, basePos.X, basePos.Y, basePos.Z), grassSize);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, TextureObjectBuilder.BuildSceneObjectFromTexture(temp, basePos.X, basePos.Y, basePos.Z), Vector3.Zero, Vector3.Zero, grassSize));
        }
        #endregion

        #region Platforms
        void InitializePlatforms()
        {
            randomPlatformStarter = rnd.Next(100, 100000);
            platformVectors.Add(TextureObjectBuilder.BuildPhysicalObjectFromTexture(Content.Load<Texture2D>("PhysicalObjects/platform0"), 0, 0, 0));
            platformVectors.Add(TextureObjectBuilder.BuildPhysicalObjectFromTexture(Content.Load<Texture2D>("PhysicalObjects/platform1"), 0, 0, 0));
            platformVectors.Add(TextureObjectBuilder.BuildPhysicalObjectFromTexture(Content.Load<Texture2D>("PhysicalObjects/platform2"), 0, 0, 0));
            platformVectors.Add(TextureObjectBuilder.BuildPhysicalObjectFromTexture(Content.Load<Texture2D>("PhysicalObjects/platform3"), 0, 0, 0));

            //Body tempBody;
            //List<Vector4> newPositions = new List<Vector4>();

            //for (; platformIndex < 10; platformIndex++)
            //{
            //    tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
            //    tempBody.Position = new Vector2(platformIndex, 0);
            //    tempBody.BodyType = BodyType.Static;
            //    tempBody.Friction = 0;
            //    tempBody.CollisionCategories = Category.Cat11;
            //    platformBodies.Add(tempBody);

            //    newPositions.Add(new Vector4(platformIndex, 0, 0, 0));
            //    newPositions.Add(new Vector4(platformIndex, -1, 0, 0));
            //    newPositions.Add(new Vector4(platformIndex, -1 * 2, 0, 0));
            //    newPositions.Add(new Vector4(platformIndex, -1 * 3, 0, 0));
            //    newPositions.Add(new Vector4(platformIndex, -1 * 4, 0, 0));
            //    newPositions.Add(new Vector4(platformIndex, -1 * 5, 0, 0));
            //}
            
            //CreateNewParticles(newPositions);
            //ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, Vector3.Zero, 1));

            //SpawnPlatformsInWave(20);
        }

        void RemovePlatforms()
        {
            for (int i = 0; i < platformBodies.Count; i++)
            {
                if (platformBodies[i].Position.X < endoftheworldX + 5)
                {
                    platformBodies[i].Dispose();
                    platformBodies.RemoveAt(i);
                    i--;
                }
            }
        }

        void UpdatePlatforms()
        {
            ledge1.Update();
            ledge2.Update();
            ledge3.Update();
            if (platformIndex - playerBody.Position.X <= SPAWN_RANGE_FROM_PLAYER)
            {
                //List<Vector4> newPositions = new List<Vector4>();

                //Body tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                //tempBody.Position = new Vector2(platformIndex, 0);
                //tempBody.BodyType = BodyType.Static;
                //tempBody.Friction = 0;
                //tempBody.CollisionCategories = Category.Cat11;
                //platformBodies.Add(tempBody);
                //newPositions.Add(new Vector4(tempBody.Position, 0, 0));

                //newPositions.Add(new Vector4(platformIndex, -1, 0, 0));
                //newPositions.Add(new Vector4(platformIndex, -1 * 2, 0, 0));
                //newPositions.Add(new Vector4(platformIndex, -1 * 3, 0, 0));
                //newPositions.Add(new Vector4(platformIndex, -1 * 4, 0, 0));
                //newPositions.Add(new Vector4(platformIndex, -1 * 5, 0, 0));

                ////CreateNewParticles(newPositions);
                //ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, Vector3.Zero, 1));

                platformIndex++;

                counter++;
                if (counter > limit)
                {
                    counter = 0;
                    limit = rnd.Next(minCounter, maxCounter);
                    //SpawnFromVectors(platformVectors[rnd.Next(0, platformVectors.Count)], platformIndex);
                    if (firstTime)
                    {
                        SpawnPlatformsInWave(20);
                        firstTime = false;
                    }
                    else
                        SpawnPlatformsInWave();
                }
            }
        }

        void SpawnPlatformsInWave(int width = 0)
        {
            float yValue = 1 + (2 * (float)Math.Sin(0.05f * (platformIndex + randomPlatformStarter)));
            int platformWidth = rnd.Next(15, 30);
            if (width != 0)
                platformWidth = width;
            List<Vector4> newPositions = new List<Vector4>();

            Vector2 leftEdge = Vector2.Zero;
            Vector2 rightEdge = Vector2.Zero;
            for (int y = -11; y < yValue; y++)
            {
                if (y + 1 >= yValue)
                {
                    leftEdge.X = platformIndex;
                    leftEdge.Y = y;
                    rightEdge.Y = y;
                }
                for (int x = 0; x < platformWidth; x++)
                {
                    if (y + 1 >= yValue || x == 0)
                    {
                        Body tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                        tempBody.Position = new Vector2(platformIndex + x, y + 1);
                        tempBody.BodyType = BodyType.Static;
                        tempBody.CollisionCategories = Category.Cat11;
                        tempBody.Friction = 0;

                        platformBodies.Add(tempBody);

                        if (x + 1 >= platformWidth)
                            rightEdge.X = platformIndex + x;
                    }

                    newPositions.Add(new Vector4(x, y + 1, 0, 0));
                }
            }
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, new Vector3(platformIndex, 0, 0), Vector3.Zero, 1));

            if (width == 0)
            {
                if (ledge1.State == LedgeState.USED)
                    ledge1.SetLedge(leftEdge, rightEdge);
                else if (ledge2.State == LedgeState.USED)
                    ledge2.SetLedge(leftEdge, rightEdge);
                else if (ledge3.State == LedgeState.USED)
                    ledge3.SetLedge(leftEdge, rightEdge);
            }
        }

        void SpawnWall()
        {
            List<Vector4> newPositions = new List<Vector4>();

            Body tempBody;

            int mxtp = 6;
            int xdif = 5;

            for (int i = 0; i < mxtp; i++)
            {
                tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                tempBody.Position = new Vector2(platformIndex, 1 * (i + 1));
                tempBody.BodyType = BodyType.Static;
                tempBody.Friction = 0;
                tempBody.CollisionCategories = Category.Cat11;
                platformBodies.Add(tempBody);

                newPositions.Add(new Vector4(platformIndex, 1 * (i + 1), 0, 0));


                tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                tempBody.Position = new Vector2(platformIndex - xdif, 1 * (i + mxtp));
                tempBody.BodyType = BodyType.Static;
                tempBody.Friction = 0;
                tempBody.CollisionCategories = Category.Cat11;
                platformBodies.Add(tempBody);

                newPositions.Add(new Vector4(platformIndex - xdif, 1 * (i + mxtp), 0, 0));

                newPositions.Add(new Vector4(platformIndex, -1 * i, 0, 0));
            }

            //CreateNewParticles(newPositions, 1);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, Vector3.Zero, Vector3.Zero, 1));
            platformIndex++;
        }

        void SpawnPlatform()
        {
            List<Vector4> newPositions = new List<Vector4>();

            Body tempBody;

            int mxtp = 10;
            int ydif = rnd.Next(3, 5);

            for (int i = 0; i < mxtp; i++)
            {
                tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                tempBody.Position = new Vector2(platformIndex + i, 1 * ydif);
                tempBody.BodyType = BodyType.Static;
                tempBody.Friction = 0;
                tempBody.CollisionCategories = Category.Cat11;
                platformBodies.Add(tempBody);

                newPositions.Add(new Vector4(platformIndex + i, 1 * ydif, 0, 0));

                newPositions.Add(new Vector4(platformIndex, -1 * i, 0, 0));
            }

            //CreateNewParticles(newPositions, 1);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, Vector3.Zero, Vector3.Zero, 1));
            platformIndex++;
        }

        void SpawnFromTexture(Texture2D texture, float baseX = 0, float baseY = 0, float baseZ = 0)
        {
            List<Vector4> newPositions = TextureObjectBuilder.BuildPhysicalObjectFromTexture(texture, baseX, baseY, baseZ);
            Body tempBody;

            for (int i = 0; i < newPositions.Count; i++)
            {
                if (newPositions[i].W == 1)
                {
                    tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                    tempBody.Position = new Vector2(newPositions[i].X, newPositions[i].Y);
                    tempBody.BodyType = BodyType.Static;
                    tempBody.Friction = 0;
                    tempBody.CollisionCategories = Category.Cat11;
                    platformBodies.Add(tempBody);
                }
            }

            //CreateNewParticles(newPositions);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, Vector3.Zero, Vector3.Zero, 1));
        }

        void SpawnFromVectors(List<Vector4> newPositions, float baseX = 0, float baseY = 0, float baseZ = 0)
        {
            Body tempBody;

            for (int i = 0; i < newPositions.Count; i++)
            {
                if (newPositions[i].W == 1)
                {
                    tempBody = BodyFactory.CreateRectangle(world, 1, 1, 1);
                    tempBody.Position = new Vector2(newPositions[i].X + baseX, newPositions[i].Y + baseY);
                    tempBody.BodyType = BodyType.Static;
                    tempBody.Friction = 0;
                    tempBody.CollisionCategories = Category.Cat11;
                    platformBodies.Add(tempBody);
                }
            }

            //CreateNewParticles(newPositions, baseX, baseY, baseZ);
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, newPositions, new Vector3(baseX, baseY, baseZ), Vector3.Zero, 1));
        }
        #endregion
    }

    public enum PlayerState
    {
        Running = 0,
        Dropping = 1,
        Jumping = 2,
        LeftWallGrab = 3,
        RightWallGrab = 4,
        Falling = 5,
        Standing = 6,
        DoubleJumpRight = 7,
        DoubleJumpLeft = 8,
        DoubleFall = 9,

        COUNT = 10,
    }

    public enum GameState
    {
        MENU,
        PLAY,
        OPTIONS,
        HIGHSCORE,
    }
}
