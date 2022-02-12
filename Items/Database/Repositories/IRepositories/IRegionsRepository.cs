using Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Database.Repositories.IRepositories
{
    public interface IRegionsRepository
    {
        Task<IEnumerable<Region>> GetRegions();
        Task AddRegion(Region region);
        Task DeleteRegion(int id);
        Task UpdateRegion(int id, string categoryName, string categoryValue, int? parentId);
        Task<IEnumerable<Region>> SearchRegion(string search);
    }
}
