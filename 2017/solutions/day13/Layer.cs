namespace solutions.day13
{
    public static class Layer
    {
        public static int Move(int argRange, int argTimeStamp)
        {
            int period = argRange * 2 - 1;
            int stepInPeriod = argTimeStamp % (period-1);

            if (stepInPeriod < argRange) return stepInPeriod;
            else
            {
                int max = argRange - 1;
                int distanceFromMiddle = stepInPeriod - max;
                return max - distanceFromMiddle;
            }
        }
    }
}
