using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace ParticleGame
{
    class Ledge
    {
        const float GOODPERCENTAGE = 0.1f;
        const float OKPERCENTAGE = 0.3f;
        const float YHITVALUE = 3f;

        public LedgeState State;

        public Vector2 LeftEdge;
        public Vector2 RightEdge;

        public Body Player;

        public float GoodLimit
        {
            get
            {
                return (RightEdge.X - LeftEdge.X) * GOODPERCENTAGE;
            }
        }

        public float OKLimit
        {
            get
            {
                return (RightEdge.X - LeftEdge.X) * OKPERCENTAGE;
            }
        }

        public Ledge(Body player)
        {
            Player = player;
            LeftEdge = Vector2.Zero;
            RightEdge = Vector2.Zero;

            State = LedgeState.USED;
        }

        public Ledge(Body player, Vector2 leftEdge, Vector2 rightEdge)
        {
            Player = player;
            LeftEdge = leftEdge;
            RightEdge = rightEdge;

            State = LedgeState.WAITING;
        }

        public void Update()
        {
            if (State == LedgeState.WAITING)
            {
                if (Player.Position.X >= LeftEdge.X && Player.Position.X <= RightEdge.X)
                {
                    if (Player.Position.Y >= LeftEdge.Y)
                    {
                        if (Player.Position.Y <= LeftEdge.Y + YHITVALUE)
                        {
                            if (Player.Position.X <= LeftEdge.X + GoodLimit)
                            {
                                State = LedgeState.HIT_GOOD;
                            }
                            else if (Player.Position.X <= LeftEdge.X + OKLimit)
                            {
                                State = LedgeState.HIT_OK;
                            }
                            else
                            {
                                State = LedgeState.HIT_BAD;
                            }
                        }
                    }
                }
            }
            else if (State == LedgeState.HIT_GOOD || State == LedgeState.HIT_OK || State == LedgeState.HIT_BAD)
            {
                State = LedgeState.USED;
            }
        }

        public void SetLedge(Vector2 leftEdge, Vector2 rightEdge)
        {
            LeftEdge = leftEdge;
            RightEdge = rightEdge;
            State = LedgeState.WAITING;
        }
    }

    public enum LedgeState
    {
        HIT_GOOD,
        HIT_OK,
        HIT_BAD,
        USED,
        WAITING,
    }
}
