using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Winger.SimpleMath
{
    public static class KinematicMath
    {
        #region Helpers
        public static float Pow(float num, float exponent)
        {
            return (float)Math.Pow(num, exponent);
        }

        public static float Sqrt(float num)
        {
            return (float)Math.Sqrt(num);
        }

        public static float Abs(float num)
        {
            return (float)Math.Abs(num);
        }
        #endregion

        #region 1D
        public static float DisplacementWithoutFinalVelocity(float initialVelocity, float time, float acceleration)
        {
            return (initialVelocity * time) + (0.5f * acceleration * Pow(time, 2));
        }

        public static float DisplacementWithoutInitialVelocity(float finalVelocity, float time, float acceleration)
        {
            float initialVelocity = InitialVelocityWithoutDisplacement(finalVelocity, time, acceleration);
            return DisplacementWithoutAcceleration(initialVelocity, finalVelocity, time);
        }

        public static float DisplacementWithoutAcceleration(float initialVelocity, float finalVelocity, float time)
        {
            return ((initialVelocity + finalVelocity) / 2f) + time;
        }

        public static float DisplacementWithoutTime(float initialVelocity, float finalVelocity, float acceleration)
        {
            if (acceleration == 0)
                return 0;
            return (Pow(finalVelocity, 2) - Pow(initialVelocity, 2)) / (2 * acceleration);
        }


        public static float FinalVelocityWithoutAcceleration(float initialVelocity, float time, float displacement)
        {
            if (time == 0)
                return 0;
            return ((displacement * 2) / time) - initialVelocity;
        }

        public static float FinalVelocityWithoutInitialVelocity(float time, float displacement, float acceleration)
        {
            float initialVelocity = InitialVelocityWithoutFinalVelocity(time, displacement, acceleration);
            return FinalVelocityWithoutDisplacement(initialVelocity, time, acceleration);
        }

        public static float FinalVelocityWithoutTime(float initialVelocity, float displacement, float acceleration)
        {
            return Sqrt(Abs((Pow(initialVelocity, 2)) + (2 * acceleration * displacement)));
        }

        public static float FinalVelocityWithoutDisplacement(float initialVelocity, float time, float acceleration)
        {
            return (initialVelocity) + (acceleration * time);
        }


        public static float InitialVelocityWithoutAcceleration(float finalVelocity, float time, float displacement)
        {
            if (time == 0)
                return 0;
            return ((displacement * 2) / time) - finalVelocity;
        }

        public static float InitialVelocityWithoutFinalVelocity(float time, float displacement, float acceleration)
        {
            if (time == 0)
                return 0;
            return (displacement - (0.5f * acceleration * Pow(time, 2))) / time;
        }

        public static float InitialVelocityWithoutTime(float finalVelocity, float displacement, float acceleration)
        {
            return Sqrt(Abs((Pow(finalVelocity, 2)) - (2 * acceleration * displacement)));
        }

        public static float InitialVelocityWithoutDisplacement(float finalVelocity, float time, float acceleration)
        {
            return (finalVelocity) - (acceleration * time);
        }


        public static float TimeWithoutAcceleration(float initialVelocity, float finalVelocity, float displacement)
        {
            if (initialVelocity + finalVelocity == 0)
                return 0;
            return displacement / ((initialVelocity + finalVelocity) / 2);
        }

        public static float TimeWithoutDisplacement(float initialVelocity, float finalVelocity, float acceleration)
        {
            if (acceleration == 0)
                return 0;
            return (finalVelocity - initialVelocity) / acceleration;
        }


        public static float AccelerationWithoutDisplacement(float initialVelocity, float finalVelocity, float time)
        {
            if (time == 0)
                return 0;
            return (finalVelocity - initialVelocity) / time;
        }

        public static float AccelerationWithoutTime(float initialVelocity, float finalVelocity, float displacement)
        {
            if (displacement == 0)
                return 0;
            return (Pow(finalVelocity, 2) - Pow(initialVelocity, 2)) / (2 * displacement);
        }

        public static float AccelerationWithoutInitialVelocity(float finalVelocity, float time, float displacement)
        {
            float initialVelocity = InitialVelocityWithoutAcceleration(finalVelocity, time, displacement);
            return AccelerationWithoutDisplacement(initialVelocity, finalVelocity, time);
        }

        public static float AccelerationWithoutFinalVelocity(float initialVelocity, float time, float displacement)
        {
            if (time == 0)
                return 0;
            return (displacement - (initialVelocity * time)) / (0.5f * Pow(time, 2));
        }
        #endregion

        #region 4D
        //public static Vector4 DisplacementWithoutFinalVelocity(Vector4 initialVelocity, float time, Vector4 acceleration)
        //{
        //    return new Vector4(
        //        DisplacementWithoutFinalVelocity(initialVelocity.X, time, acceleration.X), 
        //        DisplacementWithoutFinalVelocity(initialVelocity.Y, time, acceleration.Y), 
        //        DisplacementWithoutFinalVelocity(initialVelocity.Z, time, acceleration.Z), 
        //        DisplacementWithoutFinalVelocity(initialVelocity.W, time, acceleration.W));
        //}

        //public static Vector4 DisplacementWithoutInitialVelocity(Vector4 finalVelocity, float time, Vector4 acceleration)
        //{
        //    Vector4 initialVelocity = InitialVelocityWithoutDisplacement(finalVelocity, time, acceleration);
        //    return DisplacementWithoutAcceleration(initialVelocity, finalVelocity, time);
        //}

        //public static Vector4 DisplacementWithoutAcceleration(Vector4 initialVelocity, Vector4 finalVelocity, float time)
        //{
        //    return new Vector4(
        //        DisplacementWithoutAcceleration(initialVelocity.X, finalVelocity.X, time),
        //        DisplacementWithoutAcceleration(initialVelocity.Y, finalVelocity.Y, time),
        //        DisplacementWithoutAcceleration(initialVelocity.Z, finalVelocity.Z, time),
        //        DisplacementWithoutAcceleration(initialVelocity.W, finalVelocity.W, time));
        //}

        //public static Vector4 DisplacementWithoutTime(Vector4 initialVelocity, Vector4 finalVelocity, Vector4 acceleration)
        //{
        //    return new Vector4(
        //        DisplacementWithoutTime(initialVelocity.X, finalVelocity.X, acceleration.X),
        //        DisplacementWithoutTime(initialVelocity.Y, finalVelocity.Y, acceleration.Y),
        //        DisplacementWithoutTime(initialVelocity.Z, finalVelocity.Z, acceleration.Z),
        //        DisplacementWithoutTime(initialVelocity.W, finalVelocity.W, acceleration.W));
        //}


        //public static Vector4 FinalVelocityWithoutAcceleration(Vector4 initialVelocity, float time, Vector4 displacement)
        //{
        //    return new Vector4(
        //        FinalVelocityWithoutAcceleration(initialVelocity.X, time, displacement.X),
        //        FinalVelocityWithoutAcceleration(initialVelocity.Y, time, displacement.Y),
        //        FinalVelocityWithoutAcceleration(initialVelocity.Z, time, displacement.Z),
        //        FinalVelocityWithoutAcceleration(initialVelocity.W, time, displacement.W));
        //}

        //public static Vector4 FinalVelocityWithoutInitialVelocity(float time, Vector4 displacement, Vector4 acceleration)
        //{
        //    Vector4 initialVelocity = InitialVelocityWithoutFinalVelocity(time, displacement, acceleration);
        //    return FinalVelocityWithoutDisplacement(initialVelocity, time, acceleration);
        //}

        //public static Vector4 FinalVelocityWithoutTime(Vector4 initialVelocity, Vector4 displacement, Vector4 acceleration)
        //{
        //    return new Vector4(
        //        FinalVelocityWithoutTime(initialVelocity.X, displacement.X, acceleration.X), 
        //        FinalVelocityWithoutTime(initialVelocity.Y, displacement.Y, acceleration.Y), 
        //        FinalVelocityWithoutTime(initialVelocity.Z, displacement.Z, acceleration.Z), 
        //        FinalVelocityWithoutTime(initialVelocity.W, displacement.W, acceleration.W));
        //}

        //public static Vector4 FinalVelocityWithoutDisplacement(Vector4 initialVelocity, float time, Vector4 acceleration)
        //{
        //    return new Vector4(
        //        FinalVelocityWithoutDisplacement(initialVelocity.X, time, acceleration.X),
        //        FinalVelocityWithoutDisplacement(initialVelocity.Y, time, acceleration.Y),
        //        FinalVelocityWithoutDisplacement(initialVelocity.Z, time, acceleration.Z),
        //        FinalVelocityWithoutDisplacement(initialVelocity.W, time, acceleration.W));
        //}


        //public static Vector4 InitialVelocityWithoutAcceleration(Vector4 finalVelocity, float time, Vector4 displacement)
        //{
        //    return new Vector4(
        //        FinalVelocityWithoutAcceleration(finalVelocity.X, time, displacement.X),
        //        FinalVelocityWithoutAcceleration(finalVelocity.Y, time, displacement.Y),
        //        FinalVelocityWithoutAcceleration(finalVelocity.Z, time, displacement.Z),
        //        FinalVelocityWithoutAcceleration(finalVelocity.W, time, displacement.W));
        //}

        //public static Vector4 InitialVelocityWithoutFinalVelocity(float time, Vector4 displacement, Vector4 acceleration)
        //{
        //    if (time == 0)
        //        return 0;
        //    return (displacement - (0.5f * acceleration * Pow(time, 2))) / time;
        //}

        //public static Vector4 InitialVelocityWithoutTime(Vector4 finalVelocity, Vector4 displacement, Vector4 acceleration)
        //{
        //    return Sqrt(Abs((Pow(finalVelocity, 2)) - (2 * acceleration * displacement)));
        //}

        //public static Vector4 InitialVelocityWithoutDisplacement(Vector4 finalVelocity, float time, Vector4 acceleration)
        //{
        //    return (finalVelocity) - (acceleration * time);
        //}


        //public static float TimeWithoutAcceleration(Vector4 initialVelocity, Vector4 finalVelocity, Vector4 displacement)
        //{
        //    if (initialVelocity + finalVelocity == 0)
        //        return 0;
        //    return displacement / ((initialVelocity + finalVelocity) / 2);
        //}

        //public static float TimeWithoutDisplacement(Vector4 initialVelocity, Vector4 finalVelocity, Vector4 acceleration)
        //{
        //    if (acceleration == 0)
        //        return 0;
        //    return (finalVelocity - initialVelocity) / acceleration;
        //}


        //public static Vector4 AccelerationWithoutDisplacement(Vector4 initialVelocity, Vector4 finalVelocity, float time)
        //{
        //    if (time == 0)
        //        return 0;
        //    return (finalVelocity - initialVelocity) / time;
        //}

        //public static Vector4 AccelerationWithoutTime(Vector4 initialVelocity, Vector4 finalVelocity, Vector4 displacement)
        //{
        //    if (displacement == 0)
        //        return 0;
        //    return (Pow(finalVelocity, 2) - Pow(initialVelocity, 2)) / (2 * displacement);
        //}

        //public static Vector4 AccelerationWithoutInitialVelocity(Vector4 finalVelocity, float time, Vector4 displacement)
        //{
        //    float initialVelocity = InitialVelocityWithoutAcceleration(finalVelocity, time, displacement);
        //    return AccelerationWithoutDisplacement(initialVelocity, finalVelocity, time);
        //}

        //public static Vector4 AccelerationWithoutFinalVelocity(Vector4 initialVelocity, float time, Vector4 displacement)
        //{
        //    if (time == 0)
        //        return 0;
        //    return (displacement - (initialVelocity * time)) / (0.5f * Pow(time, 2));
        //}
        #endregion
    }
}

