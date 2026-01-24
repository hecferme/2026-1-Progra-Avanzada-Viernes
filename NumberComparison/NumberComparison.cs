using System;

namespace NumberComparison
{
    public class NumberComparer
    {
        /// <summary>
        /// Compares two real numbers.
        /// </summary>
        /// <param name="firstNumber">The first number to compare</param>
        /// <param name="secondNumber">The second number to compare</param>
        /// <returns>
        /// 0 if the numbers are equal
        /// -1 if the first number is strictly less than the second
        /// 1 if the first number is strictly greater than the second
        /// </returns>
        [Obsolete("Use NumberComparerWithSlack or NumberComparerWithoutSlack instead. This method will be removed in a future version.")]
        public int Compare(double firstNumber, double secondNumber)
        {
            if (firstNumber == secondNumber)
            {
                return 0;
            }
            else if (firstNumber < secondNumber)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares two numbers provided as strings with a specified slack (tolerance) between them.
        /// </summary>
        /// <param name="firstNumberStr">The first number as a string to compare</param>
        /// <param name="secondNumberStr">The second number as a string to compare</param>
        /// <param name="slack">The tolerance allowed between the two numbers</param>
        /// <returns>
        /// 0 if the numbers are equal within the slack tolerance
        /// -1 if the first number is strictly less than the second (beyond slack)
        /// 1 if the first number is strictly greater than the second (beyond slack)
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when either string cannot be converted to a double</exception>
        public int NumberComparerWithSlack(string firstNumberStr, string secondNumberStr, double slack)
        {
            if (!double.TryParse(firstNumberStr, out double firstNumber))
            {
                throw new ArgumentException($"'{firstNumberStr}' is not a valid number.", nameof(firstNumberStr));
            }

            if (!double.TryParse(secondNumberStr, out double secondNumber))
            {
                throw new ArgumentException($"'{secondNumberStr}' is not a valid number.", nameof(secondNumberStr));
            }

            double difference = Math.Abs(firstNumber - secondNumber);

            if (difference <= slack)
            {
                return 0;
            }
            else if (firstNumber < secondNumber)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares two numbers provided as strings without any slack (tolerance = 0).
        /// </summary>
        /// <param name="firstNumberStr">The first number as a string to compare</param>
        /// <param name="secondNumberStr">The second number as a string to compare</param>
        /// <returns>
        /// 0 if the numbers are equal
        /// -1 if the first number is strictly less than the second
        /// 1 if the first number is strictly greater than the second
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when either string cannot be converted to a double</exception>
        public int NumberComparerWithoutSlack(string firstNumberStr, string secondNumberStr)
        {
            return NumberComparerWithSlack(firstNumberStr, secondNumberStr, 0);
        }
    }
}
