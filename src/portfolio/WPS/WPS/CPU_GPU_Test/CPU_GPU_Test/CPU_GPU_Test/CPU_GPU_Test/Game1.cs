using System;
using System.IO;
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
using WPS.Input;
using WPS.Camera;

namespace CPU_GPU_Test
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FreeCamera cam;

        int timer = 0;
        int thirtySeconds = 30000;
        int testIndex = 0;
        int[] testCases = new int[] { 100, 500, 1000, 5000, 10000, 50000, 100000 };
        bool nextPS = false;
        string fileOutput = "";
        int frameIndex = 0;
        float[] frameCounter = new float[25];
        float frameRate;
        bool warmUp = true;

        ParticleSystemCPU ps_CPU;
        ParticleSystem ps_GPU;
        
        AgeAction age;
        PointDomain pointDomain;
        RingDomain ringDomain;
        SourceAction source;
        GravityAction gravity;
        FadeAction fade;
        SquareBounceAction bounce;
        MoveAction move;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fbDeprofiler.DeProfiler.Run();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
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
            cam = new FreeCamera(new Vector3(0, 10, 25), 0, -0.4f, GraphicsDevice);
            InitializeActions();
            LoadNewParticleSystems(testCases[testIndex]);
        }
        
        void LoadNewParticleSystems(int numOfParticles)
        {
            ps_CPU = new ParticleSystemCPU(numOfParticles, 100, ParticleSystemPreSetType.EXPLOSION, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);
            ps_GPU = new ParticleSystem(numOfParticles, 100, ParticleSystemPreSetType.EXPLOSION, ParticleSystemVisability.ALPHA, GraphicsDevice, Content);

            ps_CPU.AddPermanentAction(age);
            ps_CPU.AddPermanentAction(source);
            ps_CPU.AddPermanentAction(gravity);
            ps_CPU.AddPermanentAction(fade);
            ps_CPU.AddPermanentAction(bounce);
            ps_CPU.AddPermanentAction(move);

            ps_GPU.AddPermanentAction(age);
            ps_GPU.AddPermanentAction(source);
            ps_GPU.AddPermanentAction(gravity);
            ps_GPU.AddPermanentAction(fade);
            ps_GPU.AddPermanentAction(bounce);
            ps_GPU.AddPermanentAction(move);

            int sr = (int)Math.Sqrt(numOfParticles);
            source.Particle_Rate = (sr * sr) / 100;

            if (!nextPS)
                fileOutput += "CPU ";
            else
                fileOutput += "GPU ";
            fileOutput += numOfParticles + "/" + source.Particle_Rate + "\n";
        }

        void InitializeActions()
        {
            age = new AgeAction(GraphicsDevice, Content);
            pointDomain = new PointDomain(new Vector3(0, 0.1f, 0));
            ringDomain = new RingDomain(new Vector3(0, 0.3f, 0), Vector3.Up, 0.1f);
            source = new SourceAction(GraphicsDevice, Content, pointDomain, ringDomain, 1);
            gravity = new GravityAction(GraphicsDevice, Content, new Vector3(0, -0.01f, 0));
            fade = new FadeAction(GraphicsDevice, Content);
            bounce = new SquareBounceAction(GraphicsDevice, Content, Vector3.Zero, Vector3.Up, 20, 20, 0.9f);
            move = new MoveAction(GraphicsDevice, Content);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            cam.Update();

            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer >= thirtySeconds && !warmUp)
            {
                timer = 0;
                testIndex++;
                if (testIndex >= testCases.Length)
                    if (!nextPS)
                    {
                        testIndex = 0;
                        nextPS = !nextPS;
                        warmUp = true;
                        LoadNewParticleSystems(testCases[testIndex]);
                    }
                    else
                    {
                        StreamWriter sw = new StreamWriter("..\\..\\..\\..\\..\\cpu_gpu_fps_test.txt");
                        string[] output = fileOutput.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string[,] csvOutput = new string[testCases.Length * 2, 2000];
                        int csvRowIndex = 0;
                        int csvColIndex = -1;
                        foreach (string o in output)
                        {
                            if (o.Contains("CPU") || o.Contains("GPU"))
                            {
                                csvRowIndex = 0;
                                csvColIndex++;
                            }
                            if (csvRowIndex < 2000)
                            {
                                csvOutput[csvColIndex, csvRowIndex] = o;
                                csvRowIndex++;
                            }
                            else
                                throw new Exception("Ooops");
                        }

                        string line = "";
                        for (int y = 0; y < 2000; y++)
                        {
                            line = "";
                            for (int x = 0; x < testCases.Length * 2; x++)
                            {
                                if (x + 1 >= testCases.Length * 2)
                                    line += csvOutput[x, y];
                                else
                                    line += csvOutput[x, y] + ", ";
                            }
                            sw.WriteLine(line);
                        }
                        sw.Close();
                        this.Exit();
                    }
                else
                    LoadNewParticleSystems(testCases[testIndex]);
            }
            else if (timer >= 10000 && warmUp)
            {
                warmUp = false;
                timer = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (!nextPS)
            {
                if (!warmUp)
                {
                    ps_CPU.ApplyActions();
                    GraphicsDevice.Clear(Color.Black);
                    ps_CPU.DrawParticles(cam.View, cam.Projection, cam.Up);
                }
            }
            else
            {
                if (!warmUp)
                {
                    ps_GPU.ApplyActions();
                    GraphicsDevice.Clear(Color.Black);
                    ps_GPU.DrawParticles(cam.View, cam.Projection, cam.Up);
                }
            }

            frameCounter[frameIndex] = 1 / ((float)gameTime.ElapsedGameTime.Milliseconds * 0.001f);

            for (int i = 0; i < frameCounter.Length; i++)
                frameRate += frameCounter[i];
            frameRate /= (float)frameCounter.Length;

            if (!warmUp)
                fileOutput += frameRate + "\n";

            frameIndex++;
            if (frameIndex >= frameCounter.Length)
                frameIndex = 0;

            base.Draw(gameTime);
        }
    }
}
