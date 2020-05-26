﻿using System;

namespace GNet
{
    [Serializable]
    public class Network
    {
        public delegate void BatchLogger(int batch);
        public delegate void ErrorLogger(double error);
        public delegate void EpochErrorLogger(int epoch, double error);

        public event BatchLogger? OnBatch;
        public event ErrorLogger? OnStart;
        public event EpochErrorLogger? OnEpoch;
        public event EpochErrorLogger? OnFinish;

        public ImmutableArray<ILayer> Layers { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        public Network(ImmutableArray<ILayer> layers)
        {
            Layers = layers;
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[layers.Length - 1].Shape;

            Connect();
            Initialize();
        }

        public Network(params ILayer[] layers) : this(new ImmutableArray<ILayer>(layers))
        {
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void Initialize()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }

        private void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            Layers[0].Input(inputs, isTraining);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }
        }

        private void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].CalcGrads();
            }
        }

        private void Optimize(IOptimizer optimizer, int epoch)
        {
            optimizer.UpdateParams(epoch);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Optimize(optimizer);
            }
        }

        private void Update()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }
        }

        private void ClearCache()
        {
            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));
        }

        public ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs)
        {
            Forward(inputs, false);

            return Layers[Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }

        public double Validate(Dataset dataset, IMetric metric)
        {
            return dataset.Avarage(D => metric.Evaluate(D.Targets, Predict(D.Inputs)));
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, Dataset valDataset, IMetric metric, bool shuffle = true)
        {
            if (dataset.InputShape != Layers[0].Shape)
            {
                throw new ShapeMismatchException($"{nameof(dataset)} {nameof(dataset.InputShape)}");
            }

            if (dataset.TargetShape != Layers[Length - 1].Shape)
            {
                throw new ShapeMismatchException($"{nameof(dataset)} {nameof(dataset.TargetShape)}");
            }

            if (valDataset.InputShape != Layers[0].Shape)
            {
                throw new ShapeMismatchException($"{nameof(valDataset)} {nameof(dataset.InputShape)}");
            }

            if (valDataset.TargetShape != Layers[Length - 1].Shape)
            {
                throw new ShapeMismatchException($"{nameof(valDataset)} {nameof(dataset.TargetShape)}");
            }

            ClearCache();

            double valError = Validate(valDataset, metric);
            int epoch;

            OnStart?.Invoke(valError);

            for (epoch = 0; epoch < nEpoches; epoch++)
            {
                if (valError <= minError)
                {
                    break;
                }

                if (shuffle)
                {
                    dataset.Shuffle();
                }

                dataset.ForEach((D, index) =>
                {
                    Forward(D.Inputs, true);
                    CalcGrads(loss, D.Targets);
                    Optimize(optimizer, epoch);

                    if (index % batchSize == 0)
                    {
                        Update();

                        OnBatch?.Invoke(index / batchSize);
                    }
                });

                valError = Validate(valDataset, metric);

                OnEpoch?.Invoke(epoch, valError);
            }

            OnFinish?.Invoke(epoch, valError);
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, bool shuffle = true)
        {
            Train(dataset, loss, optimizer, batchSize, nEpoches, minError, dataset, loss, shuffle);
        }
    }
}