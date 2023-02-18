using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day10
{
    public class HashCreator
    {
        public string Run(byte[] list, string lenghts){
            byte[] byteLenghts = lenghts.ConvertToAscii().Add(new byte[]{17, 31, 73, 47, 23});
            byte[] after64Rounds = process64Rounds(list, byteLenghts);
            byte[] reducedHash = ReduceHash(after64Rounds);

            return reducedHash.ToString2();
        }

        public string Run(string lengths)
        {
            return Run(CreateList(), lengths);
        }

        private byte[] CreateList()
        {
            var list = new byte[256];
            byte i = 0;
            while (true)
            {
                list[i] = i;
                if (i == 255) return list;
                i++;
            }
        }

        public RoundResult processOneRound(byte[] list, byte[] lenghts, int skip = 0, int currentPosition = 0){

            foreach (var lenght in lenghts)
            {
                list.ReversePart(currentPosition, lenght);
                currentPosition += (lenght + skip);
                currentPosition = GetRelativePosition(currentPosition,list.Length);
                skip++;    
            }

            return new RoundResult(list, skip, currentPosition);
        }

        private byte[] process64Rounds(byte[] list, byte[]lenghts){

            var result = processOneRound(list, lenghts);
            for (int i = 0; i < 63; i++)
            {   
                result = processOneRound(list, lenghts, result.Skip, result.CurrentPosition);
            }     

            return result.List;       
        }

        private int GetRelativePosition(int currentPosition, int size){
            if(currentPosition >= size) return currentPosition % size;
            return currentPosition;
        }


        public byte ReducePartialHash(IEnumerable<byte> hash){
            int result = 0;
            foreach (var item in hash)
            {
                result = result ^ item;
            }

            return (byte) result;
        }

        public byte[] ReduceHash(byte[] hash)
        {
            var result = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                result[i] = ReducePartialHash(hash.Skip(i*16).Take(16));
            }

            return result;
        }
    }
}