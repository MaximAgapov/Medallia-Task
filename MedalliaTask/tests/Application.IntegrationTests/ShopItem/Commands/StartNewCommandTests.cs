using System.Threading.Tasks;
using MedalliaTask.Application.Items.Commands.StartNewCommand;
using MedalliaTask.Domain.Entities;
using NUnit.Framework;

namespace MedalliaTask.Application.IntegrationTests.ShopItem.Commands
{
    using static Testing;

    public class StartNewCommandTests : TestBase
    {
        [Test]
        public async Task ShouldDeactivateAllOldActiveOrders()
        {
            var command = new StartNewCommand();
            var newOrder = new Order();
            Assert.True(newOrder.IsActive);
            
            var result = await SendAsync(command, context =>
                {
                    context.Orders.Add(newOrder);
                    context.SaveChanges();
                }
            );
            Assert.False(newOrder.IsActive);
        }

        [Test]
        public async Task ShouldCreateNewActiveOrder()
        {
            var command = new StartNewCommand();
            var newOrder = new Order();
            var newerOrder = await SendAsync(command, context =>
                {
                    context.Orders.Add(newOrder);
                    context.SaveChanges();
                }
            );
            Assert.AreNotSame(newerOrder, newOrder);
            Assert.AreNotEqual(newerOrder.Id, newOrder.Id);
        }
    }
}