using System.Linq;
using System.Threading.Tasks;
using MedalliaTask.Application.Common.Exceptions;
using MedalliaTask.Application.Items.Queries.GetTotal;
using MedalliaTask.Domain.Entities;
using MedalliaTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MedalliaTask.Application.IntegrationTests.ShopItem.Queries
{
    using static Testing;

    public class CalcTotalQueryTests : TestBase
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly Order _order;

        public CalcTotalQueryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _order = new Order();
        }
        
        [Test]
        public async Task ShouldReturnValue()
        {
            var query = new CalcTotalQuery();
            var result = await SendAsync(query, context =>
            {
                var order = context.Orders.SingleOrDefault();
                var item = context.ShopItems.FirstOrDefault();
                order.OrderItems.Add(new OrderItem{Item = item, Amount = 4});
                context.SaveChanges();
            });
            Assert.AreNotEqual(0, result);
        }

        [Test]
        public async Task ShouldThrowIfNoActiveOrder()
        {
            _order.IsActive = false;
            
            var query = new CalcTotalQuery();
            
            Assert.ThrowsAsync<NoActiveOrderException>(async () => await SendAsync(query, context =>
            {
                var order = context.Orders.SingleOrDefault(x=>x.IsActive);
                order.IsActive = false;
                context.SaveChanges();
            }));
        }
    }
}