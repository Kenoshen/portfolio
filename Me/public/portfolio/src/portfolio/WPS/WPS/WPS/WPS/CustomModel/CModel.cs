using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.CustomModel
{
    public class CModel
    {
        /// <summary>
        /// The position of the model
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The rotation of the model
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// The scale of the model
        /// </summary>
        public Vector3 Scale { get; set; }
        
        /// <summary>
        /// The actual model object
        /// </summary>
        public Model Model { get; private set; }
        private Matrix[] modelTransforms;

        private GraphicsDevice graphicsDevice;

        private BoundingSphere boundingSphere;

        /// <summary>
        /// The model's bounding sphere
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);

                return transformed;
            }
        }

        /// <summary>
        /// This is a custom model class that simplifies the drawing of models
        /// </summary>
        /// <param name="Model">the pre-loaded model ie: Content.Load[lessthan]Model[greaterthan](string locationOfModel);</param>
        /// <param name="Position">the position of the model</param>
        /// <param name="Rotation">the rotation of the model</param>
        /// <param name="Scale">the scale of the model</param>
        /// <param name="graphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public CModel(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            this.Model = Model;

            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            buildBoundingSphere();
            generateTags();

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;

            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Draws the model using the basic effect class
        /// </summary>
        /// <param name="View">the view matrix of the camera</param>
        /// <param name="Projection">the projection matrix of the camera</param>
        public void Draw(Matrix View, Matrix Projection)
        {
            // Calculate te base transformation by combining
            // translation, rotation, and scaling
            Matrix baseWorld = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(
                Rotation.Y, Rotation.X, Rotation.Z) *
                Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    BasicEffect beffect = (BasicEffect)meshPart.Effect;

                    beffect.World = localWorld;
                    beffect.View = View;
                    beffect.Projection = Projection;

                    beffect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
        
        /// <summary>
        /// Gets the World matrix of the camera
        /// </summary>
        /// <returns>world matrix of the camera</returns>
        public Matrix GetWorld()
        {
            return Matrix.Identity * Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * Matrix.CreateTranslation(Position) * Matrix.CreateScale(Scale);
        }

        /// <summary>
        /// Gets the normal matrix of the camera
        /// </summary>
        /// <returns>normal matrix of the camera</returns>
        public Matrix GetNormalMatrix()
        {
            return Matrix.Identity * Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);
        }

        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);

            // Merge all the model's build in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }

            this.boundingSphere = sphere;
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
        }

        /// <summary>
        /// Store references to all of the model's current effects * currently doesn't work
        /// </summary>
        public void CacheEffect()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }

        /// <summary>
        /// Restore the effects referenced by the model's cache *currently doesn't work
        /// </summary>
        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
                        
        }

        /// <summary>
        /// Sets the effect used by the model *currently doesn't work
        /// </summary>
        /// <param name="effect">the effect</param>
        /// <param name="CopyEffect">if true, the effect is copyied over</param>
        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;

                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();

                    MeshTag tag = ((MeshTag)part.Tag);

                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                    {
                        setEffectParameter(toSet, "TextureEnabled", false);
                    }

                    // Set our remain parameters to the effect
                    setEffectParameter(toSet, "DeffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

                    part.Effect = toSet;
                }
        }

        // Sets the specified effect parameter to the given effect, if it has that parameter
        void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;

            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
        }
    }
}
