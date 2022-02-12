using Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Database.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders(int page = 1, int count = 10);
        Task AddOrder(Order order);
        Task<IList<Order>> GetAllOrders();
        Task DeleteOrder(int id);
        Task UpdateOrder(int id, int regionId, DateTime orderDate, int itemId, int amount);
        Task<IEnumerable<Order>> SearchOrder(string search);
    }
}
