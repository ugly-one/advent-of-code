namespace solutions.day14
{
    public static class HexToBinaryConverter
    {
        public static string Convert(string hexString)
        {
            string result = "";
            foreach (var letter in hexString)
            {
                var dec = int.Parse(letter.ToString(), System.Globalization.NumberStyles.HexNumber);
                string binary = System.Convert.ToString(dec, 2);

                string prefix = "";
                for (int i = binary.Length; i < 4; i++)
                {
                    prefix += "0";
                }
                result += prefix + binary;
            }

            return result;
        }
    }
}
