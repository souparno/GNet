﻿using GNet.Extensions;
using GNet.GlobalRandom;
using System;
using static System.Math;

namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        double[] Activate(double[] Vals);

        double[] Derivative(double[] Vals);
    }
}

namespace GNet.Activations
{
    public class Identity : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 1.0);
        }

        public IActivation Clone() => new Identity();
    }

    public class BinaryStep : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X > 0 ? 1.0 : 0.0);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 0.0);
        }

        public IActivation Clone() => new BinaryStep();
    }

    public class Sigmoid : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => 1 / (1 + Exp(-X)));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => Exp(X) / Pow(Exp(X) + 1, 2));
        }

        public IActivation Clone() => new Sigmoid();
    }

    public class HardSigmoid : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X < -2.5 ? 0 : X > 2.5 ? 1 : 0.2 * X + 0.5);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X < -2.5 || X > 2.5 ? 0 : 0.2);
        }

        public IActivation Clone() => new HardSigmoid();
    }

    public class Tanh : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => Math.Tanh(X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 1 / Pow(Cosh(X), 2));
        }

        public IActivation Clone() => new Tanh();
    }

    public class LeCunTanh : IActivation
    {
        private const double a = 1.7159;
        private const double b = 2.0 / 3;

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => a * Math.Tanh(b * X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => a * b / Pow(Cosh(b * X), 2));
        }

        public IActivation Clone() => new LeCunTanh();
    }

    public class ArcTan : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => Atan(X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 1 / (1 + X * X));
        }

        public IActivation Clone() => new ArcTan();
    }

    public class ArSinh : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => Asinh(X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 1 / Sqrt(1 + X * X));
        }

        public IActivation Clone() => new ArSinh();
    }

    public class SoftSign : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X / (1 + Abs(X)));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 1 / Pow(1 + Abs(X), 2));
        }

        public IActivation Clone() => new SoftSign();
    }

    /// <summary>
    /// Inverse Square Root Unit
    /// </summary>
    public class ISRU : IActivation
    {
        public double Alpha { get; }

        public ISRU(double alpha)
        {
            Alpha = alpha;
        }

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X / Sqrt(1 + Alpha * X * X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => Pow(X / Sqrt(1 + Alpha * X * X), 3));
        }

        public IActivation Clone() => new ISRU(Alpha);
    }

    /// <summary>
    /// Inverse Square Root Linear Unit
    /// </summary>
    public class ISRLU : IActivation
    {
        public double Alpha { get; }

        public ISRLU(double alpha)
        {
            Alpha = alpha;
        }

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X >= 0 ? X : X / Sqrt(1 + Alpha * X * X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X >= 0 ? 1 : Pow(X / Sqrt(1 + Alpha * X * X), 3));
        }

        public IActivation Clone() => new ISRLU(Alpha);
    }

    /// <summary>
    /// Square Nonlinearity 
    /// </summary>
    public class SQNL : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X =>
            {
                if (X > 2)
                    return 1;

                if (X >= 0 && X <= 2)
                    return X - X * X / 4;

                if (X >= -2 && X < 0)
                    return X - X * X / 4;

                return -1;
            });
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X > 0 ? 1 - X / 2 : 1 + X / 2);
        }

        public IActivation Clone() => new SQNL();
    }

    /// <summary>
    /// Rectified Linear Unit
    /// </summary>
    public class ReLu : IActivation
    {
        public double Slope { get; }

        public ReLu(double slope = 0)
        {
            Slope = slope;
        }

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? Slope * X : X);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? Slope : 1);
        }

        public IActivation Clone() => new ReLu(Slope);
    }

    /// <summary>
    /// Randomized Rectified Linear Unit
    /// </summary>
    public class RReLu : IActivation
    {
        public double Slope { get; }

        public RReLu()
        {
            Slope = GRandom.NextDouble();
        }

        private RReLu(double slope)
        {
            Slope = slope;
        }

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? Slope * X : X);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? Slope : 1);
        }

        public IActivation Clone() => new RReLu(Slope);
    }

    /// <summary>
    /// Exponential Linear Unit
    /// </summary>
    public class ELU : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? (Exp(X) - 1) : X);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? Exp(X) : 1);
        }

        public IActivation Clone() => new ELU();
    }

    /// <summary>
    /// Scaled Exponential Linear Unit
    /// </summary>
    public class SELU : IActivation
    {
        private const double a = 1.6732632423543772;
        private const double b = 1.0507009873554805;

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? b * a * (Exp(X) - 1) : b * X);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X < 0 ? b * a * Exp(X) : b);
        }

        public IActivation Clone() => new SELU();
    }

    public class SoftPlus : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => Log(1 + Exp(X)));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 1 / (1 + Exp(-X)));
        }

        public IActivation Clone() => new SoftPlus();
    }

    public class BentIdentity : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => (Sqrt(X * X + 1) - 1) / 2 + X);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X / (2 * Sqrt(X * X + 1)) + 1);
        }

        public IActivation Clone() => new BentIdentity();
    }

    public class Swish : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X / (Exp(-X) + 1));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X =>
            {
                var exp = Exp(X);
                return exp * (1 + exp + X) / Pow(1 + exp, 2);
            });
        }

        public IActivation Clone() => new Swish();
    }

    public class SoftExponential : IActivation
    {
        public double Alpha { get; }
        private readonly Func<double, double> activation;
        private readonly Func<double, double> derivative;

        public SoftExponential(double alpha)
        {
            Alpha = alpha;

            if (alpha < 0)
            {
                activation = (X) => -Log(1 - alpha * (alpha + X)) / alpha;
                derivative = (X) => 1 / (1 - alpha * (alpha + X));
            }
            else if (alpha > 0)
            {
                activation = (X) => (Exp(alpha * X) - 1) / alpha + alpha;
                derivative = (X) => Exp(alpha * X);
            }
            else
            {
                activation = (X) => X;
                derivative = (X) => 1;
            }
        }

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => activation(X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => derivative(X));
        }

        public IActivation Clone() => new SoftExponential(Alpha);
    }

    public class SoftClipping : IActivation
    {
        public double Alpha { get; }

        public SoftClipping(double alpha)
        {
            Alpha = alpha;
        }

        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => 1 / Alpha * Log((1 + Exp(Alpha * X)) / (1 + Exp(Alpha * X - Alpha))));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => 0.5 * Sinh(0.5 * Alpha) * (1 / Cosh(0.5 * Alpha * X)) * (1 / Cosh(0.5 * Alpha * (1 - X))));
        }

        public IActivation Clone() => new SoftClipping(Alpha);
    }

    public class Sinusoid : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => Sin(X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => Cos(X));
        }

        public IActivation Clone() => new Sinusoid();
    }

    public class Sinc : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => X != 0 ? Sin(X) / X : 1);
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => X != 0 ? Cos(X) / X - Sin(X) / (X * X) : 0);
        }

        public IActivation Clone() => new Sinc();
    }

    public class Gaussian : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            return Vals.Select(X => Exp(-X * X));
        }

        public double[] Derivative(double[] Vals)
        {
            return Vals.Select(X => -2 * X * Exp(-X * X));
        }

        public IActivation Clone() => new Gaussian();
    }

    public class Softmax : IActivation
    {
        public double[] Activate(double[] Vals)
        {
            double[] exps = Vals.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E / sum);
        }

        public double[] Derivative(double[] Vals)
        {
            double[] exps = Vals.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E * (sum - E) / (sum * sum));
        }

        public IActivation Clone() => new Softmax();
    }
}
