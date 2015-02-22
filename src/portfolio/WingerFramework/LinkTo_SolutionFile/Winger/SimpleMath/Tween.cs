using System;
using Microsoft.Xna.Framework;

namespace Winger.SimpleMath
{
    public class Tween
    {
        public float CurrentTime { get; set; }
        public float Duration { get; set; }
        public float OriginX
        {
            get { return origin.X; }
            set { origin.X = value; }
        }
        public float OriginY
        {
            get { return origin.Y; }
            set { origin.Y = value; }
        }
        public float X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }
        public float Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }
        public float GoalX
        {
            get { return goal.X; }
            set { goal.X = value; }
        }
        public float GoalY
        {
            get { return goal.Y; }
            set { goal.Y = value; }
        }
        public TweenType TweenTypeX { get; set; }
        public TweenType TweenTypeY { get; set; }
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }
        public Vector2 Pos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }
        public Vector2 Goal
        {
            get
            {
                return goal;
            }
            set
            {
                goal = value;
            }
        }
        public bool StopOnGoal
        {
            get { return stopOnGoal; }
            set { stopOnGoal = value; }
        }
        public bool IsPaused
        {
            get { return pause; }
        }

        private Vector2 origin = Vector2.Zero;
        private Vector2 pos = Vector2.Zero;
        private Vector2 goal = Vector2.Zero;

        private bool pause = true;
        private bool stopOnGoal = true;
        private bool snappedToGoal = false;

        public void Start()
        {
            CurrentTime = 0;
            Origin = Pos;
            snappedToGoal = true;
            UnPause();
        }

        public void Pause()
        {
            pause = true;
        }

        public void UnPause()
        {
            pause = false;
        }

        public bool IsTweening()
        {
            return (CurrentTime < Duration);
        }

        public void Update(float timeChange)
        {
            if (!pause)
            {
                if (!StopOnGoal || CurrentTime <= Duration)
                {
                    X = GetTweenValue(TweenTypeX, CurrentTime, OriginX, GoalX - OriginX, Duration);
                    Y = GetTweenValue(TweenTypeY, CurrentTime, OriginY, GoalY - OriginY, Duration);
                }
                else if (snappedToGoal)
                {
                    snappedToGoal = false;
                    Pos = Goal;
                }
                CurrentTime = CurrentTime + timeChange;
            }
        }

        private float GetTweenValue(TweenType type, float currentTime, float startValue, float deltaValue, float duration)
        {
            switch (type)
            {
                case TweenType.LINEAR:
                    return Linear(currentTime, startValue, deltaValue, duration);

                case TweenType.QUADRATIC_IN:
                    return QuadraticIn(currentTime, startValue, deltaValue, duration);

                case TweenType.QUADRATIC_OUT:
                    return QuadraticOut(currentTime, startValue, deltaValue, duration);

                case TweenType.QUADRATIC_INOUT:
                    return QuadraticInOut(currentTime, startValue, deltaValue, duration);

                case TweenType.CUBIC_IN:
                    return CubicIn(currentTime, startValue, deltaValue, duration);

                case TweenType.CUBIC_OUT:
                    return CubicOut(currentTime, startValue, deltaValue, duration);

                case TweenType.CUBIC_INOUT:
                    return CubicInOut(currentTime, startValue, deltaValue, duration);

                case TweenType.QUARTIC_IN:
                    return QuarticIn(currentTime, startValue, deltaValue, duration);

                case TweenType.QUARTIC_OUT:
                    return QuarticOut(currentTime, startValue, deltaValue, duration);

                case TweenType.QUARTIC_INOUT:
                    return QuarticInOut(currentTime, startValue, deltaValue, duration);

                case TweenType.QUINTIC_IN:
                    return QuinticIn(currentTime, startValue, deltaValue, duration);

                case TweenType.QUINTIC_OUT:
                    return QuinticOut(currentTime, startValue, deltaValue, duration);

                case TweenType.QUINTIC_INOUT:
                    return QuinticInOut(currentTime, startValue, deltaValue, duration);

                case TweenType.SINUSOIDAL_IN:
                    return SinusoidalIn(currentTime, startValue, deltaValue, duration);

                case TweenType.SINUSOIDAL_OUT:
                    return SinusoidalOut(currentTime, startValue, deltaValue, duration);

                case TweenType.SINUSOIDAL_INOUT:
                    return SinusoidalInOut(currentTime, startValue, deltaValue, duration);

                case TweenType.EXPONENTIAL_IN:
                    return ExponentialIn(currentTime, startValue, deltaValue, duration);

                case TweenType.EXPONENTIAL_OUT:
                    return ExponentialOut(currentTime, startValue, deltaValue, duration);

                case TweenType.EXPONENTIAL_INOUT:
                    return ExponentialInOut(currentTime, startValue, deltaValue, duration);

                case TweenType.CIRCULAR_IN:
                    return CircularIn(currentTime, startValue, deltaValue, duration);

                case TweenType.CIRCULAR_OUT:
                    return CircularOut(currentTime, startValue, deltaValue, duration);

                case TweenType.CIRCULAR_INOUT:
                    return CircularInOut(currentTime, startValue, deltaValue, duration);

                default:
                    return Linear(currentTime, startValue, deltaValue, duration);
            }
        }


        #region Factory Methods

        // NOLONGERTODO: make this tween from an Element and put it in the Element class
        /*
        public static Tween TweenFromUIObject(UIObject obj)
        {
            if (obj.Tag.ToLower() != "tween" || obj == null)
                return null;
            Tween t = new Tween();
            try
            {
                object o = obj.Get("typex");
                if (o != null)
                    t.TweenTypeX = TweenTypeFromString((string)o);
            }
            catch (Exception) { }

            try
            {
                object o = obj.Get("typey");
                if (o != null)
                    t.TweenTypeY = TweenTypeFromString((string)o);
            }
            catch (Exception) { }

            try
            {
                object o = obj.Get("origin");
                if (o != null)
                    t.Origin = Winger.UI.Helper.Utils.Vector2FromString((string)o);
            }
            catch (Exception) { }

            try
            {
                object o = obj.Get("pos");
                if (o != null)
                    t.Pos = Winger.UI.Helper.Utils.Vector2FromString((string)o);
            }
            catch (Exception) { }

            try
            {
                object o = obj.Get("goal");
                if (o != null)
                    t.Goal = Winger.UI.Helper.Utils.Vector2FromString((string)o);
            }
            catch (Exception) { }

            try
            {
                object o = obj.Get("duration");
                if (o != null)
                    t.Duration = float.Parse((string)o);
            }
            catch (Exception) { }

            if (t.Pos == Vector2.Zero && t.Origin != Vector2.Zero)
                t.Pos = t.Origin;

            return t;
        }
        */

        private static TweenType TweenTypeFromString(string s)
        {
            s = s.ToUpper().Replace(" ", "").Replace("_", "");
            if (s == "0" || s == "LINEAR")
                return TweenType.LINEAR;
            else if (s == "1" || s == "QUADRATICIN")
                return TweenType.QUADRATIC_IN;
            else if (s == "2" || s == "QUADRATICOUT")
                return TweenType.QUADRATIC_OUT;
            else if (s == "3" || s == "QUADRATICINOUT")
                return TweenType.QUADRATIC_INOUT;
            else if (s == "4" || s == "CUBICIN")
                return TweenType.CUBIC_IN;
            else if (s == "5" || s == "CUBICOUT")
                return TweenType.CUBIC_OUT;
            else if (s == "6" || s == "CUBICINOUT")
                return TweenType.CUBIC_INOUT;
            else if (s == "7" || s == "QUARTICIN")
                return TweenType.QUARTIC_IN;
            else if (s == "8" || s == "QUARTICOUT")
                return TweenType.QUARTIC_OUT;
            else if (s == "9" || s == "QUARTICINOUT")
                return TweenType.QUARTIC_INOUT;
            else if (s == "10" || s == "QUINTICIN")
                return TweenType.QUINTIC_IN;
            else if (s == "11" || s == "QUINTICOUT")
                return TweenType.QUINTIC_OUT;
            else if (s == "12" || s == "QUINTICINOUT")
                return TweenType.QUINTIC_INOUT;
            else if (s == "13" || s == "SINUSOIDALIN")
                return TweenType.SINUSOIDAL_IN;
            else if (s == "14" || s == "SINUSOIDALOUT")
                return TweenType.SINUSOIDAL_OUT;
            else if (s == "15" || s == "SINUSOIDALINOUT")
                return TweenType.SINUSOIDAL_INOUT;
            else if (s == "16" || s == "EXPONENTIALIN")
                return TweenType.EXPONENTIAL_IN;
            else if (s == "17" || s == "EXPONENTIALOUT")
                return TweenType.EXPONENTIAL_OUT;
            else if (s == "18" || s == "EXPONENTIALINOUT")
                return TweenType.EXPONENTIAL_INOUT;
            else if (s == "19" || s == "CIRCULARIN")
                return TweenType.CIRCULAR_IN;
            else if (s == "20" || s == "CIRCULAROUT")
                return TweenType.CIRCULAR_OUT;
            else if (s == "21" || s == "CIRCULARINOUT")
                return TweenType.CIRCULAR_INOUT;
            else
                return TweenType.LINEAR;
        }

        #endregion

        #region Static Tween Methods

        /// <summary>
        /// No acceleration
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float Linear(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuadraticIn(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuadraticOut(float t, float b, float c, float d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuadraticInOut(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
                return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float CubicIn(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float CubicOut(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float CubicInOut(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
                return c / 2 * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuarticIn(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t * t + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuarticOut(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuarticInOut(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
                return c / 2 * t * t * t * t + b;
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuinticIn(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuinticOut(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t - 1) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float QuinticInOut(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
                return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float SinusoidalIn(float t, float b, float c, float d)
        {
            return -c * (float)Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float SinusoidalOut(float t, float b, float c, float d)
        {
            return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float SinusoidalInOut(float t, float b, float c, float d)
        {
            return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float ExponentialIn(float t, float b, float c, float d)
        {
            return c * (float)Math.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float ExponentialOut(float t, float b, float c, float d)
        {
            return c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float ExponentialInOut(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
                return c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
            t--;
            return c / 2 * (-(float)Math.Pow(2, -10 * t) + 2) + b;
        }

        /// <summary>
        /// Accelerate from zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float CircularIn(float t, float b, float c, float d)
        {
            t /= d;
            return -c * ((float)Math.Sqrt(1 - t * t) - 1) + b;
        }

        /// <summary>
        /// Deccelerate to zero
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float CircularOut(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (float)Math.Sqrt(1 - t * t) + b;
        }

        /// <summary>
        /// Accelerate half way then deccelerate the other half
        /// </summary>
        /// <param name="t">current time</param>
        /// <param name="b">origin</param>
        /// <param name="c">delta</param>
        /// <param name="d">duration</param>
        /// <returns>tweened value</returns>
        public static float CircularInOut(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
                return -c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b;
            t -= 2;
            return c / 2 * ((float)Math.Sqrt(1 - t * t) + 1) + b;
        }

        #endregion
    }
}
