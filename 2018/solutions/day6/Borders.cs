using System;
namespace Solutions.day6
{
    public class Borders
    {
        public uint Top;
        public uint Bottom;
        public uint Left;
        public uint Right;

        public Borders(uint topBorder, uint bottomBorder, uint leftBorder, uint rightBorder)
        {
            Top = topBorder;
            Bottom = bottomBorder;
            Left = leftBorder;
            Right = rightBorder;
        }
    }
}
