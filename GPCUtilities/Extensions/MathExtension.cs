using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Extensions
{
    public static class MathExtension
    {
        #region Radians to Degrees

        // Double
        public static double ToRadians(this double degreesAngle)
        {
            return (Math.PI / 180.0) * degreesAngle;
        }

        public static double ToDegrees(this double radiansAngle)
        {
            return (180.0 / Math.PI) * radiansAngle;
        }

        // Float
        public static double ToRadians(this float degreesAngle)
        {
            return (Math.PI / 180.0) * degreesAngle;
        }

        public static double ToDegrees(this float radiansAngle)
        {
            return (180.0 / Math.PI) * radiansAngle;
        }

        // Decimal
        public static decimal ToRadians(this decimal degreesAngle)
        {
            return (decimal)(Math.PI / 180.0) * degreesAngle;
        }

        public static decimal ToDegrees(this decimal radiansAngle)
        {
            return (decimal)(180.0 / Math.PI) * radiansAngle;
        }

        // Int
        public static double ToRadians(this int degreesAngle)
        {
            return (Math.PI / 180.0) * degreesAngle;
        }

        public static double ToDegrees(this int radiansAngle)
        {
            return (180.0 / Math.PI) * radiansAngle;
        }

        // Long
        public static double ToRadians(this long degreesAngle)
        {
            return (Math.PI / 180.0) * degreesAngle;
        }

        public static double ToDegrees(this long radiansAngle)
        {
            return (180.0 / Math.PI) * radiansAngle;
        }

        // Long
        public static double ToRadians(this short degreesAngle)
        {
            return (Math.PI / 180.0) * degreesAngle;
        }

        public static double ToDegrees(this short radiansAngle)
        {
            return (180.0 / Math.PI) * radiansAngle;
        }

        #endregion

        #region Rounding

        /// <summary>
        /// Rounds a number to the next multiple of another number
        /// </summary>
        /// <param name="value">The value to be rounded</param>
        /// <param name="multiple">The multiple to which the value should be rounded</param>
        /// <returns>The rounded value</returns>
        public static int RoundToMultiple(this int value, int multiple)
        {
            if (multiple == 0)
                return value;

            int reminder = Math.Abs(value) % multiple;
            if (reminder == 0)
                return value;

            if (value < 0)
                return -(Math.Abs(value) - reminder);
            return value + multiple - reminder;
        }

        /// <summary>
        /// Rounds a number to the next multiple of another number
        /// </summary>
        /// <param name="value">The value to be rounded</param>
        /// <param name="multiple">The multiple to which the value should be rounded</param>
        /// <param name="digits">The number of digits of the returned value</param>
        /// <returns>The rounded value</returns>
        public static double RoundToMultiple(this double value, double multiple, int digits = 0)
        {
            if (multiple == 0)
                return value;

            double reminder = Math.Abs(value) % multiple;
            if (reminder == 0)
                return value;

            if (value < 0)
                return -Math.Round(Math.Abs(value) - reminder, digits);
            return Math.Round(value + multiple - reminder, digits);
        }

        #endregion
    }
}
