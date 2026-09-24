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

            multiple = Math.Abs(multiple);
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
        /// <param name="digits">The number of digits of the returned value. If negative (default) no further rounding is applied</param>
        /// <returns>The rounded value</returns>
        /// <remarks>The value is rounded towards positive infinity (e.g. 0.23 -> 0.25 and -0.23 -> -0.20 with multiple 0.05).
        /// A value that is already a multiple, apart from floating point noise, is returned unchanged</remarks>
        public static double RoundToMultiple(this double value, double multiple, int digits = -1)
        {
            if (multiple == 0 || double.IsNaN(value) || double.IsInfinity(value))
                return value;

            multiple = Math.Abs(multiple);

            double quotient = value / multiple;
            double nearest = Math.Round(quotient);
            double count = Math.Abs(quotient - nearest) < 1e-9 ? nearest : Math.Ceiling(quotient);

            double result;
            try
            {
                // decimal avoids results like 0.30000000000000004
                result = (double)((decimal)count * (decimal)multiple);
            }
            catch (OverflowException)
            {
                result = count * multiple;
            }

            return digits >= 0 ? Math.Round(result, digits) : result;
        }

        #endregion
    }
}
