using System;
using DrillHub.Infrastructure;

namespace DrillHub.Model.Orders
{
    public class Order : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
