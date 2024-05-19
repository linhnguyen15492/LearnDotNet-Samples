using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample
{
    internal interface IOrderReferenceGenerator
    {
        string Next(int length);
    }
}
