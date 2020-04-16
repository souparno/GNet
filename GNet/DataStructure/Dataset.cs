﻿using System;

namespace GNet
{
    [Serializable]
    public class Dataset : IArray<Data>
    {
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        public Data this[int index] => dataCollection[index];

        private ImmutableArray<Data> dataCollection;

        public Dataset(ImmutableArray<Data> dataCollection)
        {
            InputShape = dataCollection[0].Inputs.Shape;
            OutputShape = dataCollection[0].Outputs.Shape;

            dataCollection.ForEach((D, i) =>
            {
                if (D.Inputs.Shape != InputShape || D.Outputs.Shape != OutputShape)
                {
                    throw new ShapeMismatchException($"{nameof(dataCollection)} [{i}] mismatch.");
                }
            });

            this.dataCollection = dataCollection;
            Length = dataCollection.Length;
        }

        public Dataset(params Data[] dataCollection) : this(new ImmutableArray<Data>(dataCollection))
        {
        }

        public void Normalize(INormalizer? inputNormalizer, INormalizer? outputNormalizer)
        {
            inputNormalizer ??= new Normalizers.None();
            outputNormalizer ??= new Normalizers.None();

            inputNormalizer.UpdateParams(dataCollection.Select(D => D.Inputs));
            outputNormalizer.UpdateParams(dataCollection.Select(D => D.Outputs));

            dataCollection = dataCollection.Select(D => new Data(inputNormalizer.Normalize(D.Inputs), outputNormalizer.Normalize(D.Outputs)));
        }

        public void Shuffle()
        {
            Data[] shuffled = dataCollection.ToMutable();

            for (int i = 0; i < Length; i++)
            {
                int iRnd = Utils.GRandom.Next(i, Length);
                Data temp = shuffled[i];
                shuffled[i] = shuffled[iRnd];
                shuffled[iRnd] = temp;
            }

            dataCollection = ImmutableArray<Data>.FromRef(shuffled);
        }
    }
}