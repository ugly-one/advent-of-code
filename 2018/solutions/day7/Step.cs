using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solutions.day7
{
    [DebuggerDisplay("{Value}")]
    public class Step
    {
        private IList<Step> stepsAfter;
        private Step[] stepsToBeDoneBeforeThisOneCanBeDone;

        public Step(char Value)
        {
            this.Value = Value;
            stepsAfter = new List<Step>();
            stepsToBeDoneBeforeThisOneCanBeDone = new Step[26];
        }

        public bool IsBlocked { get; private set; }
        public char Value { get; set; }

        public void AddStepAfter(Step required)
        {
            stepsAfter.Add(required);
        }

        public void AddRequiredStep(Step required)
        {
            stepsToBeDoneBeforeThisOneCanBeDone[(int)required.Value - 65] = required;
        }

        public void SetIsBlocked()
        {
            IsBlocked = true;
        }

        internal IEnumerable<Step> GetNextSteps()
        {
            return stepsAfter;
        }

        internal bool CanBeDone(IEnumerable<Step> stepsDone)
        {
            var copyOfRequiredSteps = stepsToBeDoneBeforeThisOneCanBeDone.ToArray();

            foreach (var step in stepsDone)
            {
                copyOfRequiredSteps[step.Value - 65] = null;
            }
            if (copyOfRequiredSteps.Any(s => !(s is null)))
            {
                return false;
            }
            return true;
        }
    }
}