using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day14
{
    public class GroupMarker
    {
        private DiskCell[][] disk;

        public GroupMarker(DiskCell[][] argDisk)
        {
            disk = argDisk;
        }
        public int MarkGroups()
        {
            int groupCounter = 0;
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    if (disk[y][x].group == -1)
                    {
                        if ((disk[y][x].used) && (disk[y][x].group == -1))
                            MarkCell(disk[y][x], ++groupCounter);
                    }
                }
            }

            return groupCounter;
        }

        private void MarkCell(DiskCell argCell, int groupNr)
        {
            argCell.group = groupNr;
            IEnumerable<DiskCell> connectedCells = GetConnectedCells(argCell);
            foreach (var cell in connectedCells)
            {
                MarkCell(cell, groupNr);
            }
        }

        private IEnumerable<DiskCell> GetConnectedCells(DiskCell argCell)
        {
            var connectedCells = new List<DiskCell>();
            // north
            if (argCell.y > 0)
                connectedCells.Add(disk[argCell.y - 1][argCell.x]);
            // south
            if (argCell.y < 127)
                connectedCells.Add(disk[argCell.y + 1][argCell.x]);
            // west
            if (argCell.x > 0)
                connectedCells.Add(disk[argCell.y][argCell.x - 1]);
            // east
            if (argCell.x < 127)
                connectedCells.Add(disk[argCell.y][argCell.x + 1]);
            // return only used and not grouped cells
            return connectedCells.Where(c => c.group == -1 && c.used);
        }
    }
}
