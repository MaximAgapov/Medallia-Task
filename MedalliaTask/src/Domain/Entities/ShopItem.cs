using MedalliaTask.Domain.Common;

namespace MedalliaTask.Domain.Entities
{
    public class ShopItem : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public double Price { get; set; } 
    }
}