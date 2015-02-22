using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WPS;
using WPS.Input;
using WPS.Camera;

namespace ParticleGame
{
    public class MenuScreen
    {
        const int TRANTIME = 150;
        float tranMovement = 0;
        float size = 0.2f;

        int counterMax = 60;
        int counter = 0;

        public MenuScreenState State;

        ContentManager Content;
        GraphicsDevice GraphicsDevice;

        CMouse mouse;
        FreeCamera cam;

        Rectangle playRect;
        Rectangle optionsRect;
        Rectangle highscoresRect;

        ParticleSystemCPU ps;
        OrbitPointAction blastPoint;

        Rectangle viewport;

        List<Vector4> particlePositions = new List<Vector4>();

        int buttonIndex = -1;

        public MenuScreen(CMouse mouse, FreeCamera cam, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            this.mouse = mouse;
            this.cam = cam;
            this.Content = Content;
            this.GraphicsDevice = GraphicsDevice;

            viewport = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            int buttonWidth = (int)(viewport.Width * 0.3f);
            int buttonHeight = (int)(viewport.Height * 0.2f);
            int buttonX = (viewport.Width / 2) - (buttonWidth / 2);
            int buttonY = (int)(viewport.Height * 0.25f);

            playRect = new Rectangle(buttonX, (buttonY * 1) + (5 * 0), buttonWidth, buttonHeight);
            highscoresRect = new Rectangle(buttonX, (buttonY * 2) + (5 * 1), buttonWidth, buttonHeight);
            optionsRect = new Rectangle(buttonX, (buttonY * 3) + (5 * 2), buttonWidth, buttonHeight);
        }

