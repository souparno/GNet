﻿using System;
using System.Collections.Generic;

namespace GNet.Extensions
{
    public static class Extensions
    {
        private static readonly Random rnd = new Random();

        public static T[] DeepClone<T>(this T[] array)
        {
            return (T[])RecursiveClone(array);
        }

        private static object RecursiveClone(Array array)
        {
            Array newArr = (Array)Activator.CreateInstance(array.GetType(), array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                var element = array.GetValue(i);

                if (element is Array)
                {
                    newArr.SetValue(RecursiveClone(element as Array), i);
                }
                else if (element is ICloneable)
                {
                    newArr.SetValue((element as ICloneable).Clone(), i);
                }
                else
                {
                    newArr.SetValue(element, i);
                }
            }

            return newArr;
        }

        public static T[] Flatten<T>(this Array array)
        {
            List<T> flattened = new List<T>();

            foreach (var element in array)
            {
                if (element is Array)
                {
                    flattened.AddRange(Flatten<T>(element as Array));
                }
                else if (element is T)
                {
                    flattened.Add((T)element);
                }
                else
                {
                    throw new ArrayTypeMismatchException("array element type mismatch");
                }
            }

            return flattened.ToArray();
        }

        public static void ClearRecursive(this Array array)
        {
            if (array.GetType().GetElementType().IsArray == false)
            {
                Array.Clear(array, 0, array.Length);
                return;
            }

            foreach (var element in array)
            {
                ClearRecursive((Array)element);
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int upperBound = list.Count;

            while (upperBound > 1)
            {
                upperBound--;

                int index = rnd.Next(upperBound + 1);

                T tempVal = list[index];
                list[index] = list[upperBound];
                list[upperBound] = tempVal;
            }
        }

        public static double NextDouble(this Random rnd, double minValue, double maxValue)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static double NextDouble(this Random rnd, double range)
        {
            return NextDouble(rnd, -range, range);
        }

        public static double NextGaussian(this Random rnd)
        {
            return Math.Sqrt(-2.0 * Math.Log(1.0 - rnd.NextDouble())) * Math.Sin(2.0 * Math.PI * (1.0 - rnd.NextDouble()));
        }
    }
}

