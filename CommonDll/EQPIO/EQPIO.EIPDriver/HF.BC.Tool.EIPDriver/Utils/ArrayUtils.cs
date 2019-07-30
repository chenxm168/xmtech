
namespace HF.BC.Tool.EIPDriver.Utils
{
    using System;
    using System.Collections.Generic;

    public class ArrayUtils<T>
    {
        public static bool ArrayEqual(T[] a1, T[] a2)
        {
            if (!object.ReferenceEquals(a1, a2))
            {
                if ((a1 == null) || (a2 == null))
                {
                    return false;
                }
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                for (int i = 0; i < a1.Length; i++)
                {
                    if (!comparer.Equals(a1[i], a2[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int[] GetSubInt(int[] src, int startPos, int length)
        {
            int[] destinationArray = new int[length];
            Array.Copy(src, startPos, destinationArray, 0, length);
            return destinationArray;
        }
    }
}
