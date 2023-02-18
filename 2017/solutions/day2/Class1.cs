using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace solutions.day2
{
    public class Class1
    {
        /// <summary>
        /// returns the first number that divides another one from the collection without a remainder
        /// </summary>
        /// <param name="input">sorted collection of numbers</param>
        /// <returns></returns>
        public int FindFirstEvenDivider(IEnumerable<int> input){
            
            var reversed = input.Reverse();
            foreach (var item in reversed)
            {
                foreach (var dividor in input)
                {
                    if (dividor > (item / 2)) break;
                    if (item % dividor == 0){
                        return item / dividor;
                    }
                }
            }
            return 0;
        }

        public IEnumerable<int> convertAndSort (string line){
            string[] split = line.Split('\t');
            int integer;
            var result = new List<int>();
            foreach (var item in split)
            {
                Int32.TryParse(item, out integer);
                var firstBigger = result.FirstOrDefault(r => r > integer);
                if (firstBigger == 0) result.Add(integer);
                else {
                    var index = result.FindIndex(0,result.Count, a => a==firstBigger);
                    if (index == 0) result.Insert(0, integer);
                    else result.Insert(index, integer);
                }
            }

            return result;
        }

        public int getCheckSum(string[] lines){
            int sum = 0;
            foreach (var item in lines)
            {
                sum += this.FindFirstEvenDivider(convertAndSort(item));
            }

            return sum;
        }
    }
}
