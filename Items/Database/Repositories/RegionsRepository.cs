using Dapper;
using Items.Database.Repositories.IRepositories;
using Items.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Database.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly DbContext _dbContext;
        private readonly IDbConnection DapperConnection;
        public RegionsRepository(DbContext dbContext)
        {
            _dbContext = dbContext;

            DapperConnection = _dbContext.GetDbConnection();
        }

        public async Task AddRegion(Region region)
        {
            var ParentId = "";
            if (region.ParentId <= 0) ParentId = "NULL";
            else ParentId = region.ParentId.ToString();

            var query = @$"INSERT INTO [Regions](
                                            [CategoryName],
                                            [CategoryValue],
                                            [ParentId])
                            VALUES ('{region.CategoryName.ToUpper()}',
                                    '{region.CategoryValue.ToUpper()}',
                                     {ParentId})";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }

        public async Task DeleteRegion(int id)
        {

            var query = @$"DELETE FROM [Regions] where Id = {id}";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }

        public async Task<IEnumerable<Region>> GetRegions()
        {
            var query = @$"EXEC [dbo].[SELECTREGIONSBYHIERARCHY]";

            var regions = await DapperConnection.QueryAsync<Region>(query);

            DapperConnection.Close();
            return regions;
        }

        public async Task<IEnumerable<Region>> SearchRegion(string search)
        {
            var query = @$"SELECT * FROM [Regions] 
                            where CategoryValue like '%{search.ToUpper()}%'";

            var searchregions = await DapperConnection.QueryAsync<Region>(query);
            DapperConnection.Close();

            return searchregions;
        }

        public async Task UpdateRegion(int id, string categoryName, string categoryValue, int? parentId)
        {

            var query = @$"UPDATE [Regions] SET CategoryName = '{categoryName.ToUpper()}', 
                                    CategoryValue = '{categoryValue.ToUpper()}',
                                    ParentId = {parentId} where Id = {id}";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }
    }
}
