using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS
{
    /// <summary>
    /// Global variables that are used by WPS to simplify loading content
    /// </summary>
    public static class Global_Variables_WPS
    {
        public static string ContentBase = "WPS_CONTENT/";
        public static string ContentEffects = "WPS_CONTENT/EFFECTS/";
        public static string ContentActionEffects = "WPS_CONTENT/EFFECTS/ACTIONS/";
        public static string ContentSprites = "WPS_CONTENT/SPRITES/";
        public static string ContentOther = "WPS_CONTENT/OTHER/";
        public static int MaxParticles = 262144;
    }

    /// <summary>
    /// Preset type of particle to use when initializing a particle system
    /// </summary>
    public enum ParticleSystemPreSetType
    {
        FIRE,
        SMOKE,
        WATER,
        EXPLOSION,
    }

    public enum ParticleSystemVisability
    {
        ALPHA,
        OPAQUE,
    }
}
