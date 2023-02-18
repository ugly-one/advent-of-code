using System;
using System.Collections.Generic;

namespace solutions.day9
{
    public class StreamProcessor
    {
        public Result Process(string input)
        {
            var result = new Result();
            IMode readerMode = new GroupMode();
            
            foreach (char item in input)
            {
                readerMode = readerMode.process(item, result);
            }

            return result;
        }
    }
}
