﻿using static System.Math;

namespace GNet.Metrics.Regression
{
    class CosineSimilarity : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            double dotProd = targets.Sum(outputs, (T, O) => T * O);

            return dotProd / (Sqrt(targets.Sum(T => T * T)) + Sqrt(outputs.Sum(O => O * O)));
        }
    }
}