using MedalliaTask.Domain.Common;

namespace MedalliaTask.Domain.Entities
{
    public class SpecialOffer : AuditableEntity
    {
        public int Id { get; set; }

        public ShopItem ShopItem { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}