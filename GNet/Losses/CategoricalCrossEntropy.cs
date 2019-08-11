﻿using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    public class CategoricalCrossEntropy : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / O);
        }

        public ILoss Clone()
        {
            return new CategoricalCrossEntropy();
        }
    }
}
