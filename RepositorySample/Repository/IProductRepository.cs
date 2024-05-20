using RepositorySample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Repository
{
    internal interface IProductRepository
    {
        Product? FindById(Guid id);
        IEnumerable<Product> Find(ProductFindCreterias creterias, ProductSortBy sortBy = ProductSortBy.NameAscending);
        Product? Add(Product product);
        int DeleteAll();
        int Update(Product product);
    }
}
