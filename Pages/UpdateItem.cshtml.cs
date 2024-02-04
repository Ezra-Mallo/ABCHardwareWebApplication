using ABCHardwareWebApplication.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace ABCHardwareWebApplication.Pages
{
    public class UpdateItemModel : PageModel
    {
        
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public bool IsFindButtonDisabled;
        public bool ShowUpdateForm;
        public string FormId => "updateItemForm";

        [BindProperty]
        public string Submit { get; set; } = string.Empty;


        public Item existingItem { get; set; } = new();
        


        [BindProperty]
        public string findItemCode { get; set; } = string.Empty;


        [BindProperty]
        public string itemCode {get; set; } = string.Empty;

        [BindProperty]
        public string description {get; set; } = string.Empty;

        [BindProperty]
        public decimal unitPrice {get; set; }

        [BindProperty]
        public int stockBal {get; set; }

        [BindProperty]
        public bool stockFlag {get; set; }

        public void OnGet()
        {
        }
        public void  OnPost()
        {

            ModelState.Clear();
            ABCPOS ABCHardware = new();
            switch (Submit)
            {
                case "Find":
                    //ModelState.Clear();

                    if (string.IsNullOrEmpty(findItemCode) || !Regex.IsMatch(findItemCode, "^[A-Za-z]{1}[0-9]{5}$") || findItemCode.Length != 6)
                    {
                        ModelState.AddModelError("findItemCode", "Item Code must follow the pattern: X99999.");
                    }
                    
                    
                    if (ModelState.IsValid)
                    {
                        existingItem = ABCHardware.FindItem(findItemCode);
                        if (existingItem != null)
                        {

                            IsFindButtonDisabled = true;
                            ShowUpdateForm = true;
                            Message = "Below are the details of the Item to be updated.";
                            //return RedirectToPage();
                        }
                        else {
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;
                            Message = "Records do not exist.";
                        }
                    }
                    else
                    {
                        IsFindButtonDisabled = false;
                        ShowUpdateForm = false;
                        Message = "Records do not exist.";
                    }
                    ModelState.Clear();
                    break;

                case "Modify":
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

                        Item anItem = new()
                        {
                            ItemCode = itemCode,
                            Description = description,
                            UnitPrice = unitPrice,
                            StockBal = stockBal,
                            StockFlag = stockFlag
                        };
                        Success = ABCHardware.UpdateItem(anItem);
                        if (Success == true)
                        {
                            Message = "Item was Updated succesfully.";
                            formReset = true;
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;

                        }
                        else
                        {
                            Message = "Item was not Updated.";
                            formReset = false;
                            IsFindButtonDisabled = true;
                            ShowUpdateForm = true;

                        }
                    }
                    break;
            }
            ModelState.Clear();

        }
    }
}
