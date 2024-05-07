using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolSample
{
    internal class MyThreadPool
    {
        private static ulong currentId = 0;
        private static readonly BlockingCollection<(ulong, Action, ExecutionContext?)> actions = [];

        public static void QueueUserWorkItem(Action action) => actions.Add((Interlocked.Increment(ref currentId), action, ExecutionContext.Capture()));

        static MyThreadPool()
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var t = new Thread((threadId) =>
                {
                    Console.WriteLine($"Thread #{Environment.CurrentManagedThreadId} started!");

                    while (true)
                    {
                        (ulong id, Action action, ExecutionContext? context) = actions.Take();

                        Console.WriteLine($"Thread #{Environment.CurrentManagedThreadId} executes task #{id}");

                        if (context is null)
                        {
                            action();
                        }
                        else
                        {
                            ExecutionContext.Run(context, state => ((Action)state!).Invoke(), action);
                        }
                    }
                })
                {
                    IsBackground = true
                };
                t.Start();
            }
        }
    }
}
