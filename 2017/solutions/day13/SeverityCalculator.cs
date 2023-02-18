using System.Collections.Generic;

namespace solutions.day13
{
    class SeverityCalculator
    {
        public static int Calculate(List<(int timeStamp, int range)> argData)
        {
            int severity = 0;
            int positionAtGivenTime;
            foreach (var item in argData)
            {
                positionAtGivenTime = Layer.Move(item.range, item.timeStamp);
                if (positionAtGivenTime == 0)
                {
                    severity += item.timeStamp * item.range;
                }
            }
            return severity;
        }

        public static int CalculateWithDelay(List<(int timeStamp, int range)> argData, int delay)
        {
            int severity = 0;
            int positionAtGivenTime;
            foreach (var item in argData)
            {
                positionAtGivenTime = Layer.Move(item.range, item.timeStamp + delay);
                if (positionAtGivenTime == 0)
                {
                    severity += (item.timeStamp + delay ) * item.range;
                }

                if (severity > 0) return severity;
            }
            return severity;
        }
    }
}
