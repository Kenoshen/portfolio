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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Collections.Specialized;

namespace TypingInvaders
{
    public class TextReader
    {
        List<Keys> topRow;
        List<Keys> homeRow;
        List<Keys> bottomRow;
        int min = 2;
        int max = 8;

        public TextReader()
        {
            topRow = new List<Keys>();
            homeRow = new List<Keys>();
            bottomRow = new List<Keys>();

            EstablishKeys();
        }

        public List<string> LoadTextFile(string path)
        {
            List<string> newlines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                    while (line != null)
                    {
                        if (line.Length > min && line.Length < max)
                        {
                            newlines.Add(line);
                        }
                        line = reader.ReadLine();
                    }
            }
            return newlines;
        }

        public List<string> LoadHomeTopRow(string path)
        {
            int index = 0;
            List<string> newlines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Length > min && line.Length < max)
                    {
                        bool addline = true;
                        foreach (char c in line)
                        {
                            string test = "" + c;
                            Keys key = StringToKey(test);
                            if(IsKeyInList(bottomRow, key))
                            {
                                addline = false;
                                break;
                            }
                        }

                        if (addline)
                        {
                            newlines.Add(line);
                            index++;
                        }
                    }
                    line = reader.ReadLine();                    
                }
            }
            return newlines;
        }

        public List<string> LoadHomeBottomRow(string path)
        {
            int index = 0;
            List<string> newlines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Length > min && line.Length < max)
                    {
                        bool addline = true;
                        foreach (char c in line)
                        {
                            string test = "" + c;
                            Keys key = StringToKey(test);
                            if (IsKeyInList(topRow, key))
                            {
                                addline = false;
                                break;
                            }
                        }

                        if (addline)
                        {
                            newlines.Add(line);
                            index++;
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            return newlines;
        }

        public List<string> LoadHomeRow(string path)
        {
            int index = 0;
            List<string> newlines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Length > min && line.Length < max)
                    {
                        bool addline = true;
                        foreach (char c in line)
                        {
                            string test = "" + c;
                            Keys key = StringToKey(test);
                            if (IsKeyInList(topRow, key))
                            {
                                addline = false;
                                break;
                            }
                            if (IsKeyInList(bottomRow, key))
                            {
                                addline = false;
                                break;
                            }
                        }

                        if (addline)
                        {
                            newlines.Add(line);
                            index++;
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            return newlines;
        }

        public List<string> LoadAllRows(string path)
        {
            int index = 0;
            List<string> newlines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Length > min && line.Length < max)
                    {
                        newlines.Add(line);
                        index++;
                    }
                    line = reader.ReadLine();
                }
            }
            return newlines;
        }

        public void EstablishKeys()
        {
            topRow.Add(Keys.Q);
            topRow.Add(Keys.W);
            topRow.Add(Keys.E);
            topRow.Add(Keys.R);
            topRow.Add(Keys.T);
            topRow.Add(Keys.Y);
            topRow.Add(Keys.U);
            topRow.Add(Keys.I);
            topRow.Add(Keys.O);
            topRow.Add(Keys.P);

            homeRow.Add(Keys.A);
            homeRow.Add(Keys.S);
            homeRow.Add(Keys.D);
            homeRow.Add(Keys.F);
            homeRow.Add(Keys.G);
            homeRow.Add(Keys.H);
            homeRow.Add(Keys.J);
            homeRow.Add(Keys.K);
            homeRow.Add(Keys.L);

            bottomRow.Add(Keys.Z);
            bottomRow.Add(Keys.X);
            bottomRow.Add(Keys.C);
            bottomRow.Add(Keys.V);
            bottomRow.Add(Keys.B);
            bottomRow.Add(Keys.N);
            bottomRow.Add(Keys.M);
        }

        public bool IsKeyInList(List<Keys> keys, Keys key)
        {
            foreach (Keys inKey in keys)
            {
                if (inKey == key)
                {
                    return true;
                }
            }
            return false;
        }

        public Keys StringToKey(string s)
        {
            switch (s)
            {
                case "a":
                    return Keys.A;

                case "b":
                    return Keys.B;

                case "c":
                    return Keys.C;

                case "d":
                    return Keys.D;

                case "e":
                    return Keys.E;

                case "f":
                    return Keys.F;

                case "g":
                    return Keys.G;

                case "h":
                    return Keys.H;

                case "i":
                    return Keys.I;

                case "j":
                    return Keys.J;

                case "k":
                    return Keys.K;

                case "l":
                    return Keys.L;

                case "m":
                    return Keys.M;

                case "n":
                    return Keys.N;

                case "o":
                    return Keys.O;

                case "p":
                    return Keys.P;

                case "q":
                    return Keys.Q;

                case "r":
                    return Keys.R;

                case "s":
                    return Keys.S;

                case "t":
                    return Keys.T;

                case "u":
                    return Keys.U;

                case "v":
                    return Keys.V;

                case "w":
                    return Keys.W;

                case "x":
                    return Keys.X;

                case "y":
                    return Keys.Y;

                case "z":
                    return Keys.Z;

                default:
                    return Keys.Space;
            }
        }
    }
}