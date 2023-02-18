namespace solutions.day16
{
    public class Spin : Move
    {
        private int _spinValue;
        public int SpinValue => _spinValue;

        public Spin(int argValue)
        {
            _spinValue = argValue;
        }

        public override string Dance(string argCurrentPositions)
        {
            int amountOfPrograms = argCurrentPositions.Length;
            char[] newPrograms = new char[amountOfPrograms];

            for (int i = 0; i < amountOfPrograms; i++)
            {
                var programToMove = argCurrentPositions[i];
                var indexToPlace = ( i + _spinValue ) % amountOfPrograms;
                newPrograms[indexToPlace] = programToMove;
            }

            return new string(newPrograms); 
        }
    }
}
