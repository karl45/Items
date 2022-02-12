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
    public class OrderRepository : IOrderRepository
    {

        private readonly DbContext _dbContext;
        private readonly IDbConnection DapperConnection;

        public OrderRepository(DbContext dbContext)
        {
            _dbContext = dbContext;

            DapperConnection = _dbContext.GetDbConnection();
        }

        public async Task AddOrder(Order order)
        {
            var query = @$"INSERT INTO [Orders]([RegionId]
                                                   ,[OrderDate]
                                                   ,[ItemId]
                                                   ,[Amount])
                            VALUES ({order.RegionId},'{order.OrderDate}',{order.ItemId},{order.Amount})";

            await DapperConnection.ExecuteAsync(query);

            DapperConnection.Close();
        }

        public async Task DeleteOrder(int id)
        {
            var query = @$"DELETE FROM [Orders] where Id = {id}";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }

        public async Task<IList<Order>> GetAllOrders()
        {
            var query = @$"SELECT * FROM [ORDERS]";

            var orders = (await DapperConnection.QueryAsync<Order>(query)).ToList();
            DapperConnection.Close();
            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrders(int page = 1, int count = 10)
        {
            var query = @$"EXEC	[dbo].[PaginationOrder]
		                            @count = {count},
		                            @offset = {count * (page - 1)}";

            var orders = await DapperConnection.QueryAsync<Order>(query);

            DapperConnection.Close();
            return orders;
        }

        public async Task<IEnumerable<Order>> SearchOrder(string search)
        {
            var query = @$"select o.* from Orders o
                          left join Regions r on o.RegionId = r.Id
                          left join Items i on o.ItemId = i.Id
                          where r.CategoryValue like '%{search.ToUpper()}%' or i.Name like '%{search}%'";

            var searchregions = await DapperConnection.QueryAsync<Order>(query);
            DapperConnection.Close();

            return searchregions;
        }

        public async Task UpdateOrder(int id, int regionId, DateTime orderDate, int itemId, int amount)
        {
            var query = @$"UPDATE [Order] SET RegionId = {regionId}, 
                                    OrderDate = '{orderDate}',
                                    ItemId = {itemId},Amount = {amount} where Id = {id}";

            await DapperConnection.ExecuteAsync(query);
            DapperConnection.Close();
        }
    }
}
