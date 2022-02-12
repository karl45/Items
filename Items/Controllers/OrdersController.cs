using Items.Models;
using Items.Service;
using Items.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Items.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }
        ///<summary>
        /// Get All items
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "Administrator,Customer")]
        public async Task<IEnumerable<Order>> Get(int page = 1,int count = 1)
        {
            return await _ordersService.GetOrders(page,count);
        }

        [HttpGet]
        [Route("FileGenerate")]
        [Authorize(Roles = "Administrator,Customer")]
        public async Task<IActionResult> GetFile()
        {
            var file = await _ordersService.GenerateFile();
           
            var memoryStream = new MemoryStream();

            using (var fileStream = new FileStream(file,FileMode.Open))
                   await fileStream.CopyToAsync(memoryStream);

            memoryStream.Position = 0;
            var contentType = "application/vnd.openxmlformats";
            return File(memoryStream, contentType, "Orders.xlsx");
         
        }
        ///<summary>
        /// Search Item by Name
        ///</summary>
        [HttpGet("{searchquery}")]
        [Authorize(Roles = "Administrator,Customer")]
        public async Task<IEnumerable<Order>> Search(string searchquery)
        {
            return await _ordersService.SearchOrder(searchquery);
        }

        ///<summary>
        /// Add item
        ///</summary>
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Administrator,Customer")]

        public async Task AddOrder([FromBody] Order order)
        {
            await _ordersService.AddOrder(order);
        }

        ///<summary>
        /// Update Item by Id
        ///</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task UpdateOrder(int id, [FromBody] Order order)
        {
            await _ordersService.UpdateOrder(id, order.RegionId, order.OrderDate,order.ItemId,order.Amount);
        }

        ///<summary>
        /// Delete Item by Id
        ///</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task DeleteOrder(int id)
        {
            await _ordersService.DeleteOrder(id);
        }
    }
}
