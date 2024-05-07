using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitSample
{
    internal class MyThreadPool
    {
        private static readonly BlockingCollection<(Action, ExecutionContext?)> actions = [];

        public static void QueueUserWorkItem(Action action) => actions.Add((action, ExecutionContext.Capture()));

        static MyThreadPool()
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var t = new Thread(() =>
                {
                    while (true)
                    {
                        (Action action, ExecutionContext? context) = actions.Take();
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
