﻿using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sinc : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Sin(X) / X : 1.0);
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Cos(X) / X - Sin(X) / (X * X) : 0.0);
        }

        public IActivation Clone()
        {
            return new Sinc();
        }
    }
}
