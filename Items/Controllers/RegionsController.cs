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
    public class RegionsController : ControllerBase
    {
        private readonly IRegionsService _regionsService;
        public RegionsController(IRegionsService regionsService)
        {
            _regionsService = regionsService;
        }

        ///<summary>
        /// Get All items
        ///</summary>
        [HttpGet]
        public async Task<IEnumerable<Region>> Get()
        {
            return await _regionsService.GetRegions();
        }

        ///<summary>
        /// Search Item by Name
        ///</summary>
        [HttpGet("{searchquery}")]
        public async Task<IEnumerable<Region>> Search(string searchquery)
        {
            return await _regionsService.SearchRegion(searchquery);
        }

        ///<summary>
        /// Add item
        ///</summary>
        [HttpPost]
        [Route("add")]
        public async Task AddItem([FromBody] Region region)
        {
            await _regionsService.AddRegion(region);
        }

        ///<summary>
        /// Update Item by Id
        ///</summary>
        [HttpPut("{id}")]
        public async Task UpdateItem(int id, [FromBody] Region region)
        {
            await _regionsService.UpdateRegion(id, region.CategoryName,region.CategoryValue,region.ParentId);
        }

        ///<summary>
        /// Delete Item by Id
        ///</summary>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _regionsService.DeleteRegion(id);
        }
    }
}
