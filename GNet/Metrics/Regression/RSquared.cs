﻿namespace GNet.Metrics.Regression
{
    public class RSquared : IMetric
    {
        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            double avgT = targets.Average();

            return targets.Sum(outputs, (T, O) => (T - O) * (T - O)) / targets.Sum(T => (T - avgT) * (T - avgT));
        }
    }
}