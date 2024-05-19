using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.OrderReferenceGenerators
{
    internal class FixedStringOrderReferenceGenerator (string id) : IOrderReferenceGenerator // for testing only
    {
        public string Next(int length)
        {
            return id;
        }
    }
}
