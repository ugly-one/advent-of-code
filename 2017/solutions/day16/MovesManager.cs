using System.Collections.Generic;

namespace solutions.day16
{
    public class MovesManager
    {
        private string _currentPositions;
        private List<Move> _moves;
        private int size; // neded for reset
        public string CurrentPositions => _currentPositions;

        public MovesManager(int argSize)
        {
            _currentPositions = "";
            size = argSize;
            for (int i = 0; i < size; i++)
            {
                _currentPositions += (char)(97 + i);
            }
            _moves = new List<Move>();
        }

        public void SetMoves(List<Move> argMoves)
        {
            _moves = argMoves;
        }

        public void DoAllMoves()
        {
            foreach (var move in _moves)
            {
                _currentPositions = move.Dance(CurrentPositions);
            }
        }

        public void ResetPositions()
        {
            _currentPositions = "";
            for (int i = 0; i < size; i++)
            {
                _currentPositions += (char)(97 + i);
            }
        }
    }
}
