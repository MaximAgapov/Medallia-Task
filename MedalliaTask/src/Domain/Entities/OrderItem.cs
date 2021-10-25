using MedalliaTask.Domain.Common;

namespace MedalliaTask.Domain.Entities
{
    public class OrderItem : AuditableEntity
    {
        public int Id { get; set; }
        
        public ShopItem Item { get; set; }
        public int Amount { get; set; }
    }
}