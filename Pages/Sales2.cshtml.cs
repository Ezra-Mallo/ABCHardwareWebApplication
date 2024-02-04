using ABCHardwareWebApplication.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ABCHardwareWebApplication.Pages
{
    public class Sales2Model : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public bool IsFindButtonDisabled;
        public bool ShowUpdateForm;
        [BindProperty]
        public List<string> ExistingItemCodes { get; set; } = new List<string>();
        [BindProperty]
        public List<string> ExistingDescriptions { get; set; } = new List<string>();

        [BindProperty] 
        public List<decimal> ExistingUnitPrices { get; set; } = new List<decimal>();

        [BindProperty] 
        public List<decimal> ExistingStockBals { get; set; } = new List<decimal>();

        [BindProperty] 
        public List<bool> ExistingStockFlags { get; set; } = new List<bool>();
        
        public string FormId => "updateCustomerForm";

        [BindProperty]
        public string Submit { get; set; } = string.Empty;
        public Customer existingCustomer { get; set; } = new();
        public Item existingItem { get; set; } = new();

        [BindProperty]
        public string findCustomerID { get; set; } = string.Empty;

        [BindProperty]
        public int CustomerID { get; set; }

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string Address { get; set; } = string.Empty;

        [BindProperty]
        public string City { get; set; } = string.Empty;

        [BindProperty]
        public string Province { get; set; } = string.Empty;

        [BindProperty]
        public string PostalCode { get; set; } = string.Empty;

        [BindProperty]
        public string itemCode { get; set; } = string.Empty;

        [BindProperty]
        public int Qty { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            ModelState.Clear();
            ABCPOS ABCHardware = new();
            switch (Submit)
            {
                case "Find":
                    if (string.IsNullOrEmpty(findCustomerID) || !Regex.IsMatch(findCustomerID, "^[0-9]{1,9}$"))
                    {
                        ModelState.AddModelError("findCustomerID", "CustomerID must be numbers.");
                    }

                    if (!int.TryParse(findCustomerID, out int result))
                    {
                        ModelState.AddModelError("findCustomerID", "CustomerID must be a number.");
                    }

                    if (ModelState.IsValid)
                    {
                        existingCustomer = ABCHardware.FindCustomer(findCustomerID);
                        if (existingCustomer != null)
                        {

                            //assigning initial value
                            HttpContext.Session.SetInt32("CustomerID", (int)existingCustomer.CustomerID);
                            HttpContext.Session.SetString("FirstName", (string)existingCustomer.FirstName);
                            HttpContext.Session.SetString("LastName", (string)existingCustomer.LastName);
                            HttpContext.Session.SetString("Address", (string)existingCustomer.Address);
                            HttpContext.Session.SetString("City", (string)existingCustomer.City);
                            HttpContext.Session.SetString("Province", (string)existingCustomer.Province);
                            HttpContext.Session.SetString("PostalCode", (string)existingCustomer.PostalCode);

                            CustomerID = (int)existingCustomer.CustomerID;
                            FirstName = (string)existingCustomer.FirstName;
                            LastName = (string)existingCustomer.LastName;
                            Address = (string)existingCustomer.Address;
                            City = (string)existingCustomer.City;
                            Province = (string)existingCustomer.Province;
                            PostalCode = (string)existingCustomer.PostalCode;

                            IsFindButtonDisabled = true;
                            ShowUpdateForm = true;
                            Message = "Below are the details of the Customer to be modified.";
                        }
                        else
                        {
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;
                            Message = "Records do not exist.";
                        }
                    }
                    else
                    {
                        Message = "Enter a valid Customer ID.";
                    }
                    break;

                case "Add":
                    ShowUpdateForm = true;
                    CustomerID = (int)HttpContext.Session.GetInt32("CustomerID")!;
                    FirstName = HttpContext.Session.GetString("FirstName")!;
                    LastName = HttpContext.Session.GetString("LastName")!;
                    Address = HttpContext.Session.GetString("Address")!;
                    City = HttpContext.Session.GetString("City")!;
                    Province = HttpContext.Session.GetString("Province")!;
                    PostalCode = HttpContext.Session.GetString("PostalCode")!;

                    existingItem = ABCHardware.FindItem(itemCode);

                    if (existingItem != null)
                    {
                        // Retrieve existing items from the session
                        var existingItemCodesJson = HttpContext.Session.GetString("ItemCodes") ?? "[]";
                        var existingDescriptionsJson = HttpContext.Session.GetString("Descriptions") ?? "[]";
                        var existingUnitPricesJson = HttpContext.Session.GetString("UnitPrices") ?? "[]";
                        var existingStockBalsJson = HttpContext.Session.GetString("StockBals") ?? "[]";
                        var existingStockFlagsJson = HttpContext.Session.GetString("StockFlags") ?? "[]";

                        var existingItemCodes = JsonConvert.DeserializeObject<List<string>>(existingItemCodesJson) ?? new List<string>();
                        var existingDescriptions = JsonConvert.DeserializeObject<List<string>>(existingDescriptionsJson) ?? new List<string>();
                        var existingUnitPrices = JsonConvert.DeserializeObject<List<decimal>>(existingUnitPricesJson) ?? new List<decimal>();
                        var existingStockBals = JsonConvert.DeserializeObject<List<decimal>>(existingStockBalsJson) ?? new List<decimal>();
                        var existingStockFlags = JsonConvert.DeserializeObject<List<bool>>(existingStockFlagsJson) ?? new List<bool>();

                        // Adding a new item
                        ExistingItemCodes.Add(existingItem.ItemCode);
                        ExistingDescriptions.Add(existingItem.Description);
                        ExistingUnitPrices.Add(existingItem.UnitPrice);
                        ExistingStockBals.Add(existingItem.StockBal);
                        ExistingStockFlags.Add(existingItem.StockFlag);

                        // Storing arrays back in session
                        HttpContext.Session.SetString("ItemCodes", JsonConvert.SerializeObject(existingItemCodes));
                        HttpContext.Session.SetString("Descriptions", JsonConvert.SerializeObject(existingDescriptions));
                        HttpContext.Session.SetString("UnitPrices", JsonConvert.SerializeObject(existingUnitPrices));
                        HttpContext.Session.SetString("StockBals", JsonConvert.SerializeObject(existingStockBals));
                        HttpContext.Session.SetString("StockFlags", JsonConvert.SerializeObject(existingStockFlags));

                        IsFindButtonDisabled = true;
                        ShowUpdateForm = true;
                        Message = "Item added to the sale.";
                    }
                    break;
                case "Process Sale":
                    //// Retrieve sale items from session
                    //var saleItems = new List<SaleItem>();
                    //var itemCodesJson = HttpContext.Session.GetString("ItemCodes") ?? "[]";
                    //var descriptionsJson = HttpContext.Session.GetString("Descriptions") ?? "[]";
                    //var unitPricesJson = HttpContext.Session.GetString("UnitPrices") ?? "[]";
                    //var stockBalsJson = HttpContext.Session.GetString("StockBals") ?? "[]";
                    //var stockFlagsJson = HttpContext.Session.GetString("StockFlags") ?? "[]";

                    //var itemCodes = JsonConvert.DeserializeObject<List<string>>(itemCodesJson);
                    //var descriptions = JsonConvert.DeserializeObject<List<string>>(descriptionsJson);
                    //var unitPrices = JsonConvert.DeserializeObject<List<decimal>>(unitPricesJson);
                    //var stockBals = JsonConvert.DeserializeObject<List<decimal>>(stockBalsJson);
                    //var stockFlags = JsonConvert.DeserializeObject<List<bool>>(stockFlagsJson);

                    //// Create SaleItem objects and add them to the list
                    //for (int i = 0; i < itemCodes.Count; i++)
                    //{
                    //    var saleItem = new SaleItem
                    //    {
                    //        ItemCode = itemCodes[i],
                    //        Description = descriptions[i],
                    //        UnitPrice = unitPrices[i],
                    //        StockBal = stockBals[i],
                    //        StockFlag = stockFlags[i]
                    //    };
                    //    saleItems.Add(saleItem);
                    //}

                    //// Create Sale object and save it to the database
                    //var sale = new Sale
                    //{
                    //    CustomerID = CustomerID,
                    //    SaleItems = saleItems
                    //};

                    //// Save the sale to the database (replace this with your actual database logic)
                    //ABCHardware.ProcessSale(sale);

                    //// Clear session data after processing the sale
                    //HttpContext.Session.Remove("ItemCodes");
                    //HttpContext.Session.Remove("Descriptions");
                    //HttpContext.Session.Remove("UnitPrices");
                    //HttpContext.Session.Remove("StockBals");
                    //HttpContext.Session.Remove("StockFlags");

                    //IsFindButtonDisabled = true;
                    //ShowUpdateForm = true;
                    //Message = "Sale processed and saved to the database.";
                    break;


            }
        }
    }
}
