using System.Collections.Generic;

namespace solutions.day17
{

    public static class IndexGenerator
    {
        public static IEnumerable<int> Generate(int argStepsToJump)
        {
            int size = 1;
            int index = 0;
            while (true)
            {
                index += argStepsToJump % size;
                index = index % size;
                index++;
                if (index > size) index = 0;
                yield return index;
                size++;
            }
        }
    }
}
