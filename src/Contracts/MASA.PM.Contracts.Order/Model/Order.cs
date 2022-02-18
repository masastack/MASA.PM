namespace MASA.PM.Contracts.Order.Model
{
    public class Order
    {
        public DateTime CreateTime { get; set; }

        public int Id { get; set; }

        public string OrderNumber { get; set; } = "";

        public string Address { get; set; } = default!;
    }
}