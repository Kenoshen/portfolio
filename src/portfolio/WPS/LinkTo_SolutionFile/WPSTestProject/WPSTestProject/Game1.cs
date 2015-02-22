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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Need this stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rnd;
        CKeyboard keyboard;
        CMouse mouse;
        ArcBallCamera cam;
        Effect floorEffect;
        Texture2D floorTexture;
        Quad floor;
        Debugger_WPS debugger;
        List<UseCase> useCases;
        int currentUseCase = 0;
        #endregion
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // ***VERY IMPORTANT***
            // Must have this line of code for stuff to work
            // Must also be using the HighDef profile
            fbDeprofiler.DeProfiler.Run();

            // all the different use cases
            useCases = new List<UseCase>();
            useCases.Add(new UseCase_DualSprayBounce());
            useCases.Add(new UseCase_Fountain());
            useCases.Add(new UseCase_Cube());
            useCases.Add(new UseCase_Cube2());
            useCases.Add(new UseCase_Sprial());
            useCases.Add(new UseCase_Bomb());
            useCases.Add(new UseCase_RingOfFire());
            useCases.Add(new UseCase_Donut());
            useCases.Add(new UseCase_Snow());
            useCases.Add(new UseCase_Editor1(true));

            currentUseCase = GetActiveUseCase();
            ChangeActiveUseCase(currentUseCase);
        }

        protected override void Initialize()
        {
            rnd = new Random();
            keyboard = new CKeyboard();
            mouse = new CMouse();
            cam = new ArcBallCamera(Vector3.Up * 5, 0, 0.2f, MathHelper.ToRadians(-360), MathHelper.ToRadians(360), 100, 10, 400, GraphicsDevice);

            foreach (UseCase u in useCases)
                u.Initialize(graphics, rnd, keyboard, mouse, cam);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugger = new Debugger_WPS(GraphicsDevice, Content);
            floorEffect = Content.Load<Effect>(Global_Variables_WPS.ContentEffects + "DrawPerspectiveQuad");
            floorTexture = Content.Load<Texture2D>("floor");
            floor = new Quad(GraphicsDevice);

            foreach (UseCase u in useCases)
                u.LoadContent(spriteBatch, Content, debugger, floorEffect, floorTexture, floor);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard.BeginUpdate();
            mouse.BeginUpdate();

            if (keyboard.IsKeyBeingPressed(Keys.Escape))
                this.Exit();

            foreach (UseCase u in useCases)
                if (u.Active)
                    u.Update(gameTime);

            if (keyboard.IsKeyJustPressed(Keys.PageUp))
            {
                currentUseCase++;
                if (currentUseCase >= useCases.Count)
                    currentUseCase = 0;
                ChangeActiveUseCase(currentUseCase);
            }
            else if (keyboard.IsKeyJustPressed(Keys.PageDown))
            {
                currentUseCase--;
                if (currentUseCase < 0)
                    currentUseCase = useCases.Count - 1;
                ChangeActiveUseCase(currentUseCase);
            }

            keyboard.EndUpdate();
            mouse.EndUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            foreach (UseCase u in useCases)
                if (u.Active)
                    u.Draw(gameTime);
            
            base.Draw(gameTime);
        }

        public void ChangeActiveUseCase(int index)
        {
            for (int i = 0; i < useCases.Count; i++)
            {
                if (i == index)
                    useCases[i].Active = true;
                else
                    useCases[i].Active = false;
            }
        }

        public int GetActiveUseCase()
        {
            for (int i = 0; i < useCases.Count; i++)
            {
                if (useCases[i].Active)
                    return i;
            }
            return -1;
        }
    }
}