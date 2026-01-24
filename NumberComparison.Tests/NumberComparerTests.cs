using System;
using Xunit;
using NumberComparison;

namespace NumberComparison.Tests
{
    public class NumberComparerTests
    {
        private readonly NumberComparer _comparer = new NumberComparer();

        // Equal numbers tests
        [Fact]
        public void Compare_WithEqualIntegers_ReturnsZero()
        {
            // Arrange
            double firstNumber = 5;
            double secondNumber = 5;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_WithEqualDecimals_ReturnsZero()
        {
            // Arrange
            double firstNumber = 3.14;
            double secondNumber = 3.14;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_WithEqualNegativeNumbers_ReturnsZero()
        {
            // Arrange
            double firstNumber = -10.5;
            double secondNumber = -10.5;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(0, result);
        }

        // First number less than second number tests
        [Fact]
        public void Compare_WithFirstLessThanSecond_ReturnsNegativeOne()
        {
            // Arrange
            double firstNumber = 3.5;
            double secondNumber = 7.2;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_WithFirstNegativeLessThanSecondPositive_ReturnsNegativeOne()
        {
            // Arrange
            double firstNumber = -5;
            double secondNumber = 10;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_WithFirstNegativeLessThanSecondNegative_ReturnsNegativeOne()
        {
            // Arrange
            double firstNumber = -20;
            double secondNumber = -5;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(-1, result);
        }

        // Second number less than first number tests
        [Fact]
        public void Compare_WithSecondLessThanFirst_ReturnsOne()
        {
            // Arrange
            double firstNumber = 10.5;
            double secondNumber = 2.3;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_WithFirstPositiveGreaterThanSecondNegative_ReturnsOne()
        {
            // Arrange
            double firstNumber = 15;
            double secondNumber = -8;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_WithFirstNegativeGreaterThanSecondNegative_ReturnsOne()
        {
            // Arrange
            double firstNumber = -3;
            double secondNumber = -15;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(1, result);
        }

        // Error handling tests - Note: The NumberComparer.Compare method accepts double parameters
        // and does not have built-in error handling for non-numeric inputs.
        // These tests verify the current behavior with extreme/edge cases.
        
        [Fact]
        public void Compare_WithZero_ReturnsZero()
        {
            // Arrange
            double firstNumber = 0;
            double secondNumber = 0;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_WithVerySmallDifference_ReturnsCorrectComparison()
        {
            // Arrange
            double firstNumber = 0.0000001;
            double secondNumber = 0.0000002;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_WithVeryLargeNumbers_ReturnsCorrectComparison()
        {
            // Arrange
            double firstNumber = 1e10;
            double secondNumber = 1e11;

            // Act
            int result = _comparer.Compare(firstNumber, secondNumber);

            // Assert
            Assert.Equal(-1, result);
        }

        // Note on Input Validation:
        // The NumberComparer.Compare method accepts only double parameters.
        // String validation (like checking for letters) happens at the Program.Main level
        // using double.TryParse(), not within the Compare method itself.
        // The test class focuses on testing the Compare method's logic with valid numeric inputs.

        // NumberComparerWithSlack tests
        [Fact]
        public void NumberComparerWithSlack_WithEqualStrings_ReturnsZero()
        {
            // Arrange
            string firstNumber = "5.5";
            string secondNumber = "5.5";
            double slack = 0.1;

            // Act
            int result = _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void NumberComparerWithSlack_WithinSlackTolerance_ReturnsZero()
        {
            // Arrange
            string firstNumber = "5.0";
            string secondNumber = "5.4";
            double slack = 0.5;

            // Act
            int result = _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void NumberComparerWithSlack_FirstLessThanSecondBeyondSlack_ReturnsNegativeOne()
        {
            // Arrange
            string firstNumber = "3.0";
            string secondNumber = "7.0";
            double slack = 1.0;

            // Act
            int result = _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void NumberComparerWithSlack_FirstGreaterThanSecondBeyondSlack_ReturnsOne()
        {
            // Arrange
            string firstNumber = "10.0";
            string secondNumber = "2.0";
            double slack = 1.0;

            // Act
            int result = _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void NumberComparerWithSlack_WithNegativeNumbers_ReturnsCorrectComparison()
        {
            // Arrange
            string firstNumber = "-5.5";
            string secondNumber = "-5.3";
            double slack = 0.5;

            // Act
            int result = _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void NumberComparerWithSlack_WithInvalidFirstNumber_ThrowsArgumentException()
        {
            // Arrange
            string firstNumber = "abc";
            string secondNumber = "5.5";
            double slack = 0.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack));
        }

        [Fact]
        public void NumberComparerWithSlack_WithInvalidSecondNumber_ThrowsArgumentException()
        {
            // Arrange
            string firstNumber = "5.5";
            string secondNumber = "xyz";
            double slack = 0.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack));
        }

        [Fact]
        public void NumberComparerWithSlack_WithLettersAndNumbers_ThrowsArgumentException()
        {
            // Arrange
            string firstNumber = "5a5";
            string secondNumber = "5.5";
            double slack = 0.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack));
        }

        [Fact]
        public void NumberComparerWithSlack_WithZeroSlack_WorksLikeExactComparison()
        {
            // Arrange
            string firstNumber = "5.0";
            string secondNumber = "5.001";
            double slack = 0;

            // Act
            int result = _comparer.NumberComparerWithSlack(firstNumber, secondNumber, slack);

            // Assert
            Assert.Equal(-1, result);
        }

        // NumberComparerWithoutSlack tests
        [Fact]
        public void NumberComparerWithoutSlack_WithEqualNumbers_ReturnsZero()
        {
            // Arrange
            string firstNumber = "10.5";
            string secondNumber = "10.5";

            // Act
            int result = _comparer.NumberComparerWithoutSlack(firstNumber, secondNumber);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void NumberComparerWithoutSlack_WithFirstLessThanSecond_ReturnsNegativeOne()
        {
            // Arrange
            string firstNumber = "3.2";
            string secondNumber = "7.8";

            // Act
            int result = _comparer.NumberComparerWithoutSlack(firstNumber, secondNumber);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void NumberComparerWithoutSlack_WithFirstGreaterThanSecond_ReturnsOne()
        {
            // Arrange
            string firstNumber = "15.6";
            string secondNumber = "8.9";

            // Act
            int result = _comparer.NumberComparerWithoutSlack(firstNumber, secondNumber);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void NumberComparerWithoutSlack_WithInvalidInput_ThrowsArgumentException()
        {
            // Arrange
            string firstNumber = "letters";
            string secondNumber = "5.5";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _comparer.NumberComparerWithoutSlack(firstNumber, secondNumber));
        }

        [Fact]
        public void NumberComparerWithoutSlack_WithBothLetters_ThrowsArgumentException()
        {
            // Arrange
            string firstNumber = "abc";
            string secondNumber = "xyz";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _comparer.NumberComparerWithoutSlack(firstNumber, secondNumber));
        }

        [Fact]
        public void NumberComparerWithoutSlack_WithMixedLettersAndNumbers_ThrowsArgumentException()
        {
            // Arrange
            string firstNumber = "5a";
            string secondNumber = "3b";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _comparer.NumberComparerWithoutSlack(firstNumber, secondNumber));
        }
    }
}
