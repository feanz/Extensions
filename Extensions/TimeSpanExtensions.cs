using System;

namespace Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        ///   Determines whether [is midnight exactly] [the specified t].
        /// </summary>
        /// <param name="t"> The t. </param>
        /// <returns> <c>true</c> if [is midnight exactly] [the specified t]; otherwise, <c>false</c> . </returns>
        public static bool IsMidnightExactly(this TimeSpan t)
        {
            return t.Hours == 0 && t.Minutes == 0 && t.Seconds == 0;
        }
    }
}