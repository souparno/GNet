﻿using System;

namespace GNet.Model
{
    [Serializable]
    public class Neuron : IOptimizable
    {
        public ImmutableArray<Synapse> InSynapses { get; set; }
        public ImmutableArray<Synapse> OutSynapses { get; set; }
        public virtual double Bias { get; set; }
        public double InVal { get; set; }
        public double OutVal { get; set; }
        public double Gradient { get; set; }
        public double Cache1 { get; set; }
        public double Cache2 { get; set; }
        public double BatchDelta { get; set; }

        public Neuron()
        {
            InSynapses = new ImmutableArray<Synapse>();
            OutSynapses = new ImmutableArray<Synapse>();
        }
    }
}