        public void LoadContent()
        {
            List<Vector4> tempPositions;
            tempPositions = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("Menu/title"), (-120 * 0.5f), (80), 0, size);
            Copylist(tempPositions, particlePositions);
            tempPositions = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("Menu/playbutton"), (-25 * 0.5f), (50), 0, size);
            Copylist(tempPositions, particlePositions);
            playRect = new Rectangle(viewport.Width / 2, 200, 90, 65);
            playRect.X -= playRect.Width / 2;
            tempPositions = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("Menu/optionsbutton"), (-39 * 0.5f), (25), 0, size);
            Copylist(tempPositions, particlePositions);
            optionsRect = new Rectangle(viewport.Width / 2, 300, 150, 60);
            optionsRect.X -= optionsRect.Width / 2;
            tempPositions = TextureObjectBuilder.BuildSceneObjectFromTexture(Content.Load<Texture2D>("Menu/highscorebutton"), (-46 * 0.5f), (0), 0, size);
            Copylist(tempPositions, particlePositions);
            highscoresRect = new Rectangle(viewport.Width / 2, 400, 180, 55);
            highscoresRect.X -= highscoresRect.Width / 2;

            ps = new ParticleSystemCPU(2000, 100, Content.Load<Texture2D>("Art/block"), ParticleSystemVisability.OPAQUE, GraphicsDevice, Content);
            ps.AddPermanentAction(new MoveAction(GraphicsDevice, Content));
            ps.AddPermanentAction(new FadeAction(GraphicsDevice, Content));

            blastPoint = new OrbitPointAction(GraphicsDevice, Content, Vector3.Zero, 3, -0.9f, 1, 1);

            TransitionOn();
        }

        public void Update()
        {
            switch(State)
            {
                case MenuScreenState.ON:
                    if (mouse.LeftHeldDown())
                    {
                        int x = (int)mouse.MousePos.X;
                        int y = (int)mouse.MousePos.Y;
                        bool play = playRect.Contains(x, y);
                        bool options = optionsRect.Contains(x, y);
                        bool highscore = highscoresRect.Contains(x, y);
                        if (play || options || highscore)
                        {
                            //List<Vector4> throwAway = new List<Vector4>();
                            //throwAway.Add(new Vector4(GetClickPosition(), 0));
                            //ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, throwAway, Vector3.Zero, 0.3f));

                            blastPoint.OrbitPoint = GetClickPosition() + Vector3.UnitZ * -2;
                            ps.AddActionForThisFrame(blastPoint);

                            if (play)
                            {
                                buttonIndex = 0;
                                State = MenuScreenState.TRANSITION_OFF;
                            }
                            else if (options)
                            {
                                buttonIndex = 1;
                                State = MenuScreenState.TRANSITION_OFF;
                            }
                            else if (highscore)
                            {
                                buttonIndex = 2;
                                State = MenuScreenState.TRANSITION_OFF;
                            }

                        }
                    }
                    break;

                case MenuScreenState.TRANSITION_ON:
                    if (cam.Position.Y > 49 && cam.Position.Y < 50)
                        State = MenuScreenState.ON;
                    else
                        cam.Move(Vector3.Down * tranMovement);
                    break;

                case MenuScreenState.TRANSITION_OFF:
                    if (counter > counterMax)
                    {
                        if (cam.Position.Y > 19 && cam.Position.Y < 20)
                        {
                            switch (buttonIndex)
                            {
                                case 0:
                                    State = MenuScreenState.PLAY_OFF;
                                    break;
                                case 1:
                                    State = MenuScreenState.OPTIONS_OFF;
                                    break;
                                case 2:
                                    State = MenuScreenState.HIGHSCORE_OFF;
                                    break;
                            }
                            counter = 0;
                        }
                        else
                            cam.Move(Vector3.Down * tranMovement);
                    }
                    else
                    {
                        counter++;
                        if (mouse.LeftHeldDown())
                        {
                            blastPoint.OrbitPoint = GetClickPosition() + Vector3.UnitZ * -2;
                            ps.AddActionForThisFrame(blastPoint);
                        }
                    }
                    break;
            }
        }

        public void ApplyParticleActions()
        {
            ps.ApplyActions();
        }

        public void Draw(Matrix view, Matrix projection, Vector3 up)
        {
            ps.DrawParticles(view, projection, up);
        }

        public void TransitionOn()
        {
            State = MenuScreenState.TRANSITION_ON;

            Vector3 camPos = cam.Position;
            camPos.X = 0;
            camPos.Y = 50;
            cam.Position = camPos;

            ps.AddActionForThisFrame(new KillAllAction(GraphicsDevice, Content));
            ps.AddActionForThisFrame(new CustomSourceAction(GraphicsDevice, Content, particlePositions, GetScreenToWorldPosition(new Vector2(viewport.Width / 2f, viewport.Height)), Vector3.Zero, size));

            cam.Position = camPos + Vector3.Up * 40;
            cam.Update();
            tranMovement = (cam.Position.Y - 50) / TRANTIME;
        }

        public void TransitionOff()
        {
            State = MenuScreenState.TRANSITION_OFF;
        }

        public Vector3 GetClickPosition()
        {
            Vector2 mousePos = mouse.MousePos;
            Vector3 direction = new Vector3(
                (mousePos.X - (viewport.Width / 2)) / (viewport.Width),
                -(mousePos.Y - (viewport.Height / 2)) / (viewport.Height),
                -1);

            direction.X *= 1.4f;
            direction.Y *= 0.75f;
            direction *= cam.Position.Z;

            Vector3 point = direction + cam.Position;
            return point;
        }

        public Vector3 GetScreenToWorldPosition(Vector2 screenPos)
        {
            Vector3 direction = new Vector3(
                (screenPos.X - (viewport.Width / 2)) / (viewport.Width),
                -(screenPos.Y - (viewport.Height / 2)) / (viewport.Height),
                -1);

            direction.X *= 1.4f;
            direction.Y *= 0.75f;
            direction *= cam.Position.Z;

            Vector3 point = direction + cam.Position;
            return point;
        }

        private void Copylist(List<Vector4> from, List<Vector4> to)
        {
            for (int i = 0; i < from.Count; i++)
                to.Add(from[i]);
        }
    }

    public enum MenuScreenState
    {
        ON,
        OFF,
        PLAY_OFF,
        OPTIONS_OFF,
        HIGHSCORE_OFF,
        TRANSITION_ON,
        TRANSITION_OFF,
    }
}
