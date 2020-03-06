﻿using GNet.Model;

namespace GNet.Layers
{
    public interface IKernel : ICloneable<IKernel>
    {
        ShapedArrayImmutable<double> Weights { get; }
        Shape Shape { get; }
        bool IsTrainable { get; }

        void Initialize(Shape shape);

        void Update(ShapedArrayImmutable<Synapse> inSynapses);
    }
}