using System;

namespace Extensions
{
    public static class Boolean
    {
        /// <summary>
        ///   Validate that the condition is false and return error errorMessage provided.
        /// </summary>
        /// <param name="condition"> Condition to check. </param>
        /// <param name="errorMessage"> Error to use when throwing an <see cref="ArgumentException" /> if the condition is false. </param>
        /// <param name="paramName"> </param>
        public static void IsFalse(bool condition, string errorMessage = "", string paramName = "")
        {
            if (condition)
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The condition supplied is true when it was expected to be false",
                                              paramName);
            }
        }

        /// <summary>
        ///   Validate that the condition is true and return error errorMessage provided.
        /// </summary>
        /// <param name="condition"> Condition to check. </param>
        /// <param name="message"> Error to use when throwing an <see cref="ArgumentException" /> if the condition is false. </param>
        public static void IsTrue(bool condition, String message)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        ///   Validate that the condition is true.
        /// </summary>
        /// <param name="condition"> Condition to check. </param>
        /// <param name="errorMessage"> </param>
        /// <param name="paramName"> </param>
        public static void ValidateIsTrue(this bool condition, string errorMessage = "", string paramName = "")
        {
            if (condition == false)
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The condition supplied is false when it was expected to be true",
                                              paramName);
            }
        }

        /// <summary>
        ///   Returns a yes/no string based on the specified boolean flag
        /// </summary>
        /// <param name="flag"> Boolean to check. </param>
        /// <param name="trueValue"> The true string value. Defaults to 'Yes' </param>
        /// <param name="falseValue"> The false string value. Defaults to 'No' </param>
        /// <returns> A string that represents the boolean flag. </returns>
        public static string YesNo(this bool flag, string trueValue = "Yes", string falseValue = "No")
        {
            return (flag ? trueValue : falseValue);
        }
    }
}