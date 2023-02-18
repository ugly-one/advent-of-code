using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace solutions.day16
{
    public class DanceManager
    {
        private MovesManager movesManager;
        private List<string> _alreadyDonePositions;

        private int loopSize = 0;

        public DanceManager(MovesManager argMovesManager)
        {
            this.movesManager = argMovesManager;
            _alreadyDonePositions = new List<string>();
        }

        public string CurrentPositions { get; private set; }

        public void DanceTimes(int times)
        {
            bool loopDetected = false;
            for (int i = 0; i < times; i++)
            {
                movesManager.DoAllMoves();

                if (_alreadyDonePositions.Contains(movesManager.CurrentPositions))
                {
                    loopDetected = true;
                    break;
                }
                else
                {
                    loopSize++;
                    _alreadyDonePositions.Add(movesManager.CurrentPositions);
                }
            }

            if (loopDetected)
            {
                Debug.WriteLine(loopSize);

                decimal loops = Math.Floor((decimal)times / (decimal)loopSize);
                var rest = times - (loops * loopSize);

                movesManager.ResetPositions();
                for (int i = 0; i < rest; i++)
                {
                    movesManager.DoAllMoves();
                }
            }

            CurrentPositions = movesManager.CurrentPositions;
        }
    }
}
