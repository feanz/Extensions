using System;
using System.Globalization;

namespace Extensions
{
    public static class DateExtensions
    {
        public const string AGO = "ago";
        public const string DAY = "day";
        public const string HOUR = "hour";
        public const string MINUTE = "minute";
        public const string MONTH = "month";
        public const string SECOND = "second";
        public const string SPACE = " ";
        public const string YEAR = "year";

        /// <summary>
        ///   Diffs the specified date.
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The time span between the two dates </returns>
        public static TimeSpan Diff(this DateTime dateOne, DateTime dateTwo)
        {
            TimeSpan t = dateOne.Subtract(dateTwo);
            return t;
        }

        /// <summary>
        ///   Returns a double indicating the number of days between two dates (past is negative)
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The number of days between the two dates. </returns>
        public static double DiffDays(this string dateOne, string dateTwo)
        {
            DateTime dtOne;
            DateTime dtTwo;
            if (DateTime.TryParse(dateOne, out dtOne) && DateTime.TryParse(dateTwo, out dtTwo))
                return Diff(dtOne, dtTwo).TotalDays;
            return 0;
        }

        /// <summary>
        ///   Returns a double indicating the number of days between two dates (past is negative)
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The number of days between the two dates </returns>
        public static double DiffDays(this DateTime dateOne, DateTime dateTwo)
        {
            return Diff(dateOne, dateTwo).TotalDays;
        }

        /// <summary>
        ///   Returns a double indicating the number of days between two dates (past is negative)
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The number of hours between the two dates. </returns>
        public static double DiffHours(this string dateOne, string dateTwo)
        {
            DateTime dtOne;
            DateTime dtTwo;
            if (DateTime.TryParse(dateOne, out dtOne) && DateTime.TryParse(dateTwo, out dtTwo))
                return Diff(dtOne, dtTwo).TotalHours;
            return 0;
        }

        /// <summary>
        ///   Returns a double indicating the number of days between two dates (past is negative)
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The number of hours between the two dates. </returns>
        public static double DiffHours(this DateTime dateOne, DateTime dateTwo)
        {
            return Diff(dateOne, dateTwo).TotalHours;
        }

        /// <summary>
        ///   Returns a double indicating the number of days between two dates (past is negative)
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The number of mins between the two dates. </returns>
        public static double DiffMinutes(this string dateOne, string dateTwo)
        {
            DateTime dtOne;
            DateTime dtTwo;
            if (DateTime.TryParse(dateOne, out dtOne) && DateTime.TryParse(dateTwo, out dtTwo))
                return Diff(dtOne, dtTwo).TotalMinutes;
            return 0;
        }

        /// <summary>
        ///   Returns a double indicating the number of days between two dates (past is negative)
        /// </summary>
        /// <param name="dateOne"> The date one. </param>
        /// <param name="dateTwo"> The date two. </param>
        /// <returns> The number of mins between the two dates. </returns>
        public static double DiffMinutes(this DateTime dateOne, DateTime dateTwo)
        {
            return Diff(dateOne, dateTwo).TotalMinutes;
        }

        /// <summary>
        ///   Given a datetime object, returns the formatted day, "15th"
        /// </summary>
        /// <param name="date"> The date to extract the string from </param>
        /// <returns> </returns>
        public static string GetDateDayWithSuffix(this DateTime date)
        {
            int dayNumber = date.Day;
            string suffix = "th";

            if (dayNumber == 1 || dayNumber == 21 || dayNumber == 31)
                suffix = "st";
            else if (dayNumber == 2 || dayNumber == 22)
                suffix = "nd";
            else if (dayNumber == 3 || dayNumber == 23)
                suffix = "rd";

            return String.Concat(dayNumber, suffix);
        }

        /// <summary>
        ///   Given a datetime object, returns the formatted month and day, i.e. "April 15th"
        /// </summary>
        /// <param name="date"> The date to extract the string from </param>
        /// <returns> </returns>
        public static string GetFormattedMonthAndDay(this DateTime date)
        {
            return String.Concat(String.Format(CultureInfo.CurrentCulture, "{0:MMMM}", date), " ",
                                 GetDateDayWithSuffix(date));
        }

        /// <summary>
        ///   Determines whether [is last day of month] [the specified date].
        /// </summary>
        /// <param name="date"> The date. </param>
        /// <returns> <c>true</c> if [is last day of month] [the specified date]; otherwise, <c>false</c> . </returns>
        public static bool IsLastDayOfMonth(this DateTime date)
        {
            int lastDayOfMonth = LastDayOfMonth(date);
            return lastDayOfMonth == date.Day;
        }

        /// <summary>
        ///   Determines whether [is leap year] [the specified date].
        /// </summary>
        /// <param name="date"> The date. </param>
        /// <returns> <c>true</c> if [is leap year] [the specified date]; otherwise, <c>false</c> . </returns>
        public static bool IsLeapYear(this DateTime date)
        {
            return date.Year%4 == 0 && (date.Year%100 != 0 || date.Year%400 == 0);
        }

        /// <summary>
        ///   Checks to see if the date is a week day (Mon - Fri)
        /// </summary>
        /// <param name="dt"> The dt. </param>
        /// <returns> <c>true</c> if [is week day] [the specified dt]; otherwise, <c>false</c> . </returns>
        public static bool IsWeekDay(this DateTime dt)
        {
            return (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday);
        }

        /// <summary>
        ///   Determines whether the specified date is a weekend.
        /// </summary>
        /// <param name="source"> Source date </param>
        /// <returns> <c>true</c> if the specified source is a weekend; otherwise, <c>false</c> . </returns>
        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday ||
                   source.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        ///   Gets the Last the day of month.
        /// </summary>
        /// <param name="date"> The date. </param>
        /// <returns> </returns>
        public static int LastDayOfMonth(this DateTime date)
        {
            if (IsLeapYear(date) && date.Month == 2) return 28;
            if (date.Month == 2) return 27;
            if (date.Month == 1 || date.Month == 3 || date.Month == 5 || date.Month == 7
                || date.Month == 8 || date.Month == 10 || date.Month == 12)
                return 31;
            return 30;
        }

        /// <summary>
        ///   Returns a new instance of DateTime with a different day of the month.
        /// </summary>
        /// <param name="source"> Base DateTime object to modify </param>
        /// <param name="day"> Day of the month (1-31) </param>
        /// <returns> Instance of DateTime with specified day </returns>
        public static DateTime SetDay(this DateTime source, int day)
        {
            return new DateTime(source.Year, source.Month, day);
        }

        /// <summary>
        ///   Returns a new instance of DateTime with a different month.
        /// </summary>
        /// <param name="source"> Base DateTime object to modify </param>
        /// <param name="month"> The month as an integer (1-12) </param>
        /// <returns> Instance of DateTime with specified month </returns>
        public static DateTime SetMonth(this DateTime source, int month)
        {
            return new DateTime(source.Year, month, source.Day);
        }

        /// <summary>
        ///   Returns a new instance of DateTime with a different year.
        /// </summary>
        /// <param name="source"> Base DateTime object to modify </param>
        /// <param name="year"> The year </param>
        /// <returns> Instance of DateTime with specified year </returns>
        public static DateTime SetYear(this DateTime source, int year)
        {
            return new DateTime(year, source.Month, source.Day);
        }

        /// <summary>
        ///   Returns a datetime formatted as a string in yyyyMMddhhmm
        /// </summary>
        /// <param name="dt"> The datetime to convert </param>
        /// <returns> </returns>
        public static string ToMigrationString(this DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture);
        }
    }
}