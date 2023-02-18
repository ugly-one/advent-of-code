using System.Collections.Generic;

namespace solutions.day9
{
    public class GarbageMode : IMode
    {
        public IMode process(char character, Result result)
        {
            if (character == '>')
                return new GroupMode();

            if (character == '!')
                return new SkipMode();

            result.garbageCount++;
            return this;
        }
    }
}