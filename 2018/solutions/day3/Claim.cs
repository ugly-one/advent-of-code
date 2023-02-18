using System;
namespace Solutions.day3
{
    public class Claim
    {
        public readonly int LeftEdge;
        public readonly int TopEdge;
        public readonly int Width;
        public readonly int Height;
        public readonly int Id;

        public bool IsOverlapped { get; private set; }

        public Claim(int id, int leftEdge, int topEdge, int width, int height)
        {
            Id = id;
            LeftEdge = leftEdge;
            TopEdge = topEdge;
            Width = width;
            Height = height;
            IsOverlapped = false;
        }

        public void Overlap()
        {
            IsOverlapped = true;
        }
    }
}
