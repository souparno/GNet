﻿using static System.Math;

namespace GNet.Losses.Categorical
{
    public class CrossEntropy : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return -T * Log(O);
        }

        public double Derivative(double T, double O)
        {
            return -T / O;
        }
    }
}