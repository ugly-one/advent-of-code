using System;

namespace solutions.day16
{
    public class Partner : Move
    {
        private char _firstChar;
        private char _secondChar;

        public char FirstChar => _firstChar;
        public char SecondChar => _secondChar;

        public Partner(char argFirstChar, char argSecondChar)
        {
            _firstChar = argFirstChar;
            _secondChar = argSecondChar;
        }
        public override string Dance(string argCurrentPositions)
        {
            int firstCharIndex = argCurrentPositions.IndexOf(_firstChar);
            if (firstCharIndex == -1) throw new ArgumentException($"given string {argCurrentPositions} doesn't contain {firstCharIndex}");

            int secondCharIndex = argCurrentPositions.IndexOf(_secondChar);
            if (secondCharIndex == -1) throw new ArgumentException($"given string {argCurrentPositions} doesn't contain {secondCharIndex}");

            var exchange = new Exchange(firstCharIndex, secondCharIndex);
            return exchange.Dance(argCurrentPositions);
        }
    }

}
