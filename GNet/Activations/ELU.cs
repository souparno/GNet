﻿using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class ELU : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }
    }
}