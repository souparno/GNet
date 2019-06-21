﻿using System;
using GNet.Extensions.Generic;
using GNet.GlobalRandom;

namespace GNet
{
    public interface IDynamicDataset : IDataset
    {
        void Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer);
    }
}

namespace GNet.Datasets.Dynamic
{
    public class EvenOdd : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public int InputLength { get; }
        public int OutputLength { get; } = 1;
        public int Length { get => DataCollection.Length; }

        public EvenOdd(int intputLength)
        {
            InputLength = intputLength;
        }

        private EvenOdd(int inputLength, Data[] dataset) : this(inputLength)
        {
            DataCollection = dataset.Select(D => D);
        }

        public void Generate(int length, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null)
        {
            DataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                int count = 0;
                double[] inputs = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    inputs[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                    count += inputs[j] == 0.0 ? 0 : 1;
                }

                double output = count % 2 == 0 ? 0.0 : 1.0;

                DataCollection[i] = new Data(inputs, new double[] { output }, inputNormalizer, outputNormalizer);
            }
        }

        public IDataset Clone() => new EvenOdd(InputLength, DataCollection);
    }

    public class Uniform : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public int InputLength { get; }
        public int OutputLength { get; }
        public int Length { get => DataCollection.Length;  }

        public Uniform(int IOLength)
        {
            InputLength = IOLength;
            OutputLength = IOLength;
        }

        private Uniform(int IOLength, Data[] dataset): this(IOLength)
        {
            DataCollection = dataset.Select(D => D);
        }

        public void Generate(int length, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null)
        {
            DataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double[] io = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    io[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                }

                DataCollection[i] = new Data(io, io.Select(X => X), inputNormalizer, outputNormalizer);
            }
        }

        public IDataset Clone() => new Uniform(InputLength, DataCollection);
    }

    public class MathOp1 : IDynamicDataset
    {
        public enum Ops1 { Sin, Cos, Tan, Exp, Ln, Abs, Asin, Acos, Atan, Round }

        public Data[] DataCollection { get; private set; } = new Data[0];
        public Ops1 Operation { get; }
        public double Range { get; }
        public int InputLength { get; } = 1;
        public int OutputLength { get; } = 1;
        public int Length { get => DataCollection.Length; }

        private readonly Func<double, double> mathFunc;

        public MathOp1(Ops1 operation, double range)
        {
            Operation = operation;
            Range = range;

            switch (operation)
            {
                case Ops1.Sin: mathFunc = (X) => Math.Sin(X); break;

                case Ops1.Cos: mathFunc = (X) => Math.Cos(X); break;

                case Ops1.Tan: mathFunc = (X) => Math.Tan(X); break;

                case Ops1.Exp: mathFunc = (X) => Math.Exp(X); break;

                case Ops1.Ln: mathFunc = (X) => Math.Log10(X); break;

                case Ops1.Abs: mathFunc = (X) => Math.Abs(X); break;

                case Ops1.Asin: mathFunc = (X) => Math.Asin(X); break;

                case Ops1.Acos: mathFunc = (X) => Math.Acos(X); break;

                case Ops1.Atan: mathFunc = (X) => Math.Atan(X); break;

                case Ops1.Round: mathFunc = (X) => Math.Round(X); break;

                default: throw new ArgumentOutOfRangeException("Unsupported operation");
            }
        }

        public MathOp1(Ops1 operation, double range, Data[] dataset) : this(operation, range)
        {
            DataCollection = dataset.Select(D => D);
        }

        public void Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = Range * GRandom.NextDouble();
                    res = mathFunc(num);
                }

                DataCollection[i] = new Data(new double[] { num }, new double[] { res }, inputNormalizer, outputNormalizer);
            }
        }

        public IDataset Clone() => new MathOp1(Operation, Range, DataCollection);
    }

    public class MathOp2 : IDynamicDataset
    {
        public enum Ops2 { Add, Sub, Mul, Div, Rem, Pow, Root, Log, Min, Max }

        public Data[] DataCollection { get; private set; } = new Data[0];
        public Ops2 Operation { get; }
        public double Range { get; }
        public int InputLength { get; } = 2;
        public int OutputLength { get; } = 1;
        public int Length { get => DataCollection.Length; }

        private readonly Func<double, double, double> mathFunc;

        public MathOp2(Ops2 operation, double range)
        {
            Operation = operation;
            Range = range;

            switch (operation)
            {
                case Ops2.Add: mathFunc = (X, Y) => X + Y; break;

                case Ops2.Sub: mathFunc = (X, Y) => X - Y; break;

                case Ops2.Mul: mathFunc = (X, Y) => X * Y; break;

                case Ops2.Div: mathFunc = (X, Y) => X / Y; break;

                case Ops2.Rem: mathFunc = (X, Y) => X % Y; break;

                case Ops2.Pow: mathFunc = (X, Y) => Math.Pow(X, Y); break;

                case Ops2.Root: mathFunc = (X, Y) => Math.Pow(X, 1.0 / Y); break;

                case Ops2.Log: mathFunc = (X, Y) => Math.Log(X, Y); break;

                case Ops2.Min: mathFunc = (X, Y) => Math.Min(X, Y); break;

                case Ops2.Max: mathFunc = (X, Y) => Math.Max(X, Y); break;

                default: throw new ArgumentOutOfRangeException("Unsupported operation");
            }
        }

        private MathOp2(Ops2 operation, double range, Data[] dataset) : this(operation, range)
        {
            DataCollection = dataset.Select(D => D);
        }

        public void Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double n1 = 0.0;
                double n2 = 0.0;
                double res = 0.0;

                while (res == 0.0)
                {
                    n1 = Range * GRandom.NextDouble();
                    n2 = Range * GRandom.NextDouble();
                    res = mathFunc(n1, n2);
                }

                DataCollection[i] = new Data(new double[] { n1, n2 }, new double[] { res }, inputNormalizer, outputNormalizer);
            }
        }

        public IDataset Clone() => new MathOp2(Operation, Range, DataCollection);
    }

    // todo: implement. limit length.
    public class MNIST : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public string Path { get; }
        public int Length { get; }
        public int InputLength { get; }
        public int OutputLength { get; }

        public MNIST(string path)
        {
            Path = path;
        }

        private MNIST(int length, int inputLength, int outputLength, Data[] dataCollection)
        {
            Length = length;
            InputLength = inputLength;
            OutputLength = outputLength;
            DataCollection = dataCollection.Select(D => D);
        }

        public void Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer)
        {

        }

        public IDataset Clone() => new MNIST(Length, InputLength, OutputLength, DataCollection);
    }
}
