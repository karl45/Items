using Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Service.Abstractions
{
    public interface IItemsService
    {
        Task<IEnumerable<Item>> GetItems();
        Task AddItem(Item item);
        Task DeleteItem(int id);
        Task UpdateItem(int id, string name, decimal price);
        Task<IEnumerable<Item>> SearchItem(string search);
    }
}
