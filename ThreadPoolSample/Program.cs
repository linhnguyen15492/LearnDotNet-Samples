namespace ThreadPoolSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 100; i++) {
                int capturedI = i;

                MyThreadPool.QueueUserWorkItem(() => {
                    Console.WriteLine($"{capturedI} (thread #{Environment.CurrentManagedThreadId})");
                });
            }

            Console.ReadLine();
        }
    }
}
