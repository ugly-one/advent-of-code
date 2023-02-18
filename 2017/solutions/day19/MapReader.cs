using System;
using System.Collections.Generic;
using System.IO;

namespace solutions.day19
{
    public static class MapReader
    {
        public static char[][] Read(string fileName)
        {
            List<char[]> listOfRows = new List<char[]>();
            using (var sr = new StreamReader(fileName))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    listOfRows.Add(line.ToCharArray());
                }
            }

            char[][] map = new char[listOfRows.Count][];
            for (int i = 0; i < listOfRows.Count; i++)
            {
                map[i] = listOfRows[i];
            }
            return map;
        }

        public static Option<Point> FindStart(char[][] argMap)
        {
            var firstRow = argMap[0];
            for (int i = 0; i < firstRow.Length; i++)
            {
                if (firstRow[i] == '|') return Option<Point>.Create(new Point(i, 0));
            }
            return Option<Point>.CreateEmpty();
        }

        public static char[][] GetWindow(char[][] argMap, Point argWindowCenter)
        {
            const int size = 5;
            var window = new char[size][];
            MarkWholeWindow(size, window, 'x');

            //top top row
            if ((argWindowCenter.Y - 2) >= 0)
                window[0][2] = argMap[argWindowCenter.Y - 2][argWindowCenter.X];

            //top row
            if ((argWindowCenter.Y - 1) >= 0)
                window[1][2] = argMap[argWindowCenter.Y - 1][argWindowCenter.X];

            // middle row
            window[2] = new char[size] {
                (argWindowCenter.X - 2) >= 0 ? argMap[argWindowCenter.Y][argWindowCenter.X - 2] : 'x',
                (argWindowCenter.X - 1) >= 0 ? argMap[argWindowCenter.Y][argWindowCenter.X - 1] : 'x',
                argMap[argWindowCenter.Y][argWindowCenter.X],
                (argWindowCenter.X + 1) < argMap[argWindowCenter.Y].Length ? argMap[argWindowCenter.Y][argWindowCenter.X + 1] : 'x',
                (argWindowCenter.X + 2) < argMap[argWindowCenter.Y].Length ? argMap[argWindowCenter.Y][argWindowCenter.X + 2] : 'x',
            };

            //bottom row
            if ((argWindowCenter.Y + 1) < argMap.Length)
                window[3][2] = argMap[argWindowCenter.Y + 1][argWindowCenter.X];

            //bottom bottom row
            if ((argWindowCenter.Y + 2) < argMap.Length)
                window[4][2] = argMap[argWindowCenter.Y + 2][argWindowCenter.X];

            return window;
        }

        private static void MarkWholeWindow(int size, char[][] window, char defaultValue)
        {
            for (int i = 0; i < size; i++)
            {
                window[i] = new char[size];
                for (int j = 0; j < size; j++)
                {
                    window[i][j] = defaultValue;
                }

            }
        }
    }
}
