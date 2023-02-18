namespace solutions.day10
{
    public class RoundResult
    {
        public byte[] List {get; private set;}
        public int Skip {get; private set;}
        public int CurrentPosition {get; private set;}

        public RoundResult(byte[] list, int skip, int currentPosition)
        {
            List = list;
            Skip = skip;
            CurrentPosition = currentPosition;
        }
    }
}