using ABCHardwareWebApplication.Domain;
using ABCHardwareWebApplication.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System;
using System.Text.RegularExpressions;



namespace ABCHardwareWebApplication.Pages
{
    public class AddItemModel : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public string FormId = "itemForm";


        [BindProperty]
        public string itemCode { get; set; } = string.Empty;

        [BindProperty]
        public string description { get; set; } = string.Empty;

        [BindProperty]
        public decimal unitPrice { get; set; }

        [BindProperty]
       
        public int stockBal { get; set; }

        [BindProperty]
        public bool stockFlag { get; set; }
        
        public void OnGet()            
        {
        }
        public IActionResult OnPost()
        {
            itemCode = itemCode.ToUpper();
            if (string.IsNullOrEmpty(itemCode) || itemCode.Length != 6)
            {
                ModelState.AddModelError("itemCode", "Item Code must be 6 characters.");
            }

            if (string.IsNullOrEmpty(itemCode) || !Regex.IsMatch(itemCode, "^[A-Za-z]{1}[0-9]{5}$"))
            {
                ModelState.AddModelError("itemCode", "Item Code must follow the pattern: X99999.");
            }
            if (string.IsNullOrEmpty(description) || description.Length > 100)
            {
                ModelState.AddModelError("description", "Description cannot exceed 100 characters.");
            }

            if (string.IsNullOrEmpty(description) || !Regex.IsMatch(description, "^[A-Za-z0-9-, ]{2,100}$"))
            {
                ModelState.AddModelError("description", "Item Description must not contain special characters and must be between 2 and 100 characters long.");
            }
            if (unitPrice <= 0)
            {
                ModelState.AddModelError("unitPrice", "Unit Price must be a positive value.");
            }

            if (stockBal < 0)
            {
                ModelState.AddModelError("stockBal", "Stock Balance must be a non-negative value.");
            }
            if (!Regex.IsMatch(stockBal.ToString(), "^[0-9]{1,7}$"))
            {
                ModelState.AddModelError("stockBal", "Stock Balance must be numbers only.");
            }
            if (ModelState.IsValid)
            {
                bool Success;
                ABCPOS ABCHardware = new();
                Item anItem = new()
                {
                    ItemCode = itemCode,
                    Description = description,
                    UnitPrice = unitPrice,
                    StockBal = stockBal,
                    StockFlag = stockFlag
                };
                Success = ABCHardware.CreateItem(anItem);
                if (Success == true)
                {
                    TempData["Message"] = "New Item was added succesfully.";
                    formReset = true;
                    return RedirectToPage();
                }
                else
                {
                    TempData["Message"] = "New Item was not added.";
                    formReset = false;
                }                
            }
            else
            {
                Message = "invalid input data.";
                formReset =false;
            }
            return Page();
        }
    }
}
