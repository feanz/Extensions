using System;

namespace Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        ///   Method that finds a digit at an arbirary position of a decimal. <param name="number"> The number. </param> <param
        ///    name="position"> The position of the digit to the right of the decimal (1-n). </param> <returns> Digit at position </returns>
        ///   <example>
        ///     var number = 1.2459m; var digit = number.DigitAtPosition(3); // value is 5
        ///   </example>
        ///   <remarks>
        ///     See also http://stackoverflow.com/questions/2923510/what-is-the-best-way-to-find-the-digit-at-n-position-in-a-decimal-number/2924042#2924042.
        ///   </remarks>
        /// </summary>
        public static int DigitAtPosition(this decimal number, int position)
        {
            if (position <= 0)
            {
                throw new ArgumentException("Position must be positive.");
            }

            if (number < 0)
            {
                number = Math.Abs(number);
            }

            return number.SanitizedDigitAtPosition(position);
        }

        /// <summary>
        ///   Ensures that the specified decimal is positive
        /// </summary>
        /// <param name="number"> The decimal we are ensuring is positive. </param>
        /// <returns> </returns>
        public static decimal EnsurePositive(this decimal number)
        {
            return (number < 0 ? (number*-1M) : number);
        }

        private static int SanitizedDigitAtPosition(this decimal sanitizedNumber,
                                                    int validPosition)
        {
            sanitizedNumber -= Math.Floor(sanitizedNumber);

            if (sanitizedNumber == 0)
            {
                return 0;
            }

            if (validPosition == 1)
            {
                return (int) (sanitizedNumber*10);
            }

            return (sanitizedNumber*10).SanitizedDigitAtPosition(validPosition - 1);
        }
    }
}