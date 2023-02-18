using System;

namespace solutions.day16
{
    public static class MoveConverter
    {
        public static Move Convert(string input)
        {
            if (input[0] == 's')
            {
                int.TryParse(input.Substring(1), out int spinSize);
                return new Spin(spinSize);
            }
            else if (input[0] == 'x')
            {
                var numbersAndSlash = input.Substring(1);
                var numbers = numbersAndSlash.Split('/');
                int.TryParse(numbers[0], out int firstNumber);
                int.TryParse(numbers[1], out int secondNumber);

                return new Exchange(firstNumber, secondNumber);
            }
            else if (input[0] == 'p')
            {
                var lettersAndSlash = input.Substring(1);
                var letters = lettersAndSlash.Split('/');
                return new Partner(letters[0][0], letters[1][0]);
            }

            else throw new ArgumentException($"Unable to convert {input}");
        }
    }
}
