using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WPS
{
    public class Animator
    {
        private GraphicsDevice GraphicsDevice;
        private ContentManager Content;
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public Dictionary<string, object> AllObjects { get; private set; }
        public List<WPSXMLParticleSystem> ObjectInfoTree { get; set; }
        private List<string> ParticleSystems { get; set; }
        public List<Exception> CaughtExceptions = new List<Exception>();

        public Animator(string filename, string contentProjectName, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            FileName = filename;
            FilePath = System.Windows.Forms.Application.StartupPath + "\\..\\..\\..\\..\\" + contentProjectName + "\\" + FileName + ".wpsxml";
            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content;

            ObjectInfoTree = WPSXMLParser.ParseWPSXMLFile(FilePath);
            BuildObjects();
        }

        public Animator(string absoluteFilePath, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            string[] wpsxmlName = absoluteFilePath.Split(new char[2] { '\\', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (wpsxmlName.Length > 2)
                FileName = wpsxmlName[wpsxmlName.Length - 2];
            FilePath = absoluteFilePath;
            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content;

            ObjectInfoTree = WPSXMLParser.ParseWPSXMLFile(FilePath);
            BuildObjects();
        }

        public void ApplyActions()
        {
            foreach (string psName in ParticleSystems)
                ((AnimatorParticleSystem)AllObjects[psName]).ParticleSystem.ApplyActions();
        }

        public void DrawAnimator(Matrix view, Matrix projection, Vector3 up)
        {
            foreach(string psName in ParticleSystems)
                ((AnimatorParticleSystem)AllObjects[psName]).ParticleSystem.DrawParticles(view, projection, up);
        }

        #region ObjectBuilder
        private void BuildObjects()
        {
            AllObjects = new Dictionary<string, object>();
            ParticleSystems = new List<string>();
            Dictionary<string, object> properties = new Dictionary<string,object>();

            foreach (WPSXMLParticleSystem ps in ObjectInfoTree)
            {
                AnimatorParticleSystem aps = new AnimatorParticleSystem();

                properties = new Dictionary<string, object>();
                foreach (WPSXMLProperty property in ps.Properties)
                    InsertObject(properties, property.Name, ConvertFromString(property.Value, property.Type));

                aps.Name = (string)properties["Name"];
                aps.MaxParticles = (int)properties["MaxParticles"];
                aps.ParticleLife = (float)properties["ParticleLife"];
                aps.TextureFilePath = (string)properties["TextureFilePath"];
                aps.Visability = (string)properties["Visability"];
                aps.Loop = (bool)properties["Loop"];
                aps.CurrentFrame = 0;
                ParticleSystemVisability psv = ParticleSystemVisability.ALPHA;
                if (aps.Visability == "ALPHA")
                    psv = ParticleSystemVisability.ALPHA;
                else if (aps.Visability == "OPAQUE")
                    psv = ParticleSystemVisability.OPAQUE;
                aps.ParticleSystem = new ParticleSystemCPU(aps.MaxParticles, aps.ParticleLife, Content.Load<Texture2D>(aps.TextureFilePath), psv, GraphicsDevice, Content);
                InsertObject(AllObjects, aps.Name, aps);

                foreach (WPSXMLDomain domain in ps.Domains)
                {
                    properties = new Dictionary<string, object>();
                    foreach (WPSXMLProperty property in domain.Properties)
                        InsertObject(properties, property.Name, ConvertFromString(property.Value, property.Type));

                    AnimatorDomain ad = BuildDomain(properties, domain.Type);
                    InsertObject(AllObjects, ad.Name, ad);
                }

                foreach (WPSXMLAction action in ps.PermanentActions)
                {
                    properties = new Dictionary<string, object>();
                    foreach (WPSXMLProperty property in action.Properties)
                        InsertObject(properties, property.Name, ConvertFromString(property.Value, property.Type));

                    AnimatorPermAction ap = BuildPermAction(properties, action.Type);
                    aps.ParticleSystem.AddPermanentAction(ap.Action);
                    InsertObject(AllObjects, ap.Name, ap);
                }

                foreach (WPSXMLAction action in ps.TemporaryActions)
                {
                    properties = new Dictionary<string, object>();
                    foreach (WPSXMLProperty property in action.Properties)
                        InsertObject(properties, property.Name, ConvertFromString(property.Value, property.Type));

                    AnimatorTempAction at = BuildTempAction(properties, action.Type);
                    aps.ParticleSystem.AddActionForThisFrame(at.Action);
                    InsertObject(AllObjects, at.Name, at);
                }

                ParticleSystems.Add(aps.Name);
            }
        }
        
        private void InsertObject(Dictionary<string, object> dictionary, string key, object value)
        {
            try
            {
                object o = dictionary[key];
                InsertObject(dictionary, key + "0", value);
            }
            catch (Exception e)
            {
                dictionary.Add(key, value);
            }
        }

        private object ConvertFromString(string value, string type)
        {
            switch (type)
            {
                case "string":
                    return value;

                case "float":
                    try
                    {
                        return (float)Decimal.Parse(value);
                    }
                    catch (Exception e)
                    {
                        CaughtExceptions.Add(e);
                        return 0;
                    }

                case "int":
                    try
                    {
                        return (int)Int32.Parse(value);
                    }
                    catch (Exception e)
                    {
                        CaughtExceptions.Add(e);
                        return 0;
                    }

                case "bool":
                    if (value == "true")
                        return true;
                    else
                        return false;

                case "Vector3":
                    try
                    {
                        string[] parsed = value.Split(new char[3] { '{', ' ', '}' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parsed.Length != 3)
                            throw new Exception("Not a valid Vector3 string");
                        float[] values = new float[3];
                        for (int i = 0; i < parsed.Length; i++)
                            values[i] = (float)Decimal.Parse(parsed[i].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        return new Vector3(values[0], values[1], values[2]);
                    }
                    catch (Exception e)
                    {
                        CaughtExceptions.Add(e);
                        return Vector3.Zero;
                    }

                default:
                    CaughtExceptions.Add(new Exception("Could not determine the type"));
                    return null;
            }
        }

        private AnimatorDomain BuildDomain(Dictionary<string, object> properties, string type)
        {
            AnimatorDomain d = new AnimatorDomain();
            d.Name = (string)properties["Name"];

            switch (type)
            {
                case "BoxDomain":
                    d.Domain = new BoxDomain((Vector3)properties["Center"], (float)properties["Width"], (float)properties["Height"], (float)properties["Depth"]);
                    break;

                case "CylinderDomain":
                    d.Domain = new CylinderDomain((Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["Length"], (float)properties["OuterRadius"], (float)properties["InnerRadius"]);
                    break;

                case "DiscDomain":
                    d.Domain = new DiscDomain((Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["Radius"]);
                    break;

                case "LineDomain":
                    d.Domain = new LineDomain((Vector3)properties["StartPoint"], (Vector3)properties["EndPoint"]);
                    break;

                case "PointDomain":
                    d.Domain = new PointDomain((Vector3)properties["Point"]);
                    break;

                case "RingDomain":
                    d.Domain = new RingDomain((Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["Radius"]);
                    break;

                case "SphereDomain":
                    d.Domain = new SphereDomain((Vector3)properties["Center"], (float)properties["Radius"]);
                    break;

                case "SquareDomain":
                    d.Domain = new SquareDomain((Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["Width"], (float)properties["Height"]);
                    break;

                default:
                    CaughtExceptions.Add(new Exception("Undefined domain type"));
                    break;
            }

            return d;
        }

        private AnimatorPermAction BuildPermAction(Dictionary<string, object> properties, string type)
        {
            AnimatorPermAction pa = new AnimatorPermAction();
            pa.Name = (string)properties["Name"];

            switch (type)
            {
                case "AgeAction":
                    pa.Action = new AgeAction(GraphicsDevice, Content, (float)properties["AgeStep"]);
                    break;

                case "DiscBounceAction":
                    pa.Action = new DiscBounceAction(GraphicsDevice, Content, (Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["Radius"], (float)properties["Dampening"], (float)properties["TimeStep"]);
                    break;

                case "FadeAction":
                    pa.Action = new FadeAction(GraphicsDevice, Content);
                    break;

                case "GravityAction":
                    pa.Action = new GravityAction(GraphicsDevice, Content, (Vector3)properties["Gravity"], (float)properties["TimeStep"]);
                    break;

                case "KillAllAction":
                    pa.Action = new KillAllAction(GraphicsDevice, Content);
                    break;

                case "MoveAction":
                    pa.Action = new MoveAction(GraphicsDevice, Content, (float)properties["TimeStep"]);
                    break;

                case "OrbitAxisAction":
                    pa.Action = new OrbitAxisAction(GraphicsDevice, Content, (Vector3)properties["StartPoint"], (Vector3)properties["EndPoint"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "OrbitLineAction":
                    pa.Action = new OrbitLineAction(GraphicsDevice, Content, (Vector3)properties["StartPoint"], (Vector3)properties["EndPoint"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "OrbitPointAction":
                    pa.Action = new OrbitPointAction(GraphicsDevice, Content, (Vector3)properties["OrbitPoint"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "OrbitRingAction":
                    pa.Action = new OrbitRingAction(GraphicsDevice, Content, (Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "PlaneBounceAction":
                    pa.Action = new PlaneBounceAction(GraphicsDevice, Content, (Vector3)properties["PlanePoint"], (Vector3)properties["Normal"], (float)properties["Dampening"], (float)properties["TimeStep"]);
                    break;

                case "RotateAction":
                    pa.Action = new RotateAction(GraphicsDevice, Content, (float)properties["MinRotRevs"], (float)properties["MaxRotRevs"]);
                    break;

                case "ScaleAction":
                    pa.Action = new ScaleAction(GraphicsDevice, Content, (float)properties["EndScale"]);
                    break;

                case "SourceAction":
                    Domain posDom;
                    Domain velDom;
                    try
                    {
                        posDom = ((AnimatorDomain)AllObjects[(string)properties["PositionDomain"]]).Domain;
                    }
                    catch (Exception)
                    {
                        CaughtExceptions.Add(new Exception("Undefined position domain"));
                        posDom = new PointDomain(Vector3.Zero);
                    }
                    try
                    {
                        velDom = ((AnimatorDomain)AllObjects[(string)properties["VelocityDomain"]]).Domain;
                    }
                    catch (Exception)
                    {
                        CaughtExceptions.Add(new Exception("Undefined velocity domain"));
                        velDom = new PointDomain(Vector3.Zero);
                    }
                    pa.Action = new SourceAction(GraphicsDevice, Content, posDom, velDom, (float)properties["Size"], (float)properties["Particle_Rate"], (float)properties["TimeStep"]);
                    break;

                case "SquareBounceAction":
                    pa.Action = new SquareBounceAction(GraphicsDevice, Content, (Vector3)properties["PlanePoint"], (Vector3)properties["Normal"], (float)properties["Width"], (float)properties["Height"], (float)properties["Dampening"], (float)properties["TimeStep"]);
                    break;

                case "ZeroVelAction":
                    pa.Action = new ZeroVelAction(GraphicsDevice, Content);
                    break;

                default:
                    CaughtExceptions.Add(new Exception("Undefined action type"));
                    break;
            }

            return pa;
        }

        private AnimatorTempAction BuildTempAction(Dictionary<string, object> properties, string type)
        {
            AnimatorTempAction pa = new AnimatorTempAction();
            pa.Name = (string)properties["Name"];
            pa.FrameAdded = (int)properties["FrameAdded"];
            pa.FrameRemoved = (int)properties["FrameRemoved"];

            switch (type)
            {
                case "AgeAction":
                    pa.Action = new AgeAction(GraphicsDevice, Content, (float)properties["AgeStep"]);
                    break;

                case "DiscBounceAction":
                    pa.Action = new DiscBounceAction(GraphicsDevice, Content, (Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["Radius"], (float)properties["Dampening"], (float)properties["TimeStep"]);
                    break;

                case "FadeAction":
                    pa.Action = new FadeAction(GraphicsDevice, Content);
                    break;

                case "GravityAction":
                    pa.Action = new GravityAction(GraphicsDevice, Content, (Vector3)properties["Gravity"], (float)properties["TimeStep"]);
                    break;

                case "KillAllAction":
                    pa.Action = new KillAllAction(GraphicsDevice, Content);
                    break;

                case "MoveAction":
                    pa.Action = new MoveAction(GraphicsDevice, Content, (float)properties["TimeStep"]);
                    break;

                case "OrbitAxisAction":
                    pa.Action = new OrbitAxisAction(GraphicsDevice, Content, (Vector3)properties["StartPoint"], (Vector3)properties["EndPoint"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "OrbitLineAction":
                    pa.Action = new OrbitLineAction(GraphicsDevice, Content, (Vector3)properties["StartPoint"], (Vector3)properties["EndPoint"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "OrbitPointAction":
                    pa.Action = new OrbitPointAction(GraphicsDevice, Content, (Vector3)properties["OrbitPoint"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "OrbitRingAction":
                    pa.Action = new OrbitRingAction(GraphicsDevice, Content, (Vector3)properties["Center"], (Vector3)properties["Normal"], (float)properties["MaxRange"], (float)properties["Magnitude"], (float)properties["Epsilon"], (float)properties["TimeStep"]);
                    break;

                case "PlaneBounceAction":
                    pa.Action = new PlaneBounceAction(GraphicsDevice, Content, (Vector3)properties["PlanePoint"], (Vector3)properties["Normal"], (float)properties["Dampening"], (float)properties["TimeStep"]);
                    break;

                case "RotateAction":
                    pa.Action = new RotateAction(GraphicsDevice, Content, (float)properties["MinRotRevs"], (float)properties["MaxRotRevs"]);
                    break;

                case "ScaleAction":
                    pa.Action = new ScaleAction(GraphicsDevice, Content, (float)properties["EndScale"]);
                    break;

                case "SourceAction":
                    Domain posDom;
                    Domain velDom;
                    try
                    {
                        posDom = ((AnimatorDomain)AllObjects[(string)properties["PositionDomain"]]).Domain;
                    }
                    catch (Exception)
                    {
                        CaughtExceptions.Add(new Exception("Undefined position domain"));
                        posDom = new PointDomain(Vector3.Zero);
                    }
                    try
                    {
                        velDom = ((AnimatorDomain)AllObjects[(string)properties["VelocityDomain"]]).Domain;
                    }
                    catch (Exception)
                    {
                        CaughtExceptions.Add(new Exception("Undefined velocity domain"));
                        velDom = new PointDomain(Vector3.Zero);
                    }
                    pa.Action = new SourceAction(GraphicsDevice, Content, posDom, velDom, (float)properties["Size"], (float)properties["Particle_Rate"], (float)properties["TimeStep"]);
                    break;

                case "SquareBounceAction":
                    pa.Action = new SquareBounceAction(GraphicsDevice, Content, (Vector3)properties["PlanePoint"], (Vector3)properties["Normal"], (float)properties["Width"], (float)properties["Height"], (float)properties["Dampening"], (float)properties["TimeStep"]);
                    break;

                case "ZeroVelAction":
                    pa.Action = new ZeroVelAction(GraphicsDevice, Content);
                    break;

                default:
                    CaughtExceptions.Add(new Exception("Undefined action type"));
                    break;
            }

            return pa;
        }
        #endregion
    }

    public enum WPSXMLState
    {
        NONE,
        PARTICLE_SYSTEM,
        DOMAINS,
        PERMANENTACTIONS,
        TEMPORARYACTIONS,
    }

    public struct WPSXMLParticleSystem
    {
        public List<WPSXMLProperty> Properties;
        public List<WPSXMLDomain> Domains;
        public List<WPSXMLAction> PermanentActions;
        public List<WPSXMLAction> TemporaryActions;
    }

    public struct WPSXMLDomain
    {
        public string Type;
        public List<WPSXMLProperty> Properties;
    }

    public struct WPSXMLAction
    {
        public string Type;
        public List<WPSXMLProperty> Properties;
    }

    public struct WPSXMLProperty
    {
        public string Name;
        public string Type;
        public string Value;
    }

    public struct AnimatorParticleSystem
    {
        public string Name;
        public ParticleSystemCPU ParticleSystem;
        public int MaxParticles;
        public float ParticleLife;
        public string TextureFilePath;
        public string Visability;
        public int TotalFrames;
        public bool Loop;
        public int CurrentFrame;
    }

    public struct AnimatorDomain
    {
        public string Name;
        public Domain Domain;
    }

    public struct AnimatorPermAction
    {
        public string Name;
        public WPSAction Action;
    }

    public struct AnimatorTempAction
    {
        public string Name;
        public int FrameAdded;
        public int FrameRemoved;
        public WPSAction Action;
    }
}
