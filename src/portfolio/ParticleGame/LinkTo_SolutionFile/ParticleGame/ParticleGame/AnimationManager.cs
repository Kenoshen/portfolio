using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ParticleGame
{
    class AnimationManager
    {
        List<Texture2D> currentAnimation;

        List<Texture2D> running = new List<Texture2D>();
        List<Texture2D> jumping = new List<Texture2D>();
        List<Texture2D> wallGrab = new List<Texture2D>();
        List<Texture2D> transition = new List<Texture2D>();
        List<Texture2D> falling = new List<Texture2D>();

        int currentIndex = 0;

        public AnimationState State;

        public AnimationManager(ContentManager Content)
        {
            MakeAnimations(Content);
            currentAnimation = running;
            State = AnimationState.RUNNING;
        }

        public void Update()
        {
            currentIndex++;
            if (currentIndex >= currentAnimation.Count)
            {
                switch (State)
                {
                    case AnimationState.RUNNING:
                        currentIndex = 0;
                        break;

                    case AnimationState.FALLING:
                        currentIndex--;
                        break;

                    case AnimationState.JUMPING:
                        currentIndex--;
                        break;

                    case AnimationState.TRANSITION_TO_RUNNING:
                        currentIndex = 0;
                        TransitionToRunning();
                        break;

                    case AnimationState.WALL_GRAB:
                        currentIndex--;
                        break;
                }
            }
        }

        public void TransitionToRunning()
        {
            currentAnimation = running;
            currentIndex = 0;
            State = AnimationState.RUNNING;
        }

        public void TransitionToJumping()
        {
            currentAnimation = jumping;
            currentIndex = 0;
            State = AnimationState.JUMPING;
        }

        public void TransitionToWallGrab()
        {
            currentAnimation = wallGrab;
            currentIndex = 0;
            State = AnimationState.WALL_GRAB;
        }

        public void TransitionToTransition()
        {
            currentAnimation = transition;
            currentIndex = 0;
            State = AnimationState.TRANSITION_TO_RUNNING;
        }

        public void TransitionToFalling()
        {
            currentAnimation = falling;
            currentIndex = 0;
            State = AnimationState.FALLING;
        }

        public Texture2D GetCurrentTexture()
        {
            return currentAnimation[currentIndex];
        }

        private void MakeAnimations(ContentManager Content)
        {
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_1"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_1"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_2"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_2"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_3"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_3"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_4"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_4"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_5"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_5"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_6"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_6"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_7"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_7"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_8"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_8"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_9"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_9"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_10"));
            running.Add(Content.Load<Texture2D>("Guy/Running/guy_running_10"));

            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_5")); 
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_5"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_6"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_6"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_7"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_7"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_8"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_8"));
            jumping.Add(Content.Load<Texture2D>("Guy/Jumping/guy_jumping_9"));

            wallGrab.Add(Content.Load<Texture2D>("Guy/guy_wallgrabRight_1"));

            falling.Add(Content.Load<Texture2D>("Guy/Falling/guy_falling_1"));
            falling.Add(Content.Load<Texture2D>("Guy/Falling/guy_falling_1"));
            falling.Add(Content.Load<Texture2D>("Guy/Falling/guy_falling_2"));
            falling.Add(Content.Load<Texture2D>("Guy/Falling/guy_falling_2"));
            falling.Add(Content.Load<Texture2D>("Guy/Falling/guy_falling_3"));
        }
    }

    public enum AnimationState
    {
        TRANSITION_TO_RUNNING,
        WALL_GRAB,
        RUNNING,
        FALLING,
        JUMPING,
    }
}
