using System;

namespace Solutions.day7
{
    public class Worker
    {
        public Worker(uint delay)
        {
            m_delay = delay;
        }

        public bool IsWorking()
        {
            return !(m_currentStep is null);
        }

        private Step m_currentStep;
        private uint m_delay;
        private uint remainingTime;

        internal void Work(Step step)
        {
            m_currentStep = step;
            remainingTime = (uint)(step.Value - 64) + m_delay;
        }

        /// <summary>
        /// Increse time and returns a step has been completed now. If any. If not - null
        /// </summary>
        /// <returns>The time.</returns>
        internal Step IncreaseTime()
        {
            if (m_currentStep is null) return null;
            remainingTime -= 1;
            if (remainingTime == 0)
            {
                var result = m_currentStep;
                m_currentStep = null;
                return result;
            }
            else return null;
        }
    }
}