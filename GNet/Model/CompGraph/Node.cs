﻿using GNet.Layers;
using System;
using System.Collections.Generic;

namespace GNet.CompGraph
{
    //todo: is node a right name?
    [Serializable]
    public class Node
    {
        public ImmutableArray<Node> InNodes { get; }
        public ImmutableArray<Node> OutNodes { get; private set; }
        public ImmutableArray<Layer> Layers { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        private readonly List<Node> outNodesList;

        public Node(ImmutableArray<Layer> layers)
        {
            InNodes = new ImmutableArray<Node>();
            OutNodes = new ImmutableArray<Node>();
            Layers = layers;    
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[^1].Shape;
            outNodesList = new List<Node>();

            Connect();
            Initialize(false);
        }

        public Node(params Layer[] layers) : this(new ImmutableArray<Layer>(layers))
        {
        }

        public Node(ImmutableArray<Node> inNodes, ImmutableArray<Layer> layers)
        {
            if((layers[0] is MergeLayer) == false)
            {
                throw new ArgumentException($"nameof{layers}[0] is not of type {nameof(MergeLayer)}");
            }

            InNodes = inNodes;
            OutNodes = new ImmutableArray<Node>();
            Layers = layers;
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[^1].Shape;
            outNodesList = new List<Node>();

            inNodes.ForEach(N => N.outNodesList.Add(this));

            Connect(inNodes);
            Initialize(true);
        }

        public Node(ImmutableArray<Node> inNodes, params Layer[] layers) : this(inNodes, new ImmutableArray<Layer>(layers))
        {
        }

        private void Connect(ImmutableArray<Node> inNodes)
        {
            ((MergeLayer)Layers[0]).Connect(inNodes.Select(N => N.Layers[^1]));

            Connect();
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void Initialize(bool initFirst)
        {
            int i = initFirst ? 0 : 1;

            for (; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }

        public void InitOutNodes()
        {
            OutNodes = ImmutableArray<Node>.FromRef(outNodesList.ToArray());
            outNodesList.Clear();

            OutNodes.ForEach(N => N.InitOutNodes());
        }

        public void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }

            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        private void Forward(bool isTraining)
        {
            //todo: we need to make sure that all inNodes has fully forwarded, only then continue

            Layers.ForEach(L => L.Forward(isTraining));
            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        private void CalcGrads()
        {
            //todo: we need to make sure that all outNodes has fully calced grads, only then continue
            for (int i = Length - 1; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        public void Optimize(IOptimizer optimizer)
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Optimize(optimizer);
            }

            OutNodes.ForEach(N => N.Optimize(optimizer));
        }

        public void Update()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }

            OutNodes.ForEach(N => N.Update());
        }

        public void ClearCache()
        {
            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));

            OutNodes.ForEach(N => N.ClearCache());
        }        
    }
}