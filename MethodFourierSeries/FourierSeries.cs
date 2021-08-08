using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodFourierSeries
{
    class FourierSeries
    {
        private double l = 4;
        private (double A, double B) limits = (A: -4, B: 4);
        private int _degree = 15;
        private List<double> resultXValues;
        private List<double> resultYValues;

        private Func<double, double> Func1 = x => x;
        private Func<double, double> Func2 = x => 2 * x;

        public double Epsilon { get; set; } = 0.001;
        public (double A, double B) Limits
        {
            get => limits; set
            {
                limits = value;
                l = value.B;
            }
        }

        public FourierSeries()
        {
            resultXValues = new List<double>();
            resultYValues = new List<double>();
        }

        public FourierSeries(int degree) : this()
        {
            _degree = degree;
        }

        public FourierSeries(int degree, Func<double, double> func1, Func<double, double> func2) : this(degree)
        {
            Func1 = func1;
            Func2 = func2;
        }

        public (List<double> xValues, List<double> yValues) GetResult()
        {
            Calculate();
            return (xValues: resultXValues, yValues: resultYValues);
        }

        private void Calculate()
        {
            var a0Int1 = RightRectangle(Func1, Limits.A, 0, 1000);
            var a0Int2 = RightRectangle(Func2, 0, Limits.B, 1000);
            var a0 = (1d / l) * (a0Int1 + a0Int2);

            double[] aValues = GetAValues();
            double[] bValues = GetBValues();

            double f(double x)
            {
                double result = Math.Round(a0 / 2, 4);

                for (int i = 1; i <= _degree; i++)
                {
                    result += Math.Round(aValues[i - 1] * Math.Cos(i * x), 4);
                    result += Math.Round(bValues[i - 1] * Math.Sin(i * x), 4);
                }

                return result;
            };

            CalculateResult(f);
        }
       
        private void CalculateResult(Func<double, double> f)
        {
            resultXValues.Clear();
            resultYValues.Clear();

            for (double i = Limits.A; i < Limits.B; i += Epsilon)
            {
                resultXValues.Add(i);
                resultYValues.Add(f(i));
            }
        }

        private double[] GetAValues()
        {
            double[] aValues = new double[_degree];

            for (int i = 1; i <= _degree; i++)
            {
                double f1(double x) => Func1(x) * Math.Cos(i * Math.PI * x / l);
                double f2(double x) => Func2(x) * Math.Cos(i * Math.PI * x / l);

                aValues[i - 1] = FindIntegral(f1, f2);
            }

            return aValues;
        }

        private double[] GetBValues()
        {
            double[] bValues = new double[_degree];

            for (int i = 1; i <= _degree; i++)
            {
                double f1(double x) => Func1(x) * Math.Sin(i * Math.PI * x / l);
                double f2(double x) => Func2(x) * Math.Sin(i * Math.PI * x / l);
                
                bValues[i - 1] = FindIntegral(f1, f2); ;
            }

            return bValues;
        }

        private double FindIntegral(Func<double, double> f1, Func<double, double> f2)
        {
            var int1 = Math.Round(RightRectangle(f1, Limits.A, 0, 1000), 4);
            var int2 = Math.Round(RightRectangle(f2, 0, Limits.B, 1000), 4);

            var result = Math.Round((1d / l), 4) * (int1 + int2);
            return result;
        }

        private double RightRectangle(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = 0d;
            for (var i = 1; i <= n; i++)
            {
                var x = a + i * h;
                sum += f(x);
            }

            var result = h * sum;
            return result;
        }
    }
}
