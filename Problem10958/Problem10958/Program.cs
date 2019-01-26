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
            var values = new HashSet<int>();
            
            var avg = GenerateSequences()
                .Select(x => x.Count-1)
                .Select(x => Math.Pow(4, x) * x.Factorial())
                .Sum();
            
            var sw = new Stopwatch();
            sw.Start();

            foreach (var sequence in GenerateSequences())
            {
                Console.WriteLine(sequence.ToText());
  
                foreach (var result in Results(sequence.ToArray(), 0, sequence.Count-1))
                {
                    if (result.IsWholeNumber(out var number))
                        values.Add(number);
                }
            }
            
            sw.Stop();
            
            Console.WriteLine($"Found {values.Count} during {sw.ElapsedMilliseconds} ms");

            for (var i = -20_000; i < 20_000; i++)
            {
                if(!values.Contains(i))
                    Console.WriteLine(i);
            }
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
                var right = Results(sequence, split+1, end);

                if (end - split < 7)
                    right = right.ToList();

                foreach (var leftValue in left)
                {
                    foreach (var rightValue in right)
                    {
                        yield return -leftValue + rightValue;
                        yield return leftValue + rightValue;
                        yield return leftValue - rightValue;
                        yield return -leftValue - rightValue;
                        yield return leftValue * rightValue;
                        yield return leftValue / rightValue;
                        
                        yield return -leftValue * rightValue;
                        yield return -leftValue / rightValue;
                        
                        var power = Math.Pow(leftValue, rightValue);

                        if (power > -10e15 && power < 10e15)
                        {
                            yield return Math.Pow(leftValue, rightValue);
                            yield return -Math.Pow(leftValue, rightValue);
                        }
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
        
        public static bool IsWholeNumber(this double value, out int result, double delta=10e-12)
        {
            result = (int)Math.Round(value);
            return Math.Abs(value - result) < delta;
        }
    }
}