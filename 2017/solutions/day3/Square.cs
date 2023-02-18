namespace solutions.day3
{
    public class Square
    {
        public Position Position { get; set; }
        public int Value { get; set; }

        public Square(Position position, int value)
        {
            Position = position;
            Value = value;
        }
    }
}
