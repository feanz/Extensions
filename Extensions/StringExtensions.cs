using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class StringExtensions
    {
        #region Boolean

        /// <summary>
        ///   Validate that string contains string case insensitive search
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="contains"> </param>
        /// <returns> </returns>
        public static bool ContainsCaseInsensitive(this string text, string contains)
        {
            return text.IndexOf(contains, StringComparison.OrdinalIgnoreCase) != -1;
        }

        /// <summary>
        ///   Verifies that the source text does not equal the specified value using the specified string comparison
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="value"> </param>
        /// <param name="comparison"> </param>
        /// <returns> </returns>
        public static bool DoesNotEqual(this string text, string value,
                                        StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            return !string.Equals(text, value, comparison);
        }

        /// <summary>
        ///   Validate if string is a valide binary representation
        /// </summary>
        /// <param name="binary"> </param>
        /// <returns> </returns>
        public static bool IsBinary(this string binary)
        {
            bool isValidBinary = true;
            var chars = binary.ToCharArray();

            foreach (char t in chars)
            {
                isValidBinary = (t == '0' || t == '1');
            }
            return isValidBinary;
        }

        /// <summary>
        ///   Validate is string is a valide hex representation
        /// </summary>
        /// <param name="hex"> </param>
        /// <returns> </returns>
        public static bool IsHex(this string hex)
        {
            var chars = hex.ToCharArray();
            bool isHexChar = true;
            foreach (char t in chars)
            {
                isHexChar = (t >= '0' && t <= '9') ||
                            (t >= 'a' && t <= 'f') ||
                            (t >= 'A' && t <= 'F');
            }
            return isHexChar;
        }

        /// <summary>
        ///   Verifies that the source text is not null or empty
        /// </summary>
        /// <param name="text"> </param>
        /// <returns> </returns>
        public static bool IsNotNullOrEmpty(this string text)
        {
            return !text.IsNullOrEmpty();
        }

        /// <summary>
        ///   Verifies that the source text is null or empty
        /// </summary>
        /// <param name="text"> </param>
        /// <returns> </returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        ///   Compares two string case insensitive
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="compare"> </param>
        /// <returns> </returns>
        public static bool IsSameAs(this string text, string compare)
        {
            return String.Equals(text, compare, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   Compare strings trimmed
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="compare"> </param>
        /// <returns> </returns>
        public static bool IsSameAsTrimmed(this string text, string compare)
        {
            return String.Equals(text.Trim(), compare.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   Determines if a string is unicode
        /// </summary>
        /// <param name="text"> Input string </param>
        /// <returns> True if it's unicode, false otherwise </returns>
        public static bool IsUnicode(this string text)
        {
            return string.IsNullOrEmpty(text) || Regex.Replace(text, @"[^\u0000-\u007F]", "") != text;
        }

        /// <summary>
        ///   Validate that string matches regex pattern
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="matchPattern"> </param>
        /// <returns> </returns>
        public static bool MatchesRegex(this string text, string matchPattern)
        {
            return Regex.IsMatch(text, matchPattern,
                                 RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        #endregion

        #region Byte[]

        /// <summary>
        ///   Converts base 64 string to a byte array
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <returns> A byte array equivalent of the base 64 string </returns>
        public static byte[] Base64ToByteArray(this string input)
        {
            return string.IsNullOrEmpty(input) ? new byte[0] : Convert.FromBase64String(input);
        }

        /// <summary>
        ///   Convert binary string representation to byte array
        /// </summary>
        /// <param name="binary"> Binary string </param>
        /// <returns> </returns>
        public static byte[] BinaryToByteArray(this string binary)
        {
            if (binary.Length%8 != 0)
            {
                throw new ArgumentException("The input string did not describe a complete byte.");
            }

            var isValidBinary = IsBinary(binary);

            if (!isValidBinary)
            {
                throw new ArgumentException("The input string was not in binary format.");
            }

            var length = (binary.Length/8);
            var bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(binary.Substring(8*i, 8), 2);
            }
            return bytes;
        }

        /// <summary>
        ///   Converts a hexadecimal string to a byte array representation.
        /// </summary>
        /// <param name="hex"> Hexadecimal string to convert to byte array. </param>
        /// <returns> Byte array representation of the string. </returns>
        /// <remarks>
        ///   The string is assumed to be of even size.
        /// </remarks>
        public static byte[] HexToByteArray(this string hex)
        {
            var b = new byte[hex.Length/2];
            for (var i = 0; i < hex.Length; i += 2)
            {
                b[i/2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return b;
        }

        /// <summary>
        ///   Converts a string to a byte array
        /// </summary>
        /// <param name="input"> input string </param>
        /// <param name="encodingUsing"> The type of encoding the string is using (defaults to UTF8) </param>
        /// <returns> the byte array representing the string </returns>
        public static byte[] ToByteArray(this string input, Encoding encodingUsing = null)
        {
            encodingUsing = encodingUsing ?? new UTF8Encoding();
            return (input.IsNotNullOrEmpty()) ? null : encodingUsing.GetBytes(input);
        }

        #endregion

        #region Strings

        /// <summary>
        ///   Converts ASCII encoding to Unicode
        /// </summary>
        /// <param name="asciiCode"> The ASCII code. </param>
        /// <returns> </returns>
        public static string AsciiToUnicode(this int asciiCode)
        {
            var ascii = Encoding.UTF32;
            var c = (char) asciiCode;
            var b = ascii.GetBytes(c.ToString(CultureInfo.InvariantCulture));
            return ascii.GetString((b));
        }

        /// <summary>
        ///   Returns the decimal representation of a binary number.
        /// </summary>
        /// <param name="text"> Binary string to convert to decimal. </param>
        /// <returns> Decimal representation of string. </returns>
        public static string BinaryToDecimal(this string text)
        {
            return Convert.ToString(Convert.ToInt32(text, 2));
        }

        /// <summary>
        ///   Returns the hexadecimal representation of a binary number.
        /// </summary>
        /// <param name="text"> Binary string to convert to hexadecimal. </param>
        /// <returns> Hexadecimal representation of string. </returns>
        public static string BinaryToHex(this string text)
        {
            return Convert.ToString(Convert.ToInt32(text, 2), 16);
        }

        /// <summary>
        ///   Strips the last specified chars from a string.
        /// </summary>
        /// <param name="text"> The source string. </param>
        /// <param name="removeFromEnd"> The remove from end. </param>
        /// <returns> </returns>
        public static string Chop(this string text, int removeFromEnd)
        {
            string result = text;
            if ((removeFromEnd > 0) && (text.Length > removeFromEnd - 1))
                result = result.Remove(text.Length - removeFromEnd, removeFromEnd);
            return result;
        }

        /// <summary>
        ///   Strips the last specified chars from a string.
        /// </summary>
        /// <param name="text"> The source string. </param>
        /// <param name="backDownTo"> The back down to. </param>
        /// <returns> </returns>
        public static string Chop(this string text, string backDownTo)
        {
            int removeDownTo = text.LastIndexOf(backDownTo, StringComparison.CurrentCultureIgnoreCase);
            int removeFromEnd = 0;
            if (removeDownTo > 0)
                removeFromEnd = text.Length - removeDownTo;

            string result = text;

            if (text.Length > removeFromEnd - 1)
                result = result.Remove(removeDownTo, removeFromEnd);

            return result;
        }

        /// <summary>
        ///   Strips the last char from a a string.
        /// </summary>
        /// <param name="text"> The source string. </param>
        /// <returns> </returns>
        public static string Chop(this string text)
        {
            return Chop(text, 1);
        }

        /// <summary>
        ///   Removes the specified chars from the beginning of a string.
        /// </summary>
        /// <param name="text"> The source string. </param>
        /// <param name="removeFromBeginning"> The remove from beginning. </param>
        /// <returns> </returns>
        public static string Clip(this string text, int removeFromBeginning)
        {
            string result = text;
            if (text.Length > removeFromBeginning)
                result = result.Remove(0, removeFromBeginning);
            return result;
        }

        /// <summary>
        ///   Removes chars from the beginning of a string, up to the specified string
        /// </summary>
        /// <param name="text"> The source string. </param>
        /// <param name="removeUpTo"> The remove up to. </param>
        /// <returns> </returns>
        public static string Clip(this string text, string removeUpTo)
        {
            int removeFromBeginning = text.IndexOf(removeUpTo, StringComparison.CurrentCultureIgnoreCase);
            string result = text;

            if (text.Length > removeFromBeginning && removeFromBeginning > 0)
                result = result.Remove(0, removeFromBeginning);

            return result;
        }

        /// <summary>
        ///   Strips the last char from a a string.
        /// </summary>
        /// <param name="text"> The source string. </param>
        /// <returns> </returns>
        public static string Clip(this string text)
        {
            return Clip(text, 1);
        }

        /// <summary>
        ///   Returns the binary representation of a binary number.
        /// </summary>
        /// <param name="txt"> Decimal string to convert to binary. </param>
        /// <returns> Binary representation of string. </returns>
        public static string DecimalToBinary(this string txt)
        {
            return Convert.ToInt32(txt).ToBinary();
        }

        /// <summary>
        ///   Returns the hexadecimal representation of a decimal number.
        /// </summary>
        /// <param name="txt"> Hexadecimal string to convert to decimal. </param>
        /// <returns> Decimal representation of string. </returns>
        public static string DecimalToHex(this string txt)
        {
            return Convert.ToInt32(txt).ToHex();
        }

        /// <summary>
        ///   Formats the source text using the specified parameters
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="arguments"> </param>
        /// <returns> </returns>
        public static string Format(this string text, params object[] arguments)
        {
            return string.Format(text, arguments);
        }

        /// <summary>
        ///   Returns the binary representation of a hexadecimal number.
        /// </summary>
        /// <param name="hex"> Binary string to convert to hexadecimal. </param>
        /// <returns> Hexadecimal representation of string. </returns>
        public static string HexToBinary(this string hex)
        {
            return Convert.ToString(Convert.ToInt32(hex, 16), 2);
        }

        /// <summary>
        ///   Returns the decimal representation of a hexadecimal number.
        /// </summary>
        /// <param name="hex"> Hexadecimal string to convert to decimal. </param>
        /// <returns> Decimal representation of string. </returns>
        public static string HexToDecimal(this string hex)
        {
            return Convert.ToString(Convert.ToInt32(hex, 16));
        }

        /// <summary>
        ///   Returns the start of the string up to the specified max length
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="maxLength"> </param>
        /// <returns> </returns>
        public static string MaxLength(this string text, int maxLength)
        {
            return (text.Length > maxLength ? text.Substring(0, maxLength - 1) : text);
        }

        /// <summary>
        ///   Removes any tab spaces from the source text
        /// </summary>
        /// <param name="text"> </param>
        /// <returns> </returns>
        public static string RemoveTabs(this string text)
        {
            if (text.IsNotNullOrEmpty())
            {
                var match = Regex.Match(text, @"[^\t]+");
                if (match.Success)
                {
                    return match.Value;
                }
            }
            return null;
        }

        /// <summary>
        ///   Removes all line feeds, carriage returns and tab spaces from the source text
        /// </summary>
        /// <param name="text"> </param>
        /// <returns> </returns>
        public static string SingleLine(this string text)
        {
            return text.Replace("\r\n", string.Empty).Replace("\t", string.Empty).Replace("> <", "><");
        }

        /// <summary>
        ///   Strips all HTML tags from a string
        /// </summary>
        /// <param name="htmlValue"> The HTML string. </param>
        /// <returns> </returns>
        public static string StripHtml(this string htmlValue)
        {
            return StripHtml(htmlValue, String.Empty);
        }

        /// <summary>
        ///   Strips all HTML tags from a string and replaces the tags with the specified replacement
        /// </summary>
        /// <param name="htmlValue"> The HTML string. </param>
        /// <param name="htmlPlaceHolder"> The HTML place holder. </param>
        /// <returns> </returns>
        public static string StripHtml(this string htmlValue, string htmlPlaceHolder)
        {
            const string pattern = @"<(.|\n)*?>";
            string sOut = Regex.Replace(htmlValue, pattern, htmlPlaceHolder);
            sOut = sOut.Replace("&nbsp;", String.Empty);
            sOut = sOut.Replace("&amp;", "&");
            sOut = sOut.Replace("&gt;", ">");
            sOut = sOut.Replace("&lt;", "<");
            return sOut;
        }

        /// <summary>
        ///   Replaces the tabs in the source text with the specified number of spaces
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="spacesPerTab"> </param>
        /// <returns> </returns>
        public static string TabsToSpaces(this string text, int spacesPerTab = 16)
        {
            if (text.IsNotNullOrEmpty())
            {
                return text.Replace("\t", new string(' ', spacesPerTab));
            }
            return text;
        }

        /// <summary>
        ///   Formats the source text as title case (i.e. "ChristopherJamesFairbairn" would become "Christopher James Fairbairn")
        /// </summary>
        /// <param name="text"> </param>
        /// <returns> </returns>
        public static string TitleCase(this string text)
        {
            if (text.IsNotNullOrEmpty())
            {
                var output = string.Empty;
                text.ToCharArray().ForEach(c => output += (char.IsUpper(c) ? " " : string.Empty) + c);
                return output.Trim();
            }
            return text;
        }

        /// <summary>
        ///   Get a users username from a domain name string
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static string ToUserName(this string source)
        {
            if (!source.Contains(@"\"))
                throw new ArgumentException("String is not formated as a domain user");

            var split = source.Split('\\');

            return split[1];
        }

        #endregion

        #region Collections

        /// <summary>
        ///   Split the specified text into lines
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="delimiter"> </param>
        /// <returns> </returns>
        public static string[] SplitLines(this string text, string delimiter = "\r\n")
        {
            return text.IsNotNullOrEmpty() ? text.Split(new[] {delimiter}, StringSplitOptions.None) : null;
        }

        /// <summary>
        ///   Converts comma delimited string to array
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static string[] ToArray(this string source)
        {
            if (source.IsNotNull())
            {
                return source.ToArray(",");
            }
            throw new Exception("Can't convert null value to array.");
        }

        /// <summary>
        ///   Converts delimited string to array by provided delimiter
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="delimiter"> </param>
        /// <returns> If no delimter is found it will return an array with one string </returns>
        public static string[] ToArray(this string source, string delimiter)
        {
            var spliter = new[] {delimiter};

            return !source.Contains(delimiter)
                       ? new[] {source}
                       : source.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        ///   Converts comma delimited items of strings
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static List<string> ToList(this string source)
        {
            return source.ToList(",");
        }

        /// <summary>
        ///   Converts delimited string to items of strings by provided delimiter
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="delimiter"> </param>
        /// <returns> </returns>
        public static List<string> ToList(this string source, string delimiter)
        {
            if (source.IsNotNull())
            {
                var spliter = new[] {delimiter};

                if (!source.Contains(delimiter))
                {
                    var result = new List<string> {source};
                    return result;
                }

                return source.Split(spliter, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return new List<string>();
        }

        #endregion

        #region Validation

        /// <summary>
        ///   Determines whether the specified eval string contains only alpha characters.
        /// </summary>
        /// <param name="alpha"> The eval string. </param>
        /// <returns> <c>true</c> if the specified eval string is alpha; otherwise, <c>false</c> . </returns>
        public static bool IsAlpha(this string alpha)
        {
            return !Regex.IsMatch(alpha, RegexPattern.ALPHA);
        }

        /// <summary>
        ///   Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="alphaNumeric"> The eval string. </param>
        /// <returns> <c>true</c> if the string is alphanumeric; otherwise, <c>false</c> . </returns>
        public static bool IsAlphaNumeric(this string alphaNumeric)
        {
            return !Regex.IsMatch(alphaNumeric, RegexPattern.ALPHA_NUMERIC);
        }

        /// <summary>
        ///   Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="alphaNumeric"> The eval string. </param>
        /// <param name="allowSpaces"> if set to <c>true</c> [allow spaces]. </param>
        /// <returns> <c>true</c> if the string is alphanumeric; otherwise, <c>false</c> . </returns>
        public static bool IsAlphaNumeric(this string alphaNumeric, bool allowSpaces)
        {
            if (allowSpaces)
                return !Regex.IsMatch(alphaNumeric, RegexPattern.ALPHA_NUMERIC_SPACE);
            return IsAlphaNumeric(alphaNumeric);
        }

        /// <summary>
        ///   Determines whether the specified email address string is valid based on regular expression evaluation.
        /// </summary>
        /// <param name="emailaddress"> The email address string. </param>
        /// <returns> <c>true</c> if the specified email address is valid; otherwise, <c>false</c> . </returns>
        public static bool IsEmail(this string emailaddress)
        {
            return Regex.IsMatch(emailaddress, RegexPattern.EMAIL);
        }

        /// <summary>
        ///   Determines whether the specified string is a valid GUID.
        /// </summary>
        /// <param name="guid"> The GUID. </param>
        /// <returns> <c>true</c> if the specified string is a valid GUID; otherwise, <c>false</c> . </returns>
        public static bool IsGuid(this string guid)
        {
            return Regex.IsMatch(guid, RegexPattern.GUID);
        }

        /// <summary>
        ///   Determines whether the specified string is a valid IP address.
        /// </summary>
        /// <param name="ipAddress"> The ip address. </param>
        /// <returns> <c>true</c> if valid; otherwise, <c>false</c> . </returns>
        public static bool IsIpAddress(this string ipAddress)
        {
            return Regex.IsMatch(ipAddress, RegexPattern.IP_ADDRESS);
        }

        /// <summary>
        ///   Determines whether the specified string is lower case.
        /// </summary>
        /// <param name="text"> The input string. </param>
        /// <returns> <c>true</c> if the specified string is lower case; otherwise, <c>false</c> . </returns>
        public static bool IsLowerCase(this string text)
        {
            return Regex.IsMatch(text, RegexPattern.LOWER_CASE);
        }

        /// <summary>
        ///   Determines whether the specified string is a mobilenumber.
        /// </summary>
        /// <param name="mobileNumber"> the mobilenumber </param>
        /// <returns> <c>true</c> if mobilenumber; otherwise, <c>false</c> . </returns>
        public static bool IsMobileNumber(this string mobileNumber)
        {
            return Regex.IsMatch(mobileNumber, RegexPattern.MOBILENUMBER);
        }

        /// <summary>
        ///   Determines whether the specified eval string contains only numeric characters
        /// </summary>
        /// <param name="numeric"> The eval string. </param>
        /// <returns> <c>true</c> if the string is numeric; otherwise, <c>false</c> . </returns>
        public static bool IsNumeric(this string numeric)
        {
            return !Regex.IsMatch(numeric, RegexPattern.NUMERIC);
        }

        /// <summary>
        ///   Determines whether the specified string is a postcode.
        /// </summary>
        /// <param name="postcode"> the postcode </param>
        /// <returns> <c>true</c> if postcode; otherwise, <c>false</c> . </returns>
        public static bool IsPostcode(this string postcode)
        {
            return Regex.IsMatch(postcode, RegexPattern.POSTCODE);
        }

        /// <summary>
        ///   Determines whether the specified string is consider a strong password based on the supplied string.
        /// </summary>
        /// <param name="password"> The password. </param>
        /// <returns> <c>true</c> if strong; otherwise, <c>false</c> . </returns>
        public static bool IsStrongPassword(this string password)
        {
            return Regex.IsMatch(password, RegexPattern.STRONG_PASSWORD);
        }

        /// <summary>
        ///   Determines whether the specified string is upper case.
        /// </summary>
        /// <param name="text"> The input string. </param>
        /// <returns> <c>true</c> if the specified string is upper case; otherwise, <c>false</c> . </returns>
        public static bool IsUpperCase(this string text)
        {
            return Regex.IsMatch(text, RegexPattern.UPPER_CASE);
        }

        /// <summary>
        ///   Determines whether the specified string is a valid URL string using the referenced regex string.
        /// </summary>
        /// <param name="url"> The URL string. </param>
        /// <returns> <c>true</c> if valid; otherwise, <c>false</c> . </returns>
        public static bool IsUrl(this string url)
        {
            return Regex.IsMatch(url, RegexPattern.URL);
        }

        /// <summary>
        ///   Determines whether the specified eval string contains only alpha characters. An exception is thrown if its not alpha
        /// </summary>
        /// <param name="alpha"> The eval string. </param>
        /// <param name="paramName"> </param>
        /// <param name="errorMessage"> </param>
        public static void ValidateAlpha(this string alpha, string paramName, string errorMessage = "")
        {
            if (Regex.IsMatch(alpha, RegexPattern.ALPHA))
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The {0} was not an alpha value.".Format(new {paramName}), paramName);
            }
        }

        /// <summary>
        ///   Determines whether the specified eval string contains only alphanumeric characters An exception is thrown if its not alpha numeric
        /// </summary>
        /// <param name="alphaNumeric"> The eval string. </param>
        /// <param name="paramName"> </param>
        /// <param name="errorMessage"> </param>
        public static void ValidateAlphaNumeric(this string alphaNumeric, string paramName, string errorMessage = "")
        {
            if (Regex.IsMatch(alphaNumeric, RegexPattern.ALPHA_NUMERIC))
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The {0} was not an alpha numeric value.".Format(new {paramName}),
                                              paramName);
            }
        }

        /// <summary>
        ///   Determines whether the specified email address string is valid based on regular expression evaluation. An exception is thrown if its not numeric
        /// </summary>
        /// <param name="emailAddress"> The email address string. </param>
        /// <param name="paramName"> </param>
        /// <param name="errorMessage"> </param>
        public static void ValidateEmail(this string emailAddress, string paramName, string errorMessage = "")
        {
            if (Regex.IsMatch(emailAddress, RegexPattern.EMAIL))
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The {0} was not a valide email address.".Format(new {paramName}),
                                              paramName);
            }
        }

        /// <summary>
        ///   Validate that string is a luhn valid account number
        /// </summary>
        /// <param name="pan"> </param>
        /// <param name="context"> </param>
        /// <returns> </returns>
        public static string ValidateLuhn(this string pan, string context = "account number")
        {
            var checkDigit = int.Parse(pan.Substring((pan.Length - 1), 1));
            //Get each number in pan in reverse order that is not the check digit
            var numbers =
                pan.Substring(0, pan.Length - 1).ToCharArray().Select(
                    x => int.Parse(x.ToString(CultureInfo.InvariantCulture))).Reverse().ToArray();
            int total = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                if ((i + 1).IsOdd())
                {
                    //Double every other number
                    numbers[i] = numbers[i]*2;
                }
                numbers[i] = numbers[i].ToString(CultureInfo.InvariantCulture).ToCharArray().Sum(x => x - '0');
                total += numbers[i];
            }
            if ((total*9)%10 != checkDigit)
            {
                throw new Exception(string.Format("The specified {0} was not a valid luhn number.", context));
            }
            return pan;
        }

        /// <summary>
        ///   Validates that the source text is not null or empty using the specified errorMessage in the error errorMessage
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="paramName"> </param>
        /// <param name="errorMessage"> </param>
        /// <returns> </returns>
        public static string ValidateNotNullOrEmpty(this string text, string paramName, string errorMessage = "")
        {
            if (text.IsNullOrEmpty())
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The {0} was null or empty.".Format(new {paramName}), paramName);
            }
            return text;
        }

        /// <summary>
        ///   Determines whether the specified eval string contains only numeric characters An exception is thrown if its not numeric
        /// </summary>
        /// <param name="numeric"> The eval string. </param>
        /// <param name="paramName"> </param>
        /// <param name="errorMessage"> </param>
        public static void ValidateNumeric(this string numeric, string paramName, string errorMessage = "")
        {
            if (Regex.IsMatch(numeric, RegexPattern.NUMERIC))
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The {0} was not a numeric value.".Format(new {paramName}), paramName);
            }
        }

        #endregion
    }
}