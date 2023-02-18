using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExt{
    public static bool ContainsAnagram(this IEnumerable<string> array, string word){
        
        foreach (var item in array)
        {
            if (isAnagram(item,word)) return true;
        }
        return false;
    }

    public static bool isAnagram(string s1, string s2){

            char[] chars = s1.ToCharArray();
            var chars2 = s2.ToCharArray().ToList();

            if (s1.Count() != s2.Count()) return false;

            foreach (var c in chars)
            {
                if (chars2.Contains(c)) {
                    chars2.Remove(c);
                }
                else {
                    return false;
                }
            }
            return true;
        }
}