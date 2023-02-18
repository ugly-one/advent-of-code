using System.Collections.Generic;

namespace solutions.day9
{
    public class Result
    {
        public List<Group> Groups;
        public int currentScore;
        public int garbageCount;

        public Result()
        {
            Groups = new List<Group>();
            currentScore = 0;
            garbageCount = 0;
        }
    }
}