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
    public class ItemsRepository:IItemsRepository
    {
        private readonly DbContext _dbContext;
        private readonly IDbConnection DapperConnection;
        public ItemsRepository(DbContext dbContext)
        {
            _dbContext = dbContext;

            DapperConnection = _dbContext.GetDbConnection();
        }

        public async Task AddItem(Item item)
        {
            var query = @$"INSERT INTO [Items]([Name],[Price])
                            VALUES ('{item.Name}',{item.Price})";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }

        public async Task DeleteItem(int id)
        {
            var query = @$"DELETE FROM [Items] where Id = {id}";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            var query = @$"SELECT * FROM [Items]";

            var items = await DapperConnection.QueryAsync<Item>(query);
            DapperConnection.Close();
            
            return items;
        }

        public async Task<IEnumerable<Item>> SearchItem(string search)
        {
            var query = @$"SELECT * FROM [Items] 
                            where Name like '%{search}%'";

            var searchitems = await DapperConnection.QueryAsync<Item>(query);
            DapperConnection.Close();
            return searchitems;
        }

        public async Task UpdateItem(int id, string name, decimal price)
        {
            var query = @$"UPDATE [Items] SET Name = '{name}', 
                                                   Price = {price} where Id = {id}";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }
    }
}
