﻿using GNet.Model;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Avg : IOperation
    {
        public bool RequiresUpdate { get; } = false;

        public ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses)
        {
            int nIn = inSynapses.Length;

            return inSynapses.Select(X => 1.0 / nIn);
        }
    }
}