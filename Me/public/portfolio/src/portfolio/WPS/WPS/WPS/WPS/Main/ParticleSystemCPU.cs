using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class ParticleSystemCPU
    {
        GraphicsDevice GraphicsDevice;
        ContentManager Content;
        Random rnd;

        ParticleCollectionCPU particleCollection;
        Texture2D particleTexture;
        public Vector4[] Position;
        public Vector4[] Velocity;
        // alpha, size, age, rotation speed
        public Vector4[] Data;

        GraphicsManipulator falseGraphMan;
        GraphicsManipulator trueGraphMan;

        int size = 0;
        int numOfParticles = 0;
        public float MaxAge;

        List<WPSAction> ActionsThisFrame = new List<WPSAction>();
        List<WPSAction> PermanentActions = new List<WPSAction>();

        /// <summary>
        /// The particle system is the main feature of WPS.  It is a simple collection of data textures that track position, velocity, and (alpha, size, age, rotation speed).  To add complexity, you must add "Actions" to the system.  These actions can be permanent or just last a single frame.  Each action is a small set of functionality that when combined with other actions, can cause complex behaviors.
        /// </summary>
        /// <param name="maxNumOfParticles">the maximum number of particles allows in the particle system (~=)</param>
        /// <param name="ageLimit">the age limit of the particles in the particle system</param>
        /// <param name="particleTexture">the display texture for the particles in the particle system</param>
        /// <param name="particleVisability">use ALPHA for particles that would represent fire, and use OPAQUE for particles that would represent smoke</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public ParticleSystemCPU(int maxNumOfParticles, float ageLimit, Texture2D particleTexture, ParticleSystemVisability particleVisability, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            if (maxNumOfParticles > Global_Variables_WPS.MaxParticles)
                throw new Exception("Cannot initialize a particle system with a max number of particles over the allowed limit (512 x 512).");
            else if (maxNumOfParticles < 4)
                throw new Exception("Cannot initialize a particle system with a max number of particles under the allwed limit (2 x 2).");

            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content; 
            this.particleTexture = particleTexture;

            Initialize(maxNumOfParticles, ageLimit, particleVisability);
        }

        /// <summary>
        /// The particle system is the main feature of WPS.  It is a simple collection of data textures that track position, velocity, and (alpha, size, and age).  To add complexity, you must add "Actions" to the system.  These actions can be permanent or just last a single frame.  Each action is a small set of functionality that when combined with other actions, can cause complex behaviors.
        /// </summary>
        /// <param name="maxNumOfParticles">the maximum number of particles allows in the particle system (~=)</param>
        /// <param name="ageLimit">the age limit of the particles in the particle system</param>
        /// <param name="particlePreSetType">for simplicity, you can use a preset type of particle that uses pre-made textures for the particles in the particle system</param>
        /// <param name="particleVisability">use ALPHA for particles that would represent fire, and use OPAQUE for particles that would represent smoke</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public ParticleSystemCPU(int maxNumOfParticles, float ageLimit, ParticleSystemPreSetType particlePreSetType, ParticleSystemVisability particleVisability, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            if (maxNumOfParticles > Global_Variables_WPS.MaxParticles)
                throw new Exception("Cannot initialize a particle system with a max number of particles over the allowed limit (512 x 512).");
            else if (maxNumOfParticles < 4)
                throw new Exception("Cannot initialize a particle system with a max number of particles under the allwed limit (2 x 2).");

            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content;
            LoadParticleTexturePreset(particlePreSetType);

            Initialize(maxNumOfParticles, ageLimit, particleVisability);
        }

        private void Initialize(int maxNumOfParticles, float ageLimit, ParticleSystemVisability particleVisability)
        {
            rnd = new Random();

            size = (int)Math.Sqrt(maxNumOfParticles);
            numOfParticles = size * size;
            this.MaxAge = ageLimit;

            Position = new Vector4[numOfParticles];
            Velocity = new Vector4[numOfParticles];
            Data = new Vector4[numOfParticles];
            for (int i = 0; i < numOfParticles; i++)
            {
                Position[i] = new Vector4(-1000000, -1000000, -1000000, -1000000);
                Velocity[i] = new Vector4(0, 0, 0, 0);
                Data[i] = new Vector4(-1, -1, -1, 0);
            }

            particleCollection = new ParticleCollectionCPU(particleTexture, GraphicsDevice, Content);

            trueGraphMan = new GraphicsManipulator();
            falseGraphMan = new GraphicsManipulator();

            switch (particleVisability)
            {
                case ParticleSystemVisability.ALPHA:
                    falseGraphMan.DepthStencilState = DepthStencilState.DepthRead;
                    falseGraphMan.BlendState = BlendState.Additive;
                    break;

                case ParticleSystemVisability.OPAQUE:
                    falseGraphMan.DepthStencilState = DepthStencilState.DepthRead;
                    falseGraphMan.BlendState = BlendState.NonPremultiplied;
                    break;

                default:
                    // leave the default in the false graphics manipulator
                    break;
            }
        }

        /// <summary>
        /// Adds an action to the list of actions to be applied this frame
        /// </summary>
        /// <param name="action">the action to be applied</param>
        public void AddActionForThisFrame(WPSAction action)
        {
            ActionsThisFrame.Add(action);
            action.ActionAddedCPU(rnd, Position, Velocity, Data, MaxAge);
        }

        /// <summary>
        /// Adds an action to the list of actions that will be applied for every frame until it is removed
        /// </summary>
        /// <param name="action">the action to be applied</param>
        public void AddPermanentAction(WPSAction action)
        {
            PermanentActions.Add(action);
            action.ActionAddedCPU(rnd, Position, Velocity, Data, MaxAge);
        }

        /// <summary>
        /// Removes an action from the list of actions that will be applied for every frame
        /// </summary>
        /// <param name="action">the action to be removed</param>
        /// <returns>true if the action was successfully removed</returns>
        public bool RemovePermanentAction(WPSAction action)
        {
            try
            {
                PermanentActions.Remove(action);
                action.ActionRemovedCPU(rnd, Position, Velocity, Data, MaxAge);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This applies the actions from the list of permanent actions and then the list of actions to be applied for this frame
        /// </summary>
        public void ApplyActions()
        {
            foreach (WPSAction action in PermanentActions)
                action.ApplyActionCPU(Position, Velocity, Data, MaxAge);

            foreach (WPSAction action in ActionsThisFrame)
            {
                action.ApplyActionCPU(Position, Velocity, Data, MaxAge);
                action.ActionRemovedCPU(rnd, Position, Velocity, Data, MaxAge);
            }

            if (ActionsThisFrame.Count > 0)
                ActionsThisFrame = new List<WPSAction>();
        }

        /// <summary>
        /// This draws the particles to the screen
        /// </summary>
        /// <param name="view">the view matrix for the camera</param>
        /// <param name="projection">the projection matrix for the camera</param>
        /// <param name="up">the camera's Up vector</param>
        public void DrawParticles(Matrix view, Matrix projection, Vector3 up)
        {
            GraphicsDevice.DepthStencilState = falseGraphMan.DepthStencilState;
            GraphicsDevice.BlendState = falseGraphMan.BlendState;

            particleCollection.DrawParticleCollection(view, projection, up, Position, Data, MaxAge);

            GraphicsDevice.DepthStencilState = trueGraphMan.DepthStencilState;
            GraphicsDevice.BlendState = trueGraphMan.BlendState;
        }

        /// <summary>
        /// Gets the number of active particles
        /// </summary>
        /// <returns>the number of active particles</returns>
        public int GetActiveParticleCount()
        {
            int count = 0;

            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].Z != -1)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the number of inactive particles
        /// </summary>
        /// <returns>the number of inactive particles</returns>
        public int GetInactiveParticleCount()
        {
            int count = 0;

            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].Z == -1)
                {
                    count++;
                }
            }

            return count;
        }

        private void LoadParticleTexturePreset(ParticleSystemPreSetType type)
        {
            switch (type)
            {
                case ParticleSystemPreSetType.FIRE:
                    particleTexture = Content.Load<Texture2D>(Global_Variables_WPS.ContentSprites + "fire");
                    break;

                case ParticleSystemPreSetType.SMOKE:
                    particleTexture = Content.Load<Texture2D>(Global_Variables_WPS.ContentSprites + "smoke");
                    break;

                case ParticleSystemPreSetType.WATER:
                    particleTexture = Content.Load<Texture2D>(Global_Variables_WPS.ContentSprites + "water");
                    break;

                case ParticleSystemPreSetType.EXPLOSION:
                    particleTexture = Content.Load<Texture2D>(Global_Variables_WPS.ContentSprites + "explosion");
                    break;
            }
        }
    }
}
