using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Draw
{
    public class CSpriteBatch
    {
        private SpriteBatch spriteBatch;
        private Vector2 offset;
        private List<ToDrawObj> objs = new List<ToDrawObj>();
        private SpriteSortMode sortMode = SpriteSortMode.Deferred;
        private bool manualSorting = false;

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public float OffsetX
        {
            get { return offset.X; }
            set { offset.X = value; }
        }

        public float OffsetY
        {
            get { return offset.Y; }
            set { offset.Y = value; }
        }

        public CSpriteBatch(SpriteBatch spriteBatch)
        {
            offset = Vector2.Zero;
            this.spriteBatch = spriteBatch;
        }

        public void Begin()
        {
            manualSorting = false;
            spriteBatch.Begin();
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            this.sortMode = sortMode;
            if (sortMode == SpriteSortMode.BackToFront || sortMode == SpriteSortMode.FrontToBack)
            {
                manualSorting = true;
                spriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            }
            else
            {
                spriteBatch.Begin(sortMode, blendState);
            }
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            this.sortMode = sortMode;
            if (sortMode == SpriteSortMode.BackToFront || sortMode == SpriteSortMode.FrontToBack)
            {
                manualSorting = true;
                spriteBatch.Begin(SpriteSortMode.Deferred, blendState, samplerState, depthStencilState, rasterizerState);
            }
            else
            {
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState);
            }
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            this.sortMode = sortMode;
            if (sortMode == SpriteSortMode.BackToFront || sortMode == SpriteSortMode.FrontToBack)
            {
                manualSorting = true;
                spriteBatch.Begin(SpriteSortMode.Deferred, blendState, samplerState, depthStencilState, rasterizerState, effect);
            }
            else
            {
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
            }
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            this.sortMode = sortMode;
            if (sortMode == SpriteSortMode.BackToFront || sortMode == SpriteSortMode.FrontToBack)
            {
                manualSorting = true;
                spriteBatch.Begin(SpriteSortMode.Deferred, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            }
            else
            {
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            }
        }

        public void Dispose()
        {
            spriteBatch.Dispose();
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(destinationRectangle), color);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_DR_C;
                o.hasDepth = false;
                o.texture = texture;
                o.destinationRectangle = destinationRectangle;
                o.color = color;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(position), color);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_P_C;
                o.hasDepth = false;
                o.texture = texture;
                o.position = position;
                o.color = color;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Nullable<Rectangle> sourceRectangle, Color color)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(destinationRectangle), sourceRectangle, color);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_P_SR_C;
                o.hasDepth = false;
                o.texture = texture;
                o.destinationRectangle = destinationRectangle;
                o.sourceRectangle = sourceRectangle;
                o.color = color;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void Draw(Texture2D texture, Vector2 position, Nullable<Rectangle> sourceRectangle, Color color)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(position), sourceRectangle, color);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_P_SR_C;
                o.hasDepth = false;
                o.texture = texture;
                o.position = position;
                o.sourceRectangle = sourceRectangle;
                o.color = color;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Nullable<Rectangle> sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(destinationRectangle), sourceRectangle, color, rotation, origin, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_DR_SR_C_R_O_E_LD;
                o.hasDepth = true;
                o.texture = texture;
                o.destinationRectangle = destinationRectangle;
                o.sourceRectangle = sourceRectangle;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void Draw(Texture2D texture, Vector2 position, Nullable<Rectangle> sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(position), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_P_SR_C_R_O_SF_E_LD;
                o.hasDepth = true;
                o.texture = texture;
                o.position = position;
                o.sourceRectangle = sourceRectangle;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.scale_f = scale;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void Draw(Texture2D texture, Vector2 position, Nullable<Rectangle> sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.Draw(texture, GetWorldPosition(position), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.OBJECT_T_P_SR_C_R_O_SV_E_LD;
                o.hasDepth = true;
                o.texture = texture;
                o.position = position;
                o.sourceRectangle = sourceRectangle;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.scale_v = scale;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            if (!manualSorting)
            {
                spriteBatch.DrawString(spriteFont, text, GetWorldPosition(position), color);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.STRING_SF_TS_P_C;
                o.hasDepth = false;
                o.spriteFont = spriteFont;
                o.text_s = text;
                o.position = position;
                o.color = color;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            if (!manualSorting)
            {
                spriteBatch.DrawString(spriteFont, text, GetWorldPosition(position), color);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.STRING_SF_TSB_P_C;
                o.hasDepth = false;
                o.spriteFont = spriteFont;
                o.text_sb = text;
                o.position = position;
                o.color = color;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.DrawString(spriteFont, text, GetWorldPosition(position), color, rotation, origin, scale, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.STRING_SF_TS_P_C_R_O_SV_E_LD;
                o.hasDepth = true;
                o.spriteFont = spriteFont;
                o.text_s = text;
                o.position = position;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.scale_v = scale;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.DrawString(spriteFont, text, GetWorldPosition(position), color, rotation, origin, scale, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.STRING_SF_TS_P_C_R_O_SF_E_LD;
                o.hasDepth = true;
                o.spriteFont = spriteFont;
                o.text_s = text;
                o.position = position;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.scale_f = scale;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.DrawString(spriteFont, text, GetWorldPosition(position), color, rotation, origin, scale, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.STRING_SF_TSB_P_C_R_O_SV_E_LD;
                o.hasDepth = true;
                o.spriteFont = spriteFont;
                o.text_sb = text;
                o.position = position;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.scale_v = scale;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (!manualSorting)
            {
                spriteBatch.DrawString(spriteFont, text, GetWorldPosition(position), color, rotation, origin, scale, effects, layerDepth);
            }
            else
            {
                ToDrawObj o = new ToDrawObj();
                o.type = DrawType.STRING_SF_TSB_P_C_R_O_SF_E_LD;
                o.hasDepth = true;
                o.spriteFont = spriteFont;
                o.text_sb = text;
                o.position = position;
                o.color = color;
                o.rotation = rotation;
                o.origin = origin;
                o.scale_f = scale;
                o.effects = effects;
                o.layerDepth = layerDepth;
                o.offset = Offset;
                objs.Add(o);
            }
        }

        public void End()
        {
            if (!manualSorting)
            {
                spriteBatch.End();
            }
            else
            {
                SortAndEnd();
            }
        }

        public override bool Equals(object obj)
        {
            return spriteBatch.Equals(obj);
        }

        public override int GetHashCode()
        {
            return spriteBatch.GetHashCode();
        }

        public override string ToString()
        {
            return spriteBatch.ToString();
        }

        private Vector2 GetWorldPosition(Vector2 pos)
        {
            return GetWorldPosition(pos, Offset);
        }

        private Rectangle GetWorldPosition(Rectangle rect)
        {
            return GetWorldPosition(rect, Offset);
        }

        private Vector2 GetWorldPosition(Vector2 pos, Vector2 off)
        {
            Vector2 ret = new Vector2(pos.X, pos.Y);
            ret += off;
            return ret;
        }

        private Rectangle GetWorldPosition(Rectangle rect, Vector2 off)
        {
            Rectangle ret = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            ret.X += (int)off.X;
            ret.Y += (int)off.Y;
            return ret;
        }

        private void SortAndEnd()
        {
            List<ToDrawObj> layeredFinalSort = new List<ToDrawObj>();
            List<ToDrawObj> nonLayerFinalSort = new List<ToDrawObj>();

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i].hasDepth)
                {
                    if (layeredFinalSort.Count == 0)
                    {
                        layeredFinalSort.Add(objs[i]);
                    }
                    else
                    {
                        bool inserted = false;
                        for (int k = 0; k < layeredFinalSort.Count; k++)
                        {
                            if (objs[i].layerDepth <= layeredFinalSort[k].layerDepth)
                            {
                                layeredFinalSort.Insert(k, objs[i]);
                                inserted = true;
                                break;
                            }
                        }
                        if (!inserted)
                        {
                            layeredFinalSort.Add(objs[i]);
                        }
                    }
                }
                else
                {
                    nonLayerFinalSort.Add(objs[i]);
                }
            }

            if (sortMode == SpriteSortMode.BackToFront)
            {
                for (int i = layeredFinalSort.Count - 1; i >= 0; i--)
                {
                    DrawToDrawObj(layeredFinalSort[i]);
                }

                for (int i = nonLayerFinalSort.Count - 1; i >= 0; i--)
                {
                    DrawToDrawObj(nonLayerFinalSort[i]);
                }
            }
            else if (sortMode == SpriteSortMode.FrontToBack)
            {
                for (int i = 0; i < layeredFinalSort.Count; i++)
                {
                    DrawToDrawObj(layeredFinalSort[i]);
                }

                for (int i = 0; i < nonLayerFinalSort.Count; i++)
                {
                    DrawToDrawObj(nonLayerFinalSort[i]);
                }
            }

            spriteBatch.End();
        }

        private void DrawToDrawObj(ToDrawObj o)
        {
            switch (o.type)
            {
                case DrawType.OBJECT_T_DR_C:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.destinationRectangle, o.offset), o.color);
                    break;

                case DrawType.OBJECT_T_DR_SR_C:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.destinationRectangle, o.offset), o.sourceRectangle, o.color);
                    break;

                case DrawType.OBJECT_T_DR_SR_C_R_O_E_LD:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.destinationRectangle, o.offset), o.sourceRectangle, o.color, o.rotation, o.origin, o.effects, o.layerDepth);
                    break;

                case DrawType.OBJECT_T_P_C:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.position, o.offset), o.color);
                    break;

                case DrawType.OBJECT_T_P_SR_C:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.position, o.offset), o.sourceRectangle, o.color);
                    break;

                case DrawType.OBJECT_T_P_SR_C_R_O_SF_E_LD:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.position, o.offset), o.sourceRectangle, o.color, o.rotation, o.origin, o.scale_f, o.effects, o.layerDepth);
                    break;

                case DrawType.OBJECT_T_P_SR_C_R_O_SV_E_LD:
                    spriteBatch.Draw(o.texture, GetWorldPosition(o.position, o.offset), o.sourceRectangle, o.color, o.rotation, o.origin, o.scale_v, o.effects, o.layerDepth);
                    break;

                case DrawType.STRING_SF_TS_P_C:
                    spriteBatch.DrawString(o.spriteFont, o.text_s, GetWorldPosition(o.position, o.offset), o.color);
                    break;

                case DrawType.STRING_SF_TS_P_C_R_O_SF_E_LD:
                    spriteBatch.DrawString(o.spriteFont, o.text_s, GetWorldPosition(o.position, o.offset), o.color, o.rotation, o.origin, o.scale_f, o.effects, o.layerDepth);
                    break;

                case DrawType.STRING_SF_TS_P_C_R_O_SV_E_LD:
                    spriteBatch.DrawString(o.spriteFont, o.text_s, GetWorldPosition(o.position, o.offset), o.color, o.rotation, o.origin, o.scale_v, o.effects, o.layerDepth);
                    break;

                case DrawType.STRING_SF_TSB_P_C:
                    spriteBatch.DrawString(o.spriteFont, o.text_sb, GetWorldPosition(o.position, o.offset), o.color);
                    break;

                case DrawType.STRING_SF_TSB_P_C_R_O_SF_E_LD:
                    spriteBatch.DrawString(o.spriteFont, o.text_sb, GetWorldPosition(o.position, o.offset), o.color, o.rotation, o.origin, o.scale_f, o.effects, o.layerDepth);
                    break;

                case DrawType.STRING_SF_TSB_P_C_R_O_SV_E_LD:
                    spriteBatch.DrawString(o.spriteFont, o.text_sb, GetWorldPosition(o.position, o.offset), o.color, o.rotation, o.origin, o.scale_v, o.effects, o.layerDepth);
                    break;

                default:
                    break;
            }
        }

        private struct ToDrawObj
        {
            public DrawType type;
            public Texture2D texture;
            public Rectangle destinationRectangle;
            public Vector2 position;
            public Nullable<Rectangle> sourceRectangle;
            public Color color;
            public float rotation;
            public Vector2 origin;
            public float scale_f;
            public Vector2 scale_v;
            public SpriteEffects effects;
            public float layerDepth;
            public bool hasDepth;
            public SpriteFont spriteFont;
            public string text_s;
            public StringBuilder text_sb;
            public Vector2 offset;
        }

        private enum DrawType
        {
            OBJECT_T_DR_C,
            OBJECT_T_P_C,
            OBJECT_T_DR_SR_C,
            OBJECT_T_P_SR_C,
            OBJECT_T_DR_SR_C_R_O_E_LD,
            OBJECT_T_P_SR_C_R_O_SF_E_LD,
            OBJECT_T_P_SR_C_R_O_SV_E_LD,
            STRING_SF_TS_P_C,
            STRING_SF_TSB_P_C,
            STRING_SF_TS_P_C_R_O_SV_E_LD,
            STRING_SF_TS_P_C_R_O_SF_E_LD,
            STRING_SF_TSB_P_C_R_O_SV_E_LD,
            STRING_SF_TSB_P_C_R_O_SF_E_LD,
        }
    }
}
