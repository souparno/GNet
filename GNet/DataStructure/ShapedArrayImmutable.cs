﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public readonly struct ShapedArrayImmutable<T> : IArray<T>, IEquatable<ShapedArrayImmutable<T>>
    {
        public Shape Shape { get; }
        public int Length => internalArray.Length;
        public T this[int index] => internalArray[index];
        public T this[params int[] indices] => internalArray[Shape.FlattenIndices(indices)];
        private readonly ArrayImmutable<T> internalArray;

        public ShapedArrayImmutable(Shape shape, ArrayImmutable<T> array)
        {
            if (shape.Volume != array.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            internalArray = array;
            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, params T[] array) : this(shape, new ArrayImmutable<T>(array))
        {
        }       

        public ShapedArrayImmutable(Shape shape, IList<T> list) : this(shape, new ArrayImmutable<T>(list))
        {
        }   

        public ShapedArrayImmutable(Shape shape, IEnumerable<T> enumerable) : this(shape, new ArrayImmutable<T>(enumerable))
        {
        }

        public ShapedArrayImmutable(Shape shape, IEnumerable enumerable) : this(shape, new ArrayImmutable<T>(enumerable))
        {
        }

        public ShapedArrayImmutable(Shape shape, Func<T> element) : this(shape, new ArrayImmutable<T>(shape.Volume, element))
        {
        }

        public static bool operator !=(ShapedArrayImmutable<T> left, ShapedArrayImmutable<T> right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(ShapedArrayImmutable<T> left, ShapedArrayImmutable<T> right)
        {
            return left.Equals(right);
        }

        public bool Equals(ShapedArrayImmutable<T> other)
        {
            if (Shape != other.Shape)
            {
                return false;
            }

            if (internalArray != other.internalArray)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return (obj is ShapedArrayImmutable<T> shapedArr) && Equals(shapedArr);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(internalArray, Shape);
        }

        public ArrayImmutable<T> ToFlat()
        {
            return internalArray;
        }
    }
}