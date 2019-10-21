using System;
namespace AdventOfCode.Utils
{
    public class Lex
    {
        public void Run()
        {
            var result = NextOrder(new int[] { 0, 1, 2 });
            Console.WriteLine(string.Join(",", result));

            while (true)
            {
                result = NextOrder(result);
                if (result == null) break;
                Console.WriteLine(string.Join(",", result));
            }
        }

        public int[] NextOrder(int[] input)
        {
            int[] result = new int[input.Length];
            Array.Copy(input, result, input.Length);

            // indexA = largest index where next is larger
            int indexA = -1;
            for (int i = result.Length - 2; i >= 0; i--)
            {

                if (result[i] < result[i + 1])
                {
                    indexA = i;
                    break;
                }
            }
            if (indexA == -1) return null;

            // indexB = largest index where number is larger than above
            int indexB = -1;
            for (int i = result.Length - 1; i >= 0; i--)
            {
                if (result[indexA] < result[i])
                {
                    indexB = i;
                    break;
                }
            }
            if (indexA == -1) return null;

            Console.WriteLine(indexA);
            Console.WriteLine(indexB);

            // swap(a,b)
            Swap(result, indexA, indexB);

            // reverse after a+1;
            int j = indexA + 1;
            while (j < input.Length - j)
            {
                Swap(result, j, input.Length - j);
                j++;
            }

            return result;
        }

        private void Swap(int[] array, int indexA, int indexB)
        {
            int temp = array[indexA];
            array[indexA] = array[indexB];
            array[indexB] = temp;
        }
    }
}