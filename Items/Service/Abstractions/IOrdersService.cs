using Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Service.Abstractions
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetOrders(int page = 1, int count = 10);
        Task AddOrder(Order order);
        Task<string> GenerateFile();
        Task DeleteOrder(int id);
        Task UpdateOrder(int id, int regionId, DateTime orderDate, int itemId, int amount);
        Task<IEnumerable<Order>> SearchOrder(string search);
    }
}
