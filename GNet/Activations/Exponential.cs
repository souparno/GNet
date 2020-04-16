﻿using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Exponential : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }
    }
}