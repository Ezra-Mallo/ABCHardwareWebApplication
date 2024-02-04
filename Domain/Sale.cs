using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ABCHardwareWebApplication.Domain
{
    public class Sale
    {
        public int SaleNumber { get; set; }
        
        public string Salesperson { get; set; } = string.Empty;
        public int CustomerID { get; set; } 
        public decimal SubTotal { get; set; }
        public decimal GST { get; set; }
        public decimal SaleTotal { get; set; }

    }
}
