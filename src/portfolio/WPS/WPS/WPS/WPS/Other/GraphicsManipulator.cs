using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WPS
{
    public class GraphicsManipulator
    {
        /// <summary>
        /// Use Additive for fire and Non-Premultiplied for smoke
        /// </summary>
        public BlendState BlendState = BlendState.Opaque;

        /// <summary>
        /// Default for normal and DepthRead for particles
        /// </summary>
        public DepthStencilState DepthStencilState = DepthStencilState.Default;
    }
}
