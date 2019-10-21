using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2018
{
    public class Day07
    {
        class Step
        {
            public string Id { get; set; }
            public int WorkerId;
            public List<string> Dependencies = new List<string>();
            public int FinishBy;
            public int Duration
            {
                get
                {
                    int charId = (int)Id[0] - 'A' + 1;
                    return 60 + charId;
                }
            }
            public void CalculateFinish(int currentTime)
            {

                FinishBy = currentTime + Duration;
            }
        }

        public void Run()
        {
            Dictionary<string, Step> steps = new Dictionary<string, Step>();
            foreach (string line in System.IO.File.ReadAllLines("day07.txt"))
            {
                string key = line.Split(' ')[7];
                string dep = line.Split(' ')[1];
                if (!steps.ContainsKey(key))
                    steps.Add(key, new Step() { Id = key });
                if (!steps.ContainsKey(dep))
                    steps.Add(dep, new Step() { Id = dep });
                steps[key].Dependencies.Add(dep);
            }
            List<Step> stepsList = new List<Step>(steps.Values);

            // Console.WriteLine("Order: ");
            // while(steps.Count > 0)
            // {
            //     Step next = steps.Values.Where(s => s.Dependencies.Count == 0).OrderBy(s => s.Id).First();
            //     steps.Remove(next.Id);
            //     foreach(var item in steps.Values){
            //         item.Dependencies.Remove(next.Id);
            //     }
            //     Console.Write(next.Id);
            // }
            // Console.WriteLine();

            int timeCounter = 0;
            string[] workerSteps = new string[5];
            while (stepsList.Count > 0)
            {
                // mark jobs which have finished
                foreach (var step in stepsList.ToArray())
                {
                    if (step.FinishBy > 0)
                        if (step.FinishBy == timeCounter)
                        {
                            Console.WriteLine($"Time: {timeCounter} Finished step {step.Id} with worker {step.WorkerId}");
                            workerSteps[step.WorkerId] = null; // free up worker
                            stepsList.Remove(step);
                            foreach (var item in stepsList)
                                item.Dependencies.Remove(step.Id);
                        }
                }
                while (workerSteps.Any(w => w == null))
                {
                    Step next = stepsList.Where(s => s.Dependencies.Count == 0 && s.FinishBy == 0).OrderBy(s => s.Duration).FirstOrDefault();
                    if (next != null)
                    {
                        for (int w = 0; w < 5; w++)
                            if (workerSteps[w] == null)
                            {
                                workerSteps[w] = next.Id;
                                next.WorkerId = w;
                                break;
                            }
                        next.CalculateFinish(timeCounter);
                        Console.WriteLine($"Time: {timeCounter} Starting step {next.Id} with worker {next.WorkerId} (due {next.FinishBy})");
                    }
                    else break;
                }
                timeCounter++;
            }
            Console.WriteLine("Total time: " + timeCounter);
        }
    }
}