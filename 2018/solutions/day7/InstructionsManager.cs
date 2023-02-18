using System.Collections.Generic;
using System.Linq;

namespace Solutions.day7
{
    public class InstructionsManager
    {
        private IEnumerable<Worker> m_workers;

        public InstructionsManager(IEnumerable<Worker> workers)
        {
            m_workers = workers;
        }

        public InstructionsManager(Worker worker)
        {
            m_workers = new Worker[1] { worker };
        }

        public IEnumerable<Step> Order(IEnumerable<Step> steps, out uint elapsedTime)
        {
            var availableSteps = steps.ToList();
            var result = new List<Step>(steps.Count());
            uint timeElapsed = 0;

            while (true)
            {
                // find all steps that can be solved now.
                var canBeSolvedSteps = availableSteps.Where(s => s.CanBeDone(result));

                // order them
                var orderedCanBeSolvedSteps = canBeSolvedSteps.OrderBy(s => s.Value);

                // distribute the steps to workers
                foreach (var step in orderedCanBeSolvedSteps)
                {
                    // find an available worker and give him the task
                    var availableWorkers = m_workers.Where(w => !w.IsWorking());
                    if (availableWorkers.Any())
                    {
                        // delegate the work
                        availableWorkers.First().Work(step);
                        availableSteps.Remove(step);
                    }
                }

                //increase time
                timeElapsed++;
                List<Step> doneSteps = new List<Step>();
                foreach (var worker in m_workers)
                {
                    doneSteps.Add(worker.IncreaseTime());
                }
                // are there any steps that are done?
                if (doneSteps.Any(s => !(s is null))){
                    // order them
                    doneSteps = doneSteps.Where(s => !(s is null)).OrderBy(s => s.Value).ToList();
                    foreach (var step in doneSteps)
                    {
                        result.Add(step);
                    }
                }
                if (result.Count() == steps.Count())
                    break;
            }
            elapsedTime = timeElapsed;
            return result;
        }
    }
}
