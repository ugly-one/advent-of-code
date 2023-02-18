using System.Collections.Generic;

namespace solutions.day9
{
    public class GroupMode : IMode
    {
        public IMode process(char character, Result result){
            if (character == '{')
                result.Groups.Add(new Group(++result.currentScore));

            else if (character == '<')
                return new GarbageMode();

            else if (character == '}')
                result.currentScore--;
                
            return this;
        }
    }
}