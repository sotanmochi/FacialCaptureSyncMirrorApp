using System;

namespace FacialCaptureSync
{
    public static class MathUtils
    {
        /// <summary>
        /// Returns the smallest power of two that is greater than or equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int CeilingPowerOfTwo(int value)
        {
            var power = (int)Math.Ceiling(Math.Log(value, 2));
            return (int)Math.Pow(2, power);
        }
    }
}