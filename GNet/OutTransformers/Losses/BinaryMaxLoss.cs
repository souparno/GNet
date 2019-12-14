﻿using GNet.Extensions.Array.Generic;
using GNet.Extensions.Array.Math;
using GNet.Extensions.ShapedArray.Generic;
using GNet.Extensions.ShapedArray.Math;
using System;

namespace GNet.OutTransformers.Losses
{
    public class BinaryMaxLoss : BinaryMax, ILoss
    {
        public double Compute(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public ShapedArray<double> Derivative(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            throw new NotSupportedException("This loss can't be used in backpropogation.");
        }

        public new ILoss Clone()
        {
            return new BinaryMaxLoss();
        }
    }
}
