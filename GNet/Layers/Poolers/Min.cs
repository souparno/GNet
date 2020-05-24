﻿using GNet.Model;
using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Min : IPooler
    {
        public ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses)
        {
            double minVal = inSynapses.Min(X => X.InNeuron.OutVal);

            return inSynapses.Select(X => X.InNeuron.OutVal == minVal ? 1.0 : 0.0);
        }
    }
}