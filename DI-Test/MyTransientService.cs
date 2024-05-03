using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Test
{
    internal class MyTransientService: IMyTransientService
    {
        private static int Id = 0;
        public MyTransientService()
        {
            Console.WriteLine($"Transient!: {++Id}");
        }
    }
}
