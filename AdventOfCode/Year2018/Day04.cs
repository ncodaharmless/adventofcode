using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2018
{
    public class Day04
    {

        class Record
        {
            public DateTime EventTime;
            public string Description;

            public Record(string line)
            {
                EventTime = DateTime.Parse(line.Substring(1, 16));
                Description = line.Substring(19);
            }
        }

        class SleepInterval
        {
            public int GuardId;
            public int StartMinute;
            public int DurationMinutes;
            public int WakeMinute;
        }

        public void Run()
        {
            string[] lines = System.IO.File.ReadAllLines("day04.txt");

            List<Record> list = new List<Record>();
            foreach (string line in lines)
            {
                list.Add(new Record(line));
            }
            list = list.OrderBy(l => l.EventTime).ToList();

            List<SleepInterval> sleep = new List<SleepInterval>();
            int currentGuard = -1;
            DateTime sleepTime = DateTime.MinValue;
            Dictionary<int, int> guardSleepTime = new Dictionary<int, int>();
            foreach (var item in list)
            {
                if (item.Description.StartsWith("Guard #"))
                    currentGuard = Convert.ToInt32(item.Description.Substring(6).Split(' ')[0].TrimStart('#'));
                else if (item.Description.StartsWith("wakes"))
                {
                    var i = new SleepInterval();
                    i.GuardId = currentGuard;
                    i.StartMinute = sleepTime.Minute;
                    i.WakeMinute = item.EventTime.Minute;
                    i.DurationMinutes = i.WakeMinute - i.StartMinute;
                    sleep.Add(i);
                    if (guardSleepTime.ContainsKey(currentGuard))
                        guardSleepTime[currentGuard] = guardSleepTime[currentGuard] + i.DurationMinutes;
                    else
                        guardSleepTime.Add(currentGuard, i.DurationMinutes);
                }
                else if (item.Description.StartsWith("falls"))
                    sleepTime = item.EventTime;
                else throw new NotSupportedException(item.Description);
            }
            var mostAsleep = guardSleepTime.OrderByDescending(g => g.Value).First();
            int guardId = mostAsleep.Key;

            Console.WriteLine("Guard ID " + guardId);

            int highestMinute = MinuteMostAsleep(sleep, guardId).Item1;

            Console.WriteLine("Minute " + highestMinute);
            Console.WriteLine(highestMinute * guardId);

            int maxCount = -1;
            int guardIdPart2 = -1;
            int minuteResult = -1;
            foreach (var guard in guardSleepTime.Keys)
            {
                var guardSleep2 = MinuteMostAsleep(sleep, guard);
                int count = guardSleep2.Item2;
                if (count > maxCount)
                {
                    maxCount = count;
                    guardIdPart2 = guard;
                    minuteResult = guardSleep2.Item1;
                }
            }

            Console.WriteLine("P2: Guard ID " + guardIdPart2);
            Console.WriteLine("P2: Minute " + minuteResult);
            Console.WriteLine(minuteResult * guardIdPart2);
        }

        private Tuple<int, int> MinuteMostAsleep(List<SleepInterval> list, int guardId)
        {
            var gaurdSleep = list.Where(g => g.GuardId == guardId).ToArray();
            int[] minuteCount = new int[60];
            foreach (var interval in gaurdSleep)
            {
                for (int i = interval.StartMinute; i < interval.WakeMinute; i++)
                {
                    minuteCount[i]++;
                }
            }
            int highestMinute = 0;
            int maxCount = 0;
            for (int i = 0; i < minuteCount.Length; i++)
            {
                if (minuteCount[i] > maxCount)
                {
                    maxCount = minuteCount[i];
                    highestMinute = i;
                }
            }
            return new Tuple<int, int>(highestMinute, maxCount);
        }
    }
}