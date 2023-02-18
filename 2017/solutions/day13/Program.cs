using System;
using System.Collections.Generic;
using System.IO;

namespace solutions.day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<(int timeStamp, int range)>();
            using (var sr = new StreamReader("realTestData.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var lineChunks = line.Split(':');
                    int.TryParse(lineChunks[0].Trim(), out int timeStamp);
                    int.TryParse(lineChunks[1].Trim(), out int range);

                    data.Add((timeStamp, range));
                }
            }

            int delay = 0;
            while (true)
            {
                delay++;
                var severity = SeverityCalculator.CalculateWithDelay(data,delay);
                if ((delay % 10000) == 0) Console.WriteLine($"delay: {delay} - severity {severity}");
                if (severity == 0) break;
            }

            Console.WriteLine(delay);
        }
    }
}
