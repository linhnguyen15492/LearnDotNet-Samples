using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Test
{
    internal class MySingletonService: IMySingletonService
    {
        private static int Id = 0;
        public MySingletonService()
        {
            Console.WriteLine($"Singleton!: {++Id}");
        }
    }
}
