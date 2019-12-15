﻿using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet.OutTransformers.Losses
{
    public class BinaryMaxLoss : BinaryMax, ILoss
    {
        public double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            throw new NotSupportedException("This loss can't be used in backpropogation.");
        }

        public new ILoss Clone()
        {
            return new BinaryMaxLoss();
        }
    }
}
