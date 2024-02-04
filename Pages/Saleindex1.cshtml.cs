using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ABCHardwareWebApplication.Domain;
using ABCHardwareWebApplication.TechnicalServices;

namespace ABCHardwareWebApplication.Pages
{
    public class Saleindex1Model : PageModel
    {
        [BindProperty]
        public int saleNumber {  get; set; }
        public void OnGet()
        {
            ABCPOS ABCHardware= new();
            Sale mySale;            
            mySale = new()
            {
                Salesperson = "'John Doe3", 
                CustomerID = 1, 
                SubTotal = 100, 
                GST = 5, 
                SaleTotal = 130
        };
            saleNumber = ABCHardware.ProcessSale(mySale);

        }
    }
}
