namespace ConfigureAwaitSample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                // await Print(i).ConfigureAwait(false);


                Console.WriteLine(i);
            }

            Console.ReadLine();
        }

        static async Task Print(int i)
        {
            Console.WriteLine(i);
            await Task.Delay(1000);
        }
    }
}
