using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluatorLibrary;

namespace EvaluatorLibraryTests
{
    [TestFixture]
    public class ExpressionEvaluatorTests
    {
        [TestCase("1 + 1", 2)]
        [TestCase("( 8 + 8 )", 16)]
        [TestCase("2 * 2", 4)]
        [TestCase("1 + 2 + 3", 6)]
        [TestCase("6 / 2", 3)]
        [TestCase("11 + 23", 34)]
        [TestCase("11.1 + 23", 34.1)]
        [TestCase("1 + 1 + 3", 5)]
        [TestCase("( 11.5 + 15.4 ) + 10.1", 37)]
        [TestCase("23 - ( 29.3 - 12.5 )", 6.1999999999999993D)]
        [TestCase("( 1 / 2 ) - 1 + 1", 0.5)]
        public void CalculateTest(string expression, double expectedValue)
        {
            // Arrange

            // Act
            var result = ExpressionEvaluator.Calculate(expression);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [TestCase("1 + 1", 2)]
        [TestCase("2 * 2", 4)]
        [TestCase("1 + 2 + 3", 6)]
        [TestCase("6 / 2", 3)]
        [TestCase("11 + 23", 34)]
        [TestCase("11.1 + 23", 34.1)]
        [TestCase("1 + 1 + 3", 5)]
        [TestCase("( 11.5 + 15.4 ) + 10.1", 37)]
        [TestCase("23 - ( 29.3 - 12.5 )", 6.1999999999999993D)]
        [TestCase("( 1 / 2 ) - 1 + 1", 0.5)]
        [TestCase("5 / ( 1 + 1 )", 2.5)]
        [TestCase("4 * ( ( 5 - 1 ) + 1 )", 20)]
        [TestCase("2 * ( 5 * ( 1 + 2 ) )", 30)]
        [TestCase("( 1 + 6 ) - ( ( 1 / 2 ) * 3 )", 5.5)]
        public void CalculateWithNestedBracketTest(string expression, double expectedValue)
        {
            // Arrange

            // Act
            var result = ExpressionEvaluator.CalculateWithNestedBracketAndPrecedence(expression);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}