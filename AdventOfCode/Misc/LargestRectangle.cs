using System;

namespace AdventOfCode.Utils
{
    public class LargestRectangle
    {

        public void Run()
        {

            var histogramData = new int[] { 1, 3, 6, 3, 0, 2, 6, 6, 1, 0, 3, 7 };
            histogramData = new int[] { 2, 1, 5, 6, 4, 4, 3 };
            int result = LargestHistogramArea(histogramData);
            Console.WriteLine($"Area = {result}");
        }

        private int LargestHistogramArea(int[] input)
        {
            int currentLargest = 0;
            int indexForHeight = -1;
            for (int i = 0; i < input.Length; i++)
            {
                // test with current one being largest
                int minX = i;
                int maxX = i;
                while (minX > 0 && input[minX - 1] >= input[i]) minX--;
                while (maxX < input.Length - 1 && input[maxX + 1] >= input[i]) maxX++;
                int width = maxX - minX + 1;
                int area = width * input[i];
                if (area > currentLargest)
                {
                    currentLargest = area;
                    indexForHeight = i;
                }
            }
            Console.WriteLine(indexForHeight);
            return currentLargest;
        }
    }
}