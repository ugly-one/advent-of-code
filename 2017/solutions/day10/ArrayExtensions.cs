using System;

namespace solutions.day10
{
    public static class ArrayExtensions
    {
        public static void ReversePart(this byte[] list, int startIndex, int size){
            // find all indexes that has to be reverted
            int[] indexesToReverse = new int[size];

            int currentIndex = startIndex;
            for (int i = 0; i < size; i++)
            {
                if (currentIndex == list.Length) currentIndex = 0;
                indexesToReverse[i] = currentIndex++;
            }
            // revert numbers
            for (int i = 0; i < size/2; i++)
            {
                var firstItem = list[indexesToReverse[i]];
                var lastItem = list[indexesToReverse[size-1-i]];
                
                list[indexesToReverse[i]] = lastItem;
                list[indexesToReverse[size-1-i]] = firstItem;
            }   
        }

        public static byte[] Add (this byte[] list, byte[] toAdd){
            var totalSize = list.Length + toAdd.Length;
            var result = new byte[totalSize];

            for (int i = 0; i < list.Length; i++)
            {
                result[i] = list[i];
            }

            for (int i = 0; i < toAdd.Length; i++)
            {
                result[list.Length+i] = toAdd[i];
            }

            return result;
        }

        public static string ToString2(this byte[] list){
            string result = "";
            
            foreach (var item in list)
            {
                result = result + item.ToString("X2");
            }

            return result.ToLower();
        }
    }
}
