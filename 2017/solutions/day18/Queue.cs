using System;
using System.Collections.Generic;
using System.Threading;

namespace solutions.day18
{
    public class Queue
    {
        private IList<long> messages;
        private object _lock = new object();
        public Queue()
        {
            messages = new List<long>();
        }

        public void Add(long message)
        {
            lock (_lock)
            {
                messages.Add(message);
            }
            SomeMessagesExist.Set();
        }

        public string Read()
        {
            SomeMessagesExist.WaitOne();
            lock (_lock)
            {
                var toReturn = messages[0].ToString();
                messages.RemoveAt(0);

                if (messages.Count == 0)
                    SomeMessagesExist.Reset();

                return toReturn;
            }
        }

        public ManualResetEvent SomeMessagesExist = new ManualResetEvent(false);

        public void OnMessageSend(object sender, long e)
        {
            Add(e);
        }
    }

}
