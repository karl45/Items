using Items.Database;
using Items.Database.Repositories;
using Items.Database.Repositories.IRepositories;
using Items.Service.Abstractions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ItemsTest
{
    public class TestOrders
    {
        [Fact]
        public async Task TestGettingAllOrders()
        {
            var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddEnvironmentVariables()
                 .Build();

            var dbContext = new DbContext(config);
            var IOrderRepo = new OrderRepository(dbContext);


            var orderMockService = new Mock<IOrdersService>();
            orderMockService.Setup(x => x.GetOrders(1,10)).Returns(IOrderRepo.GetOrders());
            var orders = await orderMockService.Object.GetOrders(1,10);

            Assert.NotEmpty(orders);
        }
    }
}
