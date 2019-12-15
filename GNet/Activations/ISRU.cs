﻿using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
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

        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => X / Sqrt(1.0 + Alpha * X * X));
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => Pow(X / Sqrt(1.0 + Alpha * X * X), 3.0));
        }

        public IActivation Clone()
        {
            return new ISRU(Alpha);
        }
    }
}
