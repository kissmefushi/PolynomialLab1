using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolynomialLab;

namespace PolynomialLab.Tests
{
    [TestClass]
    public class UnitTest
    {
        private const double Delta = 1e-9;

        //1. сума поліномів однакової розмірності
        [TestMethod]
        public void Add_PolynomialsOfSameSize_ReturnsCoefficientWiseSum()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2, 3);
            Polynomial second = new Polynomial(4, 5, 6);
            Polynomial expected = new Polynomial(5, 7, 9);

            Polynomial actual = first.Add(second);//act
            Assert.AreEqual(expected, actual);//assert
        }

        //2. сума поліномів різної розмірності
        [TestMethod]
        public void Add_PolynomialsOfDifferentSizes_ReturnsSumOfLargerSize()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2);
            Polynomial second = new Polynomial(3, 4, 5);
            Polynomial expected = new Polynomial(4, 6, 5);

            Polynomial actual = first.Add(second);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //3. старші члени знищуються, результат нормалізується
        [TestMethod]
        public void Add_LeadingTermsCancel_ReturnsNormalizedPolynomial()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2, 3);
            Polynomial second = new Polynomial(1, 1, -3);
            Polynomial expected = new Polynomial(2, 3);

            Polynomial actual = first.Add(second);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //4. додавання null -> ArgumentNullException
        [TestMethod]
        public void Add_NullArgument_ThrowsArgumentNullException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            Action act = () => polynomial.Add(null);//act

            Assert.ThrowsException<ArgumentNullException>(act);//assert
        }

        //5. різниця поліномів різної розмірності
        [TestMethod]
        public void Subtract_PolynomialsOfDifferentSizes_ReturnsDifference()
        {
            //arrange
            Polynomial first = new Polynomial(5, 3);
            Polynomial second = new Polynomial(1, 1, 2);
            Polynomial expected = new Polynomial(4, 2, -2);

            Polynomial actual = first.Subtract(second);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //6. P - P = 0
        [TestMethod]
        public void Subtract_PolynomialFromItself_ReturnsZeroPolynomial()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            Polynomial expected = new Polynomial();

            Polynomial actual = polynomial.Subtract(polynomial);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //7. віднімання null -> ArgumentNullException
        [TestMethod]
        public void Subtract_NullArgument_ThrowsArgumentNullException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            Action act = () => polynomial.Subtract(null);//act

            Assert.ThrowsException<ArgumentNullException>(act);//assert
        }

        //8. множення на від'ємне дробове число
        [TestMethod]
        public void MultiplyByNumber_NegativeFractionalNumber_ScalesAllCoefficients()
        {
            //arrange
            Polynomial polynomial = new Polynomial(2, -4, 6);
            Polynomial expected = new Polynomial(-3, 6, -9);

            Polynomial actual = polynomial.MultiplyByNumber(-1.5);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //9. множення на 0 дає нульовий поліном
        [TestMethod]
        public void MultiplyByNumber_Zero_ReturnsZeroPolynomial()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            Polynomial expected = new Polynomial();

            Polynomial actual = polynomial.MultiplyByNumber(0);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //10. множення на NaN -> ArgumentException
        [TestMethod]
        public void MultiplyByNumber_NaN_ThrowsArgumentException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            Action act = () => polynomial.MultiplyByNumber(double.NaN);//act

            Assert.ThrowsException<ArgumentException>(act);//assert
        }

        //11. переповнення double -> OverflowException
        [TestMethod]
        public void MultiplyByNumber_ResultOutOfDoubleRange_ThrowsOverflowException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 1e300);

            Action act = () => polynomial.MultiplyByNumber(1e10);//act

            Assert.ThrowsException<OverflowException>(act);//assert
        }

        //12. додавання числа змінює лише вільний член
        [TestMethod]
        public void AddNumber_PositiveNumber_ChangesOnlyFreeTerm()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            Polynomial expected = new Polynomial(6, 2, 3);

            Polynomial actual = polynomial.AddNumber(5);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //13. додавання числа до нульового полінома
        [TestMethod]
        public void AddNumber_ToZeroPolynomial_ReturnsConstantPolynomial()
        {
            //arrange
            Polynomial polynomial = new Polynomial();
            Polynomial expected = new Polynomial(-7);

            Polynomial actual = polynomial.AddNumber(-7);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //14. додавання нескінченності -> ArgumentException
        [TestMethod]
        public void AddNumber_PositiveInfinity_ThrowsArgumentException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            Action act = () => polynomial.AddNumber(double.PositiveInfinity);//act

            Assert.ThrowsException<ArgumentException>(act);//assert
        }

        //15. P(2) = 17
        [TestMethod]
        public void Evaluate_PositiveX_ReturnsValue()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            double expected = 17;

            double actual = polynomial.Evaluate(2);//act

            Assert.AreEqual(expected, actual, Delta);//assert
        }

        //16. P(-1.5) = 4.75
        [TestMethod]
        public void Evaluate_NegativeFractionalX_ReturnsValue()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            double expected = 4.75;

            double actual = polynomial.Evaluate(-1.5);//act

            Assert.AreEqual(expected, actual, Delta);//assert
        }

        //17. P(0) дорівнює вільному члену
        [TestMethod]
        public void Evaluate_ZeroX_ReturnsFreeTerm()
        {
            //arrange
            Polynomial polynomial = new Polynomial(7, -2, 0, 5);
            double expected = 7;

            double actual = polynomial.Evaluate(0);//act

            Assert.AreEqual(expected, actual, Delta);//assert
        }

        //18. x = NaN -> ArgumentException
        [TestMethod]
        public void Evaluate_NaN_ThrowsArgumentException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            Action act = () => polynomial.Evaluate(double.NaN);//act

            Assert.ThrowsException<ArgumentException>(act);//assert
        }

        //19. переповнення double -> OverflowException
        [TestMethod]
        public void Evaluate_ResultOutOfDoubleRange_ThrowsOverflowException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(0, 0, 1);

            Action act = () => polynomial.Evaluate(1e200);//act

            Assert.ThrowsException<OverflowException>(act);//assert
        }

        //20. (1 + x)(1 - x) = 1 - x^2
        [TestMethod]
        public void Multiply_TwoLinearPolynomials_ReturnsQuadratic()
        {
            //arrange
            Polynomial first = new Polynomial(1, 1);
            Polynomial second = new Polynomial(1, -1);
            Polynomial expected = new Polynomial(1, 0, -1);

            Polynomial actual = first.Multiply(second);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //21. розмірність добутку n + m - 1
        [TestMethod]
        public void Multiply_PolynomialsOfDifferentSizes_ReturnsProductOfSizeNPlusMMinusOne()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2, 3);
            Polynomial second = new Polynomial(2, -1);
            Polynomial expected = new Polynomial(2, 3, 4, -3);

            Polynomial actual = first.Multiply(second);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //22. множення на нульовий поліном
        [TestMethod]
        public void Multiply_ByZeroPolynomial_ReturnsZeroPolynomial()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            Polynomial zero = new Polynomial();
            Polynomial expected = new Polynomial();

            Polynomial actual = polynomial.Multiply(zero);//act

            Assert.AreEqual(expected, actual);//assert
        }

        //23. множення на null -> ArgumentNullException
        [TestMethod]
        public void Multiply_NullArgument_ThrowsArgumentNullException()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            Action act = () => polynomial.Multiply(null);//act

            Assert.ThrowsException<ArgumentNullException>(act);//assert
        }

        //24. переповнення double -> OverflowException
        [TestMethod]
        public void Multiply_ResultOutOfDoubleRange_ThrowsOverflowException()
        {
            //arrange
            Polynomial first = new Polynomial(0, 1e200);
            Polynomial second = new Polynomial(0, 1e200);

            Action act = () => first.Multiply(second);//act

            Assert.ThrowsException<OverflowException>(act);//assert
        }

        //25. поліном дорівнює своїй копії
        [TestMethod]
        public void Equals_CopyOfPolynomial_ReturnsTrue()
        {
            //arrange
            Polynomial original = new Polynomial(1, 2, 3);
            Polynomial copy = new Polynomial(original);

            bool actual = original.Equals(copy);//act

            Assert.IsTrue(actual);//assert
        }

        //26. різні коефіцієнти -> false
        [TestMethod]
        public void Equals_DifferentCoefficients_ReturnsFalse()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2, 3);
            Polynomial second = new Polynomial(1, 2, 4);

            bool actual = first.Equals(second);//act

            Assert.IsFalse(actual);//assert
        }

        //27. різна розмірність -> false
        [TestMethod]
        public void Equals_DifferentSizes_ReturnsFalse()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2);
            Polynomial second = new Polynomial(1, 2, 3);

            bool actual = first.Equals(second);//act

            Assert.IsFalse(actual);//assert
        }

        //28. старші нулі відкидаються -> true
        [TestMethod]
        public void Equals_ArrayWithTrailingZeros_ReturnsTrue()
        {
            //arrange
            Polynomial first = new Polynomial(1, 2, 0, 0);
            Polynomial second = new Polynomial(1, 2);

            bool actual = first.Equals(second);//act

            Assert.IsTrue(actual);//assert
        }

        //29. різниця менша за Epsilon -> true
        [TestMethod]
        public void Equals_DifferenceWithinEpsilon_ReturnsTrue()
        {
            //arrange
            Polynomial first = new Polynomial(0.1 + 0.2, 1);
            Polynomial second = new Polynomial(0.3, 1);

            bool actual = first.Equals(second);//act

            Assert.IsTrue(actual);//assert
        }

        //30. порівняння з null -> false
        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);

            bool actual = polynomial.Equals(null);//act

            Assert.IsFalse(actual);//assert
        }

        //31. об'єкт іншого типу -> false
        [TestMethod]
        public void Equals_ObjectOfAnotherType_ReturnsFalse()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2);
            object other = "2x + 1";

            bool actual = polynomial.Equals(other);//act

            Assert.IsFalse(actual);//assert
        }

        //32. "3x^2 - 2x + 1"
        [TestMethod]
        public void ToString_GeneralPolynomial_ReturnsTermsFromHighestPower()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, -2, 3);
            string expected = "3x^2 - 2x + 1";

            string actual = polynomial.ToString();//act

            Assert.AreEqual(expected, actual);//assert
        }

        //33. одиничні й нульові коефіцієнти
        [TestMethod]
        public void ToString_UnitAndZeroCoefficients_OmitsOnesAndZeroTerms()
        {
            //arrange
            Polynomial polynomial = new Polynomial(0, -1, 0, 1);
            string expected = "x^3 - x";

            string actual = polynomial.ToString();//act

            Assert.AreEqual(expected, actual);//assert
        }

        //34. десяткова крапка в дробових коефіцієнтах
        [TestMethod]
        public void ToString_FractionalCoefficients_UsesDecimalPoint()
        {
            //arrange
            Polynomial polynomial = new Polynomial(-0.5, 2.5);
            string expected = "2.5x - 0.5";

            string actual = polynomial.ToString();//act

            Assert.AreEqual(expected, actual);//assert
        }

        //35. нульовий поліном -> "0"
        [TestMethod]
        public void ToString_ZeroPolynomial_ReturnsZero()
        {
            //arrange
            Polynomial polynomial = new Polynomial();
            string expected = "0";

            string actual = polynomial.ToString();//act

            Assert.AreEqual(expected, actual);//assert
        }

        //36. зміна вихідного масиву не змінює поліном
        [TestMethod]
        public void Constructor_SourceArrayChangedAfterCreation_PolynomialUnchanged()
        {
            //arrange
            double[] source = { 1, 2, 3 };
            Polynomial polynomial = new Polynomial(source);
            Polynomial expected = new Polynomial(1, 2, 3);

            source[0] = 100;//act

            Assert.AreEqual(expected, polynomial);//assert
        }

        //37. зміна масиву з Coefficients не змінює поліном
        [TestMethod]
        public void Coefficients_ReturnedArrayChanged_PolynomialUnchanged()
        {
            //arrange
            Polynomial polynomial = new Polynomial(1, 2, 3);
            Polynomial expected = new Polynomial(1, 2, 3);

            //act
            double[] coefficients = polynomial.Coefficients;
            coefficients[0] = 100;

            Assert.AreEqual(expected, polynomial);//assert
        }
    }
}
