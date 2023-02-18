using System.Collections.Generic;
using System.IO;

namespace Common
{
    public  class FileReader
    {
        public static IEnumerable<string> Read(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    yield return sr.ReadLine();
                }
            }
        }
    }
}
