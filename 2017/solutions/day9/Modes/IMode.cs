using System.Collections.Generic;

namespace solutions.day9
{
    public interface IMode
    {
        IMode process(char character, Result result);
    }
}