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
    }
}
