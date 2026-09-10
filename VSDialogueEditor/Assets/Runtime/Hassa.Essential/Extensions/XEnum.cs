using System;

namespace Hassa.Essentials
{
    public static class XEnum
    {

        /// <summary>
        /// Convert Enum value to Int32.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns enum's value as integer.</returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

    }
}
