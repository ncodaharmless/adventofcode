using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Day11
    {
        private int SerialNumber;
        int[,] grid = new int[300, 300];

        public Day11(int serial = 9798)
        {
            SerialNumber = serial;
            for (int x = 1; x <= 300; x++)
                for (int y = 1; y <= 300; y++)
                    grid[x - 1, y - 1] = PowerLevel(x, y);
        }

        private int RackId(int x, int y) => x + 10;

        public int PowerLevel(int x, int y)
        {
            int power = RackId(x, y) * y + SerialNumber;
            power = power * RackId(x, y);
            power = power / 100 % 10;
            return power - 5;
        }

        public void Largest3x3()
        {
            Largest(3, 3);
        }

        public void LargestAny()
        {
            Largest(1, 300);
        }

        private void Largest(int gridSizeStart, int gridSizeEnd)
        {
            int currentLargestPower = int.MinValue;
            int largestCoordX = 0;
            int largestCoordY = 0;
            int largestGridSize = 0;
            for (int gridSize = gridSizeStart; gridSize <= gridSizeEnd; gridSize++)
            {
                for (int x = 1; x <= 300 - gridSize + 1; x++)
                {
                    for (int y = 1; y <= 300 - gridSize + 1; y++)
                    {
                        int powerForGrid = PowerForGridSize(grid, x, y, gridSize);
                        if (powerForGrid > currentLargestPower)
                        {
                            largestCoordX = x;
                            largestCoordY = y;
                            largestGridSize = gridSize;
                            currentLargestPower = powerForGrid;
                        }
                    }
                }
                Console.WriteLine($"Tested size: {gridSize}");
            }
            Console.WriteLine($"Largest {largestCoordX},{largestCoordY} - size:{largestGridSize}");
        }

        private static int PowerForGridSize(int[,] power, int x, int y, int gridSize)
        {
            int powerForGrid = 0;
            for (int i1 = 0; i1 < gridSize; i1++)
                for (int i2 = 0; i2 < gridSize; i2++)
                    powerForGrid += power[x + i1 - 1, y + i2 - 1];
            return powerForGrid;
        }
    }

    [TestClass]
    public class Day11Test
    {
        [TestMethod]
        [DataRow(3, 5, 8, 4)]
        [DataRow(122, 79, 57, -5)]
        [DataRow(217, 196, 39, 0)]
        [DataRow(101, 153, 71, 4)]
        public void TestDefault(int x, int y, int serial, int expected)
        {
            Assert.AreEqual(expected, new Day11(serial).PowerLevel(x, y));
        }
    }

}
