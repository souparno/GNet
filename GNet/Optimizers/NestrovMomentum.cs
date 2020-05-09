﻿namespace GNet.Optimizers
{
    public class NestrovMomentum : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Momentum { get; }
        public double Epsilon { get; }

        private double epochLr;

        public NestrovMomentum(double learningRate = 0.01, double momentum = 0.9, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Momentum = momentum;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            double oldDelta = O.Cache1;
            O.Cache1 = -epochLr * O.Gradient + Momentum * O.Cache1;
            return (1.0 + Momentum) * O.Cache1 - Momentum * oldDelta;
        }
    }
}