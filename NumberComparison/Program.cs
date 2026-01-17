using System;

namespace NumberComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: NumberComparison <firstNumber> <secondNumber>");
                Console.WriteLine("Example: NumberComparison 5.5 3.2");
                return;
            }

            if (!double.TryParse(args[0], out double firstNumber))
            {
                Console.WriteLine($"Error: '{args[0]}' is not a valid number.");
                return;
            }

            if (!double.TryParse(args[1], out double secondNumber))
            {
                Console.WriteLine($"Error: '{args[1]}' is not a valid number.");
                return;
            }

            NumberComparer comparer = new NumberComparer();
            int result = comparer.Compare(firstNumber, secondNumber);

            Console.WriteLine($"Compare({firstNumber}, {secondNumber}) = {result}");
        }
    }
}
