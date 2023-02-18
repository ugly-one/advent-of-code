namespace solutions.day18
{
    public class Register
    {
        public char Name { get; }
        public long Value { get; private set; }

        public Register(char argName)
        {
            Name = argName;
            Value = 0;
        }

        public void SetValue(long argValue)
        {
            Value = argValue;
        }
    }
}
