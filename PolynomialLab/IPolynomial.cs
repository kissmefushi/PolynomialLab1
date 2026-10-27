namespace PolynomialLab
{
    public interface IPolynomial
    {
        int Size { get; }

        double[] Coefficients { get; }

        Polynomial Add(Polynomial other);

        Polynomial Subtract(Polynomial other);

        Polynomial MultiplyByNumber(double number);

        Polynomial AddNumber(double number);

        double Evaluate(double x);

        Polynomial Multiply(Polynomial other);
    }
}
