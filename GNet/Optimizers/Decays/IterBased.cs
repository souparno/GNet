﻿namespace GNet.Optimizers.Decays
{
    public class IterBased : IDecay
    {
        public double Rate { get; }

        public IterBased(double rate)
        {
            Rate = rate;
        }

        public double Compute(double X, int T)
        {
            return X / (1.0 + Rate * T);
        }
    }
}