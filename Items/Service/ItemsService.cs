using Items.Database.Repositories.IRepositories;
using Items.Models;
using Items.Service.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Items.Service
{
    public class ItemsService:IItemsService
    {
        private readonly IItemsRepository _itemsRepository; 

        public ItemsService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
  
        public async Task<IEnumerable<Item>> GetItems()
        {
            try
            {
                return await _itemsRepository.GetItems();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddItem(Item item)
        {
            try
            {
                await _itemsRepository.AddItem(item);
            }
            catch
            {
                throw;
            }

        }

        public async Task DeleteItem(int id)
        {
            try
            {
               await _itemsRepository.DeleteItem(id);
            }
            catch
            {
                throw;
            }

        }

        public async Task UpdateItem(int id,string name,decimal price)
        {
            try
            {
                await _itemsRepository.UpdateItem(id, name, price);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Item>> SearchItem(string search)
        {
            try
            {
                return await _itemsRepository.SearchItem(search);
            }
            catch
            {
                throw;
            }
        }
    }
}
