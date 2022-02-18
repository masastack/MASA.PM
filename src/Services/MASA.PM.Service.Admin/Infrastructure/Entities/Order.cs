namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;

        public string OrderNumber { get; set; } = default!;

        public string Address { get; set; } = default!;
    }
}