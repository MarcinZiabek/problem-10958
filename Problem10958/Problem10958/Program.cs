using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem10958
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var values in GenerateSequences())
            {
                Console.WriteLine(values.ToText());
            }
        }
        
        static IEnumerable<List<int>> GenerateSequences()
        {
            for (var i = 0; i <= 255; i++)
            {
                var result = new List<int>();
                var number = 1;

                for (var j = 2; j <= 10; j++)
                {
                    if (((1 << (j-2)) & i) != 0)
                    {
                        number = number * 10 + j;
                    }
                    else
                    {
                        result.Add(number);
                        number = j;
                    }
                }

                yield return result;
            }
        }
    }

    static class Helpers
    {
        public static string ToText(this List<int> values)
        {
            return string.Join(" ", values.Select(x => x.ToString()));
        }
    }
}