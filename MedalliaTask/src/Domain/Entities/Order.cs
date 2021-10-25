using System.Collections;
using System.Collections.Generic;
using MedalliaTask.Domain.Common;

namespace MedalliaTask.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public bool IsActive { get; set; } = true;
    }
}