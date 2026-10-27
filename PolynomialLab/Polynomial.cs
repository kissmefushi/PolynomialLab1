using System;
using System.Globalization;
using System.Text;

namespace PolynomialLab
{
    public class Polynomial : IPolynomial
    {
        public const double Epsilon = 1e-9;

        private int size;
        private double[] coefficients;

        public Polynomial()
            : this(0.0)
        {
        }

        public Polynomial(params double[] coefficients)
        {
            Coefficients = coefficients;
        }

        public Polynomial(Polynomial other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            Coefficients = other.coefficients;
        }

        public int Size
        {
            get { return size; }
        }

        public int Degree
        {
            get { return size - 1; }
        }

        public double[] Coefficients
        {
            get
            {
                double[] copy = new double[size];
                Array.Copy(coefficients, copy, size);
                return copy;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (value.Length == 0)
                {
                    throw new ArgumentException("Поліном повинен мати хоча б один коефіцієнт.");
                }
                for (int i = 0; i < value.Length; i++)
                {
                    CheckFinite(value[i], nameof(value));
                }
                coefficients = Normalize(value);
                size = coefficients.Length;
            }
        }

        public Polynomial Add(Polynomial other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            double[] result = new double[Math.Max(size, other.size)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetCoefficient(i) + other.GetCoefficient(i);
            }
            return CreateResult(result);
        }

        public Polynomial Subtract(Polynomial other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            double[] result = new double[Math.Max(size, other.size)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetCoefficient(i) - other.GetCoefficient(i);
            }
            return CreateResult(result);
        }

        public Polynomial MultiplyByNumber(double number)
        {
            CheckFinite(number, nameof(number));
            double[] result = new double[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = coefficients[i] * number;
            }
            return CreateResult(result);
        }

        public Polynomial AddNumber(double number)
        {
            CheckFinite(number, nameof(number));
            double[] result = Coefficients;
            result[0] += number;
            return CreateResult(result);
        }

        public double Evaluate(double x)
        {
            CheckFinite(x, nameof(x));
            double result = 0.0;
            for (int i = size - 1; i >= 0; i--)
            {
                result = result * x + coefficients[i];
            }
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                throw new OverflowException("Значення полінома виходить за межі типу double.");
            }
            return result;
        }

        public Polynomial Multiply(Polynomial other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            double[] result = new double[size + other.size - 1];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < other.size; j++)
                {
                    result[i + j] += coefficients[i] * other.coefficients[j];
                }
            }
            return CreateResult(result);
        }

        public override bool Equals(object obj)
        {
            Polynomial other = obj as Polynomial;
            if (other == null)
            {
                return false;
            }
            if (size != other.size)
            {
                return false;
            }
            for (int i = 0; i < size; i++)
            {
                if (Math.Abs(coefficients[i] - other.coefficients[i]) > Epsilon)
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return size.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            for (int i = size - 1; i >= 0; i--)
            {
                double value = coefficients[i];
                if (value == 0.0)
                {
                    continue;
                }
                if (text.Length == 0)
                {
                    text.Append(value < 0 ? "-" : "");
                }
                else
                {
                    text.Append(value < 0 ? " - " : " + ");
                }
                double absValue = Math.Abs(value);
                if (i == 0 || absValue != 1.0)
                {
                    text.Append(absValue.ToString("G15", CultureInfo.InvariantCulture));
                }
                if (i >= 1)
                {
                    text.Append("x");
                }
                if (i >= 2)
                {
                    text.Append("^").Append(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            return text.Length == 0 ? "0" : text.ToString();
        }

        private double GetCoefficient(int power)
        {
            return power < size ? coefficients[power] : 0.0;
        }

        private static Polynomial CreateResult(double[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                if (double.IsInfinity(result[i]) || double.IsNaN(result[i]))
                {
                    throw new OverflowException("Коефіцієнт результату виходить за межі типу double.");
                }
            }
            return new Polynomial(result);
        }

        private static void CheckFinite(double number, string paramName)
        {
            if (double.IsNaN(number) || double.IsInfinity(number))
            {
                throw new ArgumentException("Число повинно бути скінченним.", paramName);
            }
        }

        private static double[] Normalize(double[] source)
        {
            int newSize = source.Length;
            while (newSize > 1 && Math.Abs(source[newSize - 1]) < Epsilon)
            {
                newSize--;
            }
            double[] result = new double[newSize];
            for (int i = 0; i < newSize; i++)
            {
                result[i] = Math.Abs(source[i]) < Epsilon ? 0.0 : source[i];
            }
            return result;
        }
    }
}
