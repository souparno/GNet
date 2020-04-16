﻿using System;

namespace GNet.OutTransformers.Losses
{
    public class BinaryMaxLoss : BinaryMax, ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            throw new NotSupportedException();
        }
    }
}