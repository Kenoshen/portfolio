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
    public static class WPSXMLParser
    {
        public static List<WPSXMLParticleSystem> ParseWPSXMLFile(string filepath)
        {
            List<WPSXMLParticleSystem> ObjectInfoTree = new List<WPSXMLParticleSystem>();

            StreamReader sr = File.OpenText(filepath);

            WPSXMLParticleSystem currParticleSystem = new WPSXMLParticleSystem();
            currParticleSystem.Properties = new List<WPSXMLProperty>();
            currParticleSystem.Domains = new List<WPSXMLDomain>();
            currParticleSystem.PermanentActions = new List<WPSXMLAction>();
            currParticleSystem.TemporaryActions = new List<WPSXMLAction>();
            WPSXMLDomain currDomain = new WPSXMLDomain();
            currDomain.Type = "";
            currDomain.Properties = new List<WPSXMLProperty>();
            WPSXMLAction currAction = new WPSXMLAction();
            currAction.Type = "";
            currAction.Properties = new List<WPSXMLProperty>();
            WPSXMLProperty currProperty = new WPSXMLProperty();
            currProperty.Name = "";
            currProperty.Type = "";
            currProperty.Value = "";
            WPSXMLState currState = WPSXMLState.NONE;

            // Reads through the file title
            sr.ReadLine();

            string line = sr.ReadLine();
            while (line != "End")
            {
                string[] parsed = line.Split(new char[3] { '<', ',', '>' }, StringSplitOptions.RemoveEmptyEntries);
                if (parsed.Length > 0)
                    switch (currState)
                    {
                        case WPSXMLState.NONE:
                            if (parsed[0] == "particlesystem")
                            {
                                currParticleSystem = new WPSXMLParticleSystem();
                                currParticleSystem.Properties = new List<WPSXMLProperty>();
                                currParticleSystem.Domains = new List<WPSXMLDomain>();
                                currParticleSystem.PermanentActions = new List<WPSXMLAction>();
                                currParticleSystem.TemporaryActions = new List<WPSXMLAction>();
                                ObjectInfoTree.Add(currParticleSystem);
                                currState = WPSXMLState.PARTICLE_SYSTEM;
                            }
                            break;

                        case WPSXMLState.PARTICLE_SYSTEM:
                            switch (parsed[0])
                            {
                                case "property":
                                    ParsePropertyLine(out currProperty, parsed);
                                    currParticleSystem.Properties.Add(currProperty);
                                    break;

                                case "domains":
                                    currState = WPSXMLState.DOMAINS;
                                    break;

                                case "permanentactions":
                                    currState = WPSXMLState.PERMANENTACTIONS;
                                    break;

                                case "temporaryactions":
                                    currState = WPSXMLState.TEMPORARYACTIONS;
                                    break;

                                case "/particlesystem":
                                    currState = WPSXMLState.NONE;
                                    break;
                            }
                            break;

                        case WPSXMLState.DOMAINS:
                            switch (parsed[0])
                            {
                                case "domain":
                                    ParseDorALine(out currDomain, parsed);
                                    currParticleSystem.Domains.Add(currDomain);
                                    break;

                                case "property":
                                    ParsePropertyLine(out currProperty, parsed);
                                    currDomain.Properties.Add(currProperty);
                                    break;

                                case "/domains":
                                    currState = WPSXMLState.PARTICLE_SYSTEM;
                                    break;
                            }
                            break;

                        case WPSXMLState.PERMANENTACTIONS:
                            switch (parsed[0])
                            {
                                case "action":
                                    ParseDorALine(out currAction, parsed);
                                    currParticleSystem.PermanentActions.Add(currAction);
                                    break;

                                case "property":
                                    ParsePropertyLine(out currProperty, parsed);
                                    currAction.Properties.Add(currProperty);
                                    break;

                                case "/permanentactions":
                                    currState = WPSXMLState.PARTICLE_SYSTEM;
                                    break;
                            }
                            break;

                        case WPSXMLState.TEMPORARYACTIONS:
                            switch (parsed[0])
                            {
                                case "action":
                                    ParseDorALine(out currAction, parsed);
                                    currParticleSystem.TemporaryActions.Add(currAction);
                                    break;

                                case "property":
                                    ParsePropertyLine(out currProperty, parsed);
                                    currAction.Properties.Add(currProperty);
                                    break;

                                case "/temporaryactions":
                                    currState = WPSXMLState.PARTICLE_SYSTEM;
                                    break;
                            }
                            break;
                    }

                line = sr.ReadLine();
            }

            sr.Close();

            return ObjectInfoTree;
        }

        private static void ParsePropertyLine(out WPSXMLProperty p, string[] line)
        {
            p = new WPSXMLProperty();
            p.Name = line[1].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            p.Type = line[2].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            p.Value = line[3];
        }

        private static void ParseDorALine(out WPSXMLDomain d, string[] line)
        {
            d = new WPSXMLDomain();
            d.Type = line[1].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            d.Properties = new List<WPSXMLProperty>();
        }

        private static void ParseDorALine(out WPSXMLAction a, string[] line)
        {
            a = new WPSXMLAction();
            a.Type = line[1].Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            a.Properties = new List<WPSXMLProperty>();
        }
    }
}
