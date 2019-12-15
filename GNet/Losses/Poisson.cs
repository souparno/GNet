﻿using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class Poisson : ILoss
    {
        public double Compute(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T * Log(O)).Avarage();
        }

        public ShapedArray<double> Derivative(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 1.0 - (T / O));
        }

        public ILoss Clone()
        {
            return new Poisson();
        }
    }
}
