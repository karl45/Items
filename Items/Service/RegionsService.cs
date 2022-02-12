using Items.Database.Repositories.IRepositories;
using Items.Models;
using Items.Service.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Items.Service
{
    public class RegionsService:IRegionsService
    {

        private readonly IRegionsRepository _regionsRepository;
        public RegionsService(IRegionsRepository regionsRepository)
        {
            _regionsRepository = regionsRepository;
        }

        public async Task<IEnumerable<Region>> GetRegions()
        {
            try
            {
                return await _regionsRepository.GetRegions();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddRegion(Region region)
        {
            try
            {
                await _regionsRepository.AddRegion(region);
            }
            catch
            {
                throw;
            }

        }

        public async Task DeleteRegion(int id)
        {
            try
            {
                await _regionsRepository.DeleteRegion(id);
            }
            catch
            {
                throw;
            }

        }

        public async Task UpdateRegion(int id, string categoryName, string categoryValue,int? parentId)
        {
            try
            {
                await _regionsRepository.UpdateRegion(id, categoryName, categoryValue, parentId);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Region>> SearchRegion(string search)
        {
            try
            {
                return await _regionsRepository.SearchRegion(search);
            }
            catch
            {
                throw;
            }
        }
    }
}
