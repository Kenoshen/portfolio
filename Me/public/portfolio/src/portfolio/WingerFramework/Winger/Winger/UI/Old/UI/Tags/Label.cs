using Microsoft.Xna.Framework;

namespace Winger.UI.UI.Tags
{
    public class Label : UIObject
    {
        public override Vector2 GetSpriteOrigin(float width, float height)
        {
            if (Font == null || Text == null || Text == "")
                return Vector2.Zero;

            Vector2 str = Font.MeasureString(Text);
            float midW = width / 2f;
            float midH = height / 2f;

            switch (Alignment)
            {
                case Winger.UI.Helper.Alignment.CENTER:
                    return new Vector2(midW, midH);

                case Winger.UI.Helper.Alignment.LEFT:
                    return new Vector2(0, midH);

                case Winger.UI.Helper.Alignment.TOP_LEFT:
                    return Vector2.Zero;

                case Winger.UI.Helper.Alignment.TOP:
                    return new Vector2(midW, 0);

                case Winger.UI.Helper.Alignment.TOP_RIGHT:
                    return new Vector2(width, 0);

                case Winger.UI.Helper.Alignment.RIGHT:
                    return new Vector2(width, midH);

                case Winger.UI.Helper.Alignment.BOTTOM_RIGHT:
                    return new Vector2(width, height);

                case Winger.UI.Helper.Alignment.BOTTOM:
                    return new Vector2(midW, height);

                case Winger.UI.Helper.Alignment.BOTTOM_LEFT:
                    return new Vector2(0, height);
            }

            return Vector2.Zero;
        }
    }
}
