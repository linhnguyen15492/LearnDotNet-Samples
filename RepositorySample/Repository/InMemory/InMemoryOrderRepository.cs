using RepositorySample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Repository.InMemory
{
    internal class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> orders = [];

        public Order? Add(Order order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));

            if (orders.Any(o => o.OrderReference == order.OrderReference))
            {
                throw new ArgumentException("Duplicated order reference");
            }

            orders.Add(order);

            return order;
        }

        public IEnumerable<Order> Find(OrderFindCreterias creterias, OrderSortBy sortBy = OrderSortBy.ReferenceAscending)
        {
            var query = from o in orders select o;

            if (creterias.Ids.Any())
            {
                query = query.Where(o => creterias.Ids.Contains(o.Id));
            }

            if (creterias.CustomerIds.Any())
            {
                query = query.Where(o => creterias.CustomerIds.Contains(o.CustomerId));
            }

            if (creterias.Skip > 0)
            {
                query = query.Skip(creterias.Skip);
            }

            if (creterias.Take > 0 && creterias.Take != int.MaxValue)
            {
                query = query.Take(creterias.Take);
            }

            if (sortBy == OrderSortBy.ReferenceAscending)
            {
                query = query.OrderBy(o => o.OrderReference);
            }
            else
            {
                query = query.OrderByDescending(o => o.OrderReference);
            }

            return query;
        }

        public Order? FindById(Guid id)
        {
            return orders.Where(o => o.Id == id).FirstOrDefault();
        }

        public Order? FindByReference(string reference)
        {
            return orders.Where(o => o.OrderReference == reference).FirstOrDefault();
        }
    }
}
