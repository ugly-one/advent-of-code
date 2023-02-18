using System.Collections.Generic;

public static class NumbersGenerator
{
    public static IEnumerable<int> getOddNumbers()
    {
        int i = -1;
        while (true)
        {
            yield return i += 2;
        }
    }
}