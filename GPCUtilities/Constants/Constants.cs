using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Constants
{
    public static class Constants
    {        
        /// <summary>
        /// Value of the gravity accelaration [mm/s^2]
        /// </summary>
        public const double GRAVITYACCELERATION = 9806.65;

        /// <summary>
        /// Seconds in a hour: 60 * 60 
        /// </summary>
        public const int SECONDSINAHOUR = 60 * 60;

        /// <summary>
        /// Seconds in a day: 24 * 60 * 60 
        /// </summary>
        public const int SECONDSINADAY = 24 * 60 * 60;

        /// <summary>
        /// Seconds in a year: 365 * 24 * 60 * 60 
        /// </summary>
        public const int SECONDSINAYEAR = 365 * 24 * 60 * 60;
    }
}
