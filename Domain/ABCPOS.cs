using ABCHardwareWebApplication.TechnicalServices;
using ABCHardwareWebApplication.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;


namespace ABCHardwareWebApplication.Domain
{
    public class ABCPOS
    {
        public bool CreateItem(Item anItem)
        {
                bool Success = false;

                Items SaleManager = new();
                Success = SaleManager.AddItem(anItem);

                return Success;
        }
        public bool UpdateItem(Item anItem)
        {
            bool Success = false;

            Items SaleManager = new();
            Success = SaleManager.UpdateItem(anItem);
            return Success;
        }

        public bool UpdateItemQuantity(string itemCode, int StockBal)
        {
            bool Success = false;

            Items SaleManager = new();
            Success = SaleManager.UpdateItemQty(itemCode, StockBal);
            return Success;
        }
        public bool DeleteItem(string itemCode)
        {
            bool Success = false;

            Items SaleManager = new();
            Success = SaleManager.RemoveItem(itemCode);
            return Success;
        }

        public Item FindItem(string itemCode)
        {
            Item ExistingItem = new();

            Items SaleManager = new();
            ExistingItem = SaleManager.GetItem(itemCode);
            return ExistingItem;
        }


        public bool CreateCustomer(Customer aCustomer)
        {
            bool Success = false;

            Customers SaleManager = new();
            Success = SaleManager.AddCustomer(aCustomer);

            return Success;
        }
        public Customer FindCustomer(string  customerID)
        {
            Customer ExistingCustomer= new();

            Customers SaleManager = new();
            ExistingCustomer = SaleManager.GetCustomer(customerID);
            return ExistingCustomer;
        }
        public bool UpdateCustomer(Customer aCustomer)
        {
            bool Success = false;

            Customers SaleManager = new();
            Success = SaleManager.UpdateCustomer(aCustomer);
            return Success;
        }
        public bool DeleteCustomer(string customerID)
        {
            bool Success = false;

            Customers SaleManager = new();
            Success = SaleManager.DeleteCustomer(customerID);
            return Success;
        }
        public int ProcessSale(Sale mySale)
        {
            int saleNumber;

            Sales SaleManager = new();
            saleNumber = SaleManager.AddSales(mySale);
            return saleNumber;
        }


    }
}
