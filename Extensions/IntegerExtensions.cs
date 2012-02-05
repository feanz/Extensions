using System;
using System.Globalization;

namespace Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        ///   Ensures that the specified integer is positive
        /// </summary>
        /// <param name="number"> The integer we are ensuring is positive. </param>
        /// <returns> Positive Integer. </returns>
        public static int EnsurePositive(this int number)
        {
            return (number < 0 ? (number*-1) : number);
        }

        /// <summary>
        ///   Ensures that the specified float is positive
        /// </summary>
        /// <param name="number"> The float we are ensuring is positive </param>
        /// <returns> </returns>
        public static float EnsurePositive(this float number)
        {
            return (number < 0 ? (number*-1F) : number);
        }

        /// <summary>
        ///   Convert integer to string with a minimum length padding with "0" if int is to short.
        /// </summary>
        /// <param name="number"> The number we are coverting to a fixed string length. </param>
        /// <param name="length"> The length to fix the integer to. </param>
        /// <returns> The new fixed length string </returns>
        public static string ForceLength(this int number, int length)
        {
            string s = number.ToString();
            string returnString = "";

            for (int i = s.Length; i < length; i++)
            {
                returnString += "0";
            }

            return returnString + s;
        }

        /// <summary>
        ///   Returns true if number is even
        /// </summary>
        /// <param name="value"> The value to check. </param>
        /// <returns> <c>true</c> If number is even. <c>If numbner is odd.</c> </returns>
        public static bool IsEven(this int value)
        {
            return value%2 == 0;
        }

        /// <summary>
        ///   Returns true if number is odd
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> <c>true</c> If number is odd. <c>If numbner is even.</c> </returns>
        public static bool IsOdd(this int value)
        {
            return value%2 != 0;
        }

        /// <summary>
        ///   Returns a binary string representation of the number.
        /// </summary>
        /// <param name="number"> The integer number to convert. </param>
        /// <returns> A binary string of the number provided. </returns>
        public static string ToBinary(this int number)
        {
            return Convert.ToString(number, 2);
        }

        /// <summary>
        ///   Returns a hexadecimal string representation of the number.
        /// </summary>
        /// <param name="number"> The integer number to convert. </param>
        /// <returns> A hex string of the number provided </returns>
        public static string ToHex(this int number)
        {
            return Convert.ToString(number, 16);
        }

        /// <summary>
        ///   Convert an nibble value(4 bit) integer to its corresponding hex value
        /// </summary>
        /// <param name="nibble"> </param>
        /// <returns> </returns>
        public static int ToHexChar(this int nibble)
        {
            if (nibble < 10)
                return nibble + 48;

            return nibble + 55;
        }

        /// <summary>
        ///   Returns a string representation using the Current culture
        /// </summary>
        /// <param name="instance"> </param>
        /// <returns> </returns>
        public static string ToStringCurrentCulture(this int instance)
        {
            return instance.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   Returns a string representation using an invariant culture
        /// </summary>
        /// <param name="instance"> </param>
        /// <returns> </returns>
        public static string ToStringInvariantCulture(this int instance)
        {
            return instance.ToString(CultureInfo.InvariantCulture);
        }

        #region DateTime

        /// <summary>
        ///   Returns a date in the past by given number of days.
        /// </summary>
        /// <param name="days"> The days. </param>
        /// <returns> </returns>
        public static DateTime DaysAgo(this int days)
        {
            TimeSpan t = new TimeSpan(days, 0, 0, 0);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        ///   Returns a date in the future by days.
        /// </summary>
        /// <param name="days"> The days. </param>
        /// <returns> </returns>
        public static DateTime DaysFromNow(this int days)
        {
            TimeSpan t = new TimeSpan(days, 0, 0, 0);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        ///   Returns a date in the past by hours.
        /// </summary>
        /// <param name="hours"> The hours. </param>
        /// <returns> </returns>
        public static DateTime HoursAgo(this int hours)
        {
            TimeSpan t = new TimeSpan(hours, 0, 0);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        ///   Returns a date in the future by hours.
        /// </summary>
        /// <param name="hours"> The hours. </param>
        /// <returns> </returns>
        public static DateTime HoursFromNow(this int hours)
        {
            TimeSpan t = new TimeSpan(hours, 0, 0);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        ///   Returns a date in the past by minutes
        /// </summary>
        /// <param name="minutes"> The minutes. </param>
        /// <returns> </returns>
        public static DateTime MinutesAgo(this int minutes)
        {
            TimeSpan t = new TimeSpan(0, minutes, 0);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        ///   Returns a date in the future by minutes.
        /// </summary>
        /// <param name="minutes"> The minutes. </param>
        /// <returns> </returns>
        public static DateTime MinutesFromNow(this int minutes)
        {
            TimeSpan t = new TimeSpan(0, minutes, 0);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        ///   Gets a date in the past according to seconds
        /// </summary>
        /// <param name="seconds"> The seconds. </param>
        /// <returns> </returns>
        public static DateTime SecondsAgo(this int seconds)
        {
            TimeSpan t = new TimeSpan(0, 0, seconds);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        ///   Gets a date in the future by seconds.
        /// </summary>
        /// <param name="seconds"> The seconds. </param>
        /// <returns> </returns>
        public static DateTime SecondsFromNow(this int seconds)
        {
            TimeSpan t = new TimeSpan(0, 0, seconds);
            return DateTime.Now.Add(t);
        }

        #endregion

        #region TimeSpan

        /// <summary>
        ///   Converts the number to days as a TimeSpan.
        /// </summary>
        /// <param name="num"> Number representing days </param>
        /// <returns> </returns>
        public static TimeSpan Days(this int num)
        {
            return new TimeSpan(num, 0, 0, 0);
        }

        /// <summary>
        ///   Converts the number to hours as a TimeSpan
        /// </summary>
        /// <param name="num"> Number representing hours </param>
        /// <returns> </returns>
        public static TimeSpan Hours(this int num)
        {
            return new TimeSpan(0, num, 0, 0);
        }

        /// <summary>
        ///   Converts the number of minutes as a TimeSpan
        /// </summary>
        /// <param name="num"> Number representing minutes </param>
        /// <returns> </returns>
        public static TimeSpan Minutes(this int num)
        {
            return new TimeSpan(0, 0, num, 0);
        }

        /// <summary>
        ///   Converts the number to seconds as a TimeSpan
        /// </summary>
        /// <param name="num"> </param>
        /// <returns> </returns>
        public static TimeSpan Seconds(this int num)
        {
            return new TimeSpan(0, 0, 0, num);
        }

        /// <summary>
        ///   Converts the number to a TimeSpan.
        /// </summary>
        /// <param name="num"> Number reprsenting a timespan </param>
        /// <returns> </returns>
        public static TimeSpan Time(this int num)
        {
            return Time(num, false);
        }

        /// <summary>
        ///   Converts the military time to a timespan.
        /// </summary>
        /// <param name="num"> </param>
        /// <param name="convertSingleDigitsToHours"> Indicates whether to treat "9" as 9 hours instead of minutes. </param>
        /// <returns> </returns>
        public static TimeSpan Time(this int num, bool convertSingleDigitsToHours)
        {
            TimeSpan time = TimeSpan.MinValue;
            if (convertSingleDigitsToHours)
            {
                if (num <= 24)
                    num *= 100;
            }
            int hours = num/100;
            int hour = hours;
            int minutes = num%100;

            time = new TimeSpan(hours, minutes, 0);
            return time;
        }

        #endregion
    }
}