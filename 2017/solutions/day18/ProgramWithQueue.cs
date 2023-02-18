using System;
using System.Threading;
using System.Threading.Tasks;

namespace solutions.day18
{
    public class ProgramWithQueue : MyProgram
    {
        public readonly Queue m_queue;
        public int ID;
        public EventHandler ProgramDone = delegate { };
        public bool IsProgramDone = false;
        public EventHandler<long> MessageSend = delegate { };
        public bool IsWaiting { get; internal set; }
        public int SendCounter { get; internal set; }

        public ProgramWithQueue(Queue argQueue, int argId)
        {
            m_queue = argQueue;
            ID = argId;
            this.memory.Add('p', ID.ToString());
            SendCounter = 0;
        }

        public long ReadValue(char argRegisterName)
        {
            return memory.ReadValue(argRegisterName);
        }

        internal override void Receive(char argName)
        {
            IsWaiting = true;
            var value = m_queue.Read();
            IsWaiting = false;
            memory.Set(argName, value);
        }

        protected override void Send(char argName)
        {
            if (!long.TryParse(argName.ToString(), out long value))
                value = memory.ReadValue(argName);

            SendCounter++;
            MessageSend.Invoke(this, value);
        }

        public override void Run()
        {
            Task internalTask = new Task(() =>
            {
                base.Run();
                IsProgramDone = true;
                ProgramDone.Invoke(this, new EventArgs());
            });
            internalTask.Start();
        }
    }
}
