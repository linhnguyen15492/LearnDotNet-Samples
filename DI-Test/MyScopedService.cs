using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Test
{
    internal class MyScopedService: IMyScopedService
    {
        private static int Id = 0;
        public MyScopedService() {
            Console.WriteLine($"Scoped!: {++Id}");
        }
    }
}
