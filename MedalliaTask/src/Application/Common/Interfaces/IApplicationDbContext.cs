using MedalliaTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MedalliaTask.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ShopItem> ShopItems { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItem { get; set; }
        DbSet<SpecialOffer> SpecialOffer { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}