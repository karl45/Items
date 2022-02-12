using Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Service.Abstractions
{
    public interface IRegionsService
    {
        Task<IEnumerable<Region>> GetRegions();
        Task AddRegion(Region region);
        Task DeleteRegion(int id);
        Task UpdateRegion(int id, string categoryName, string categoryValue, int? parentId);
        Task<IEnumerable<Region>> SearchRegion(string search);
    }
}
