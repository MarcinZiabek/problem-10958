using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Problem10958
{
    class Program
    {
        static void Main(string[] args)
        {
            var avg = GenerateSequences()
                .Select(x => x.Count-1)
                .Select(x => Math.Pow(4, x) * x.Factorial())
                .Sum();
            
            var sum = 0;
            var sw = new Stopwatch();
            sw.Start();
            
            foreach (var sequence in GenerateSequences())
            {
                Console.WriteLine(sequence.ToText());
                
                var results = Results(sequence.ToArray(), 0, sequence.Count-1).Count();

                Console.WriteLine($"Solutions: {results}");
                sum += results;
            }
            
            sw.Stop();
            Console.WriteLine($"Found: {sum} solutions during {sw.ElapsedMilliseconds} ms");
        }

        static IEnumerable<double> Results(int[] sequence, int start, int end)
        {
            if (start == end)
            {
                yield return sequence[start];
                yield break;
            }
            
            for (var split = start; split < end; split++)
            {
                var left = Results(sequence, start, split);
                var right = Results(sequence, split+1, end).ToList();

                foreach (var leftValue in left)
                {
                    foreach (var rightValue in right)
                    {
                        yield return leftValue + rightValue;
                        yield return leftValue - rightValue;
                        yield return leftValue * rightValue;
                        yield return leftValue / rightValue;
                        yield return Math.Pow(leftValue, rightValue);
                    } 
                }
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
        static readonly int[] FactorialCache = {1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880};
        
        public static string ToText(this List<int> values)
        {
            return string.Join(" ", values.Select(x => x.ToString()));
        }
        
        public static int Factorial(this int value)
        {
            return FactorialCache[value];
        }
        
        public static bool IsEqualTo(this double value, int target, double delta=10e-6)
        {
            return Math.Abs(value - target) < delta;
        }
    }
}