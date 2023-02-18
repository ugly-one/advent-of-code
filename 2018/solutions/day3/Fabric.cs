using System;
using System.Collections.Generic;

namespace Solutions.day3
{
    public class Fabric
    {
        private readonly int m_size;
        private List<Claim>[][] fabric;
        private List<Claim> claims;

        public Fabric(int size)
        {
            m_size = size;
            fabric = new List<Claim>[m_size][];
            for (int i = 0; i < m_size; i++)
            {
                fabric[i] = new List<Claim>[m_size];
                for (int j = 0; j < m_size; j++)
                {
                    fabric[i][j] = new List<Claim>(10);
                }
            }

            claims = new List<Claim>();
        }

        public void Mark(string[] input)
        {
            foreach (var line in input)
            {
                var claim = Parser.ParseClaim(line);

                claims.Add(claim);

                for (int w = 0; w < claim.Width; w++)
                {
                    for (int h = 0; h < claim.Height; h++)
                    {
                        fabric[h + claim.TopEdge][w + claim.LeftEdge].Add(claim);
                        if (fabric[h + claim.TopEdge][w + claim.LeftEdge].Count > 1)
                        {
                            foreach (var c in fabric[h + claim.TopEdge][w + claim.LeftEdge])
                            {
                                c.Overlap();
                            }
                        }
                    }
                }

            }
        }

        public int GetOverlappedCount()
        {
            int count = 0;
            for (int i = 0; i < m_size; i++)
            {
                for (int j = 0; j < m_size; j++)
                {
                    if (fabric[i][j].Count > 1) count++;
                }
            }            
            return count;
        }

        public int GetIdOfNonOverlappingClaims()
        {
            foreach (var c in claims)
            {
                if (!c.IsOverlapped) return c.Id;
            }

            throw new Exception("Cannot find any claims that do not overlap");
        }
    }
}
