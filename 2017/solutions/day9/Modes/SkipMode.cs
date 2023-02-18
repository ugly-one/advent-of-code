using System.Collections.Generic;

namespace solutions.day9
{
    public class SkipMode : IMode
    {
        public SkipMode()
        {
        }
        public IMode process(char character, Result result)
        {
            return new GarbageMode();
        }
    }
}