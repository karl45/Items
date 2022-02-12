using Items.Models;
using Items.Service;
using Items.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Items.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsService _itemsService;
        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        ///<summary>
        /// Get All items
        ///</summary>
        [HttpGet]
        public async Task<IEnumerable<Item>> Get()
        {
            return await _itemsService.GetItems();
        }

        ///<summary>
        /// Search Item by Name
        ///</summary>
        [HttpGet("{searchquery}")]
        public async Task<IEnumerable<Item>> Search(string searchquery)
        {
            return await _itemsService.SearchItem(searchquery);
        }

        ///<summary>
        /// Add item
        ///</summary>
        [HttpPost]
        [Route("add")]
        public async Task AddItem([FromBody] Item item)
        {
            await _itemsService.AddItem(item);
        }

        ///<summary>
        /// Update Item by Id
        ///</summary>
        [HttpPut("{id}")]
        public async Task UpdateItem(int id, [FromBody] Item item)
        {
            await _itemsService.UpdateItem(id, item.Name, item.Price);
        }

        ///<summary>
        /// Delete Item by Id
        ///</summary>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _itemsService.DeleteItem(id);
        }
    }
}
