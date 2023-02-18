using System;

namespace solutions.day1
{
    public class Captcha
    {
        public int solve(string input){
            int length = input.Length;

            if (length == 0) return 0;
            if (length == 1) return 0;

            int sum = 0;
            int iToCopmare;
            int valueToAdd;
            for (int i = 0; i < length; i++)
            {
                iToCopmare = i + length/2;
                if (iToCopmare >= length) iToCopmare = iToCopmare - length;
                if (input[i] == input[iToCopmare]) {
                    
                    Int32.TryParse(input[i].ToString(), out valueToAdd);
                    sum += valueToAdd;
                    }

            }

            return sum;
        }
    }
}
