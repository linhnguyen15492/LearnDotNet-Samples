using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Repository
{
    public class ProductFindCreterias: PagingCreterias
    {
        public double MinPrice { get; set; } = double.MinValue;
        public double MaxPrice { get; set; } = double.MaxValue;
        public IEnumerable<Guid> Ids { get; set; } = Enumerable.Empty<Guid>();
        public string Name { get; set; } = string.Empty;

        public static ProductFindCreterias Empty => new() 
        { 
        };
    }
}
