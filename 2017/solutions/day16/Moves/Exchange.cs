namespace solutions.day16
{
    public class Exchange : Move
    {
        private int _firstPosition;
        private int _secondPosition;

        public int FirstPosition => _firstPosition;
        public int SeconPosition => _secondPosition;

        public Exchange(int argFirstPosition, int argSecondPosition)
        {
            _firstPosition = argFirstPosition;
            _secondPosition = argSecondPosition;
        }
        public override string Dance(string argCurrentPositions)
        {
            int amountOfPrograms = argCurrentPositions.Length;
            char[] newPrograms = new char[amountOfPrograms];

            for (int i = 0; i < amountOfPrograms; i++)
            {
                var indexToPlace = i;
                if (i == _firstPosition) indexToPlace = _secondPosition;
                else if (i == _secondPosition) indexToPlace = _firstPosition;
                newPrograms[indexToPlace] = argCurrentPositions[i];
            }

            return new string(newPrograms);
        }
    }
}
