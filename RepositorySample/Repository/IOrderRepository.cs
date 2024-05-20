using RepositorySample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Repository
{
    internal interface IOrderRepository
    {
        Order? FindById(Guid id);
        Order? FindByReference(string reference);
        IEnumerable<Order> Find(OrderFindCreterias creterias, OrderSortBy sortBy = OrderSortBy.ReferenceAscending);
        Order? Add(Order order);
        int DeleteAll();
    }
}
