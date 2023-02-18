
using solutions.day15;

namespace tests.day15
{
    public static class TestHelper
    {
    public static void GenerateMultipleTimes(Generator argGenerator, int argTimes)
    {
        for (int i = 0; i < argTimes; i++)
        {
            argGenerator.Generate();
        }
    }
}
}
