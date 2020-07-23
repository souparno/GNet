﻿using System;

namespace GNet.ComputaionGraph
{
    [Serializable]
    public class Graph : Network
    {        
        public Node InputNode { get; }
        public Node OutputNode { get; }

        public Graph(Node inNode, Node outNode) : base(inNode.InputShape, outNode.OutputShape)
        {
            InputNode = inNode;
            OutputNode = outNode;

            inNode.Interconnect();
        }

        protected override void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            InputNode.ResetProcessed();
            InputNode.Forward(inputs, isTraining);
        }

        protected override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            InputNode.ResetProcessed();
            OutputNode.CalcGrads(loss, targets);
        }

        protected override void Optimize(IOptimizer optimizer)
        {
            InputNode.ResetProcessed();
            InputNode.Optimize(optimizer);
        }

        protected override void Update()
        {
            InputNode.ResetProcessed();
            InputNode.Update();
        }

        protected override void ClearCache()
        {
            InputNode.ResetProcessed();
            InputNode.ClearCache();
        }

        protected override ImmutableShapedArray<double> GetOutput()
        {
            return OutputNode.Layers[OutputNode.Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}
