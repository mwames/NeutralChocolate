using System;

namespace DebugZone
{
    public static class ArrayHelper {
        public static T[] Take<T>(int start, int count, T[] array) {
            var endIndex = start + count;
            
            var length = (endIndex > array.Length)
                ? array.Length - start
                : endIndex - start;

            T[] copy = new T[length];
            Array.Copy(array, start, copy, 0, length);
            return copy;
        }
    }
}
