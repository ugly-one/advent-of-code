using System.Linq;
using solutions.day10;

namespace solutions.day14
{
    public static class DiskDefragmentor
    {
        public static string GetRowRepresantation(string key, int row)
        {
            var hashCreator = new HashCreator();
            string knotHash = hashCreator.Run(key + "-" + row.ToString());
            return HexToBinaryConverter.Convert(knotHash);
        }
        /*
        public static string[] GetDiskRepresentation(string argKey)
        {
            var rows = new string[128];

            for (int i = 0; i < 128; i++)
            {
                rows[i] = GetRowRepresantation(argKey, i);
            }

            return rows;
        }*/

        public static DiskCell[][] GetDiskRepresentation(string argKey)
        {
            var rows = new DiskCell[128][];

            for (int i = 0; i < 128; i++)
            {
                rows[i] = new DiskCell[128];
                var rowString = GetRowRepresantation(argKey, i);
                //map string to array of cells
                var x = 0;
                rows[i] = rowString.Select(c => new DiskCell(c == '1',x++,i)).ToArray();
            }

            return rows;
        }

        public static int GetUsedCount(DiskCell[][] argDisk)
        {
            var usedCount = 0;
            foreach (var row in argDisk)
            {
                usedCount += row.Where(c => c.used).Count();
            }
            return usedCount;
        }
    }

    public class DiskCell
    {
        public int group;
        public bool used;
        public int x;
        public int y;

        public DiskCell(bool argUsed, int argX, int argY)
        {
            used = argUsed;
            x = argX;
            y = argY;
            group = -1; // undefined group
        }
    }
}
