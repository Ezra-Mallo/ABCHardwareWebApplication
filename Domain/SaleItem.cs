namespace ABCHardwareWebApplication.Domain
{
    public class SaleItem
    {
        public int SaleNumber { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal ItemTotal { get; set; }

    }
}
