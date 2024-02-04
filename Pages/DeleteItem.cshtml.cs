using ABCHardwareWebApplication.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ABCHardwareWebApplication.Pages
{
    public class DeleteItemModel : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public bool IsFindButtonDisabled;
        public bool ShowUpdateForm;
        public string FormId => "itemForm";
        public Item existingItem { get; set; } = new();



        [BindProperty]
        public string findItemCode { get; set; } = string.Empty;



        [BindProperty]
        public string ItemCodeforDelete {  get; set; }= string.Empty;

        [BindProperty]
        public string itemCode { get; set; } = string.Empty;

        [BindProperty]
        public string description { get; set; } = string.Empty;

        [BindProperty]
        public decimal unitPrice { get; set; }

        [BindProperty]
        public int stockBal { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Stock Flag is required.")]
        public bool stockFlag { get; set; }

        [BindProperty]
        public string Submit { get; set; } = string.Empty;


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
                            Message = "Below are the details of the Item for deletion.";
                            //return RedirectToPage();
                        }
                        else
                        {
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;
                            Message = "Record do not exist.";
                        }
                    }
                    else
                    {
                        IsFindButtonDisabled = false;
                        ShowUpdateForm = false;
                        Message = "Invalid input data.";
                    }
                    //ModelState.Clear();
                    break;

                case "Delete":
                   // ModelState.Clear();
                    if (ModelState.IsValid)
                    {
                        bool Success;
                        
                        Success = ABCHardware.DeleteItem(ItemCodeforDelete);
                        if (Success == true)
                        {
                            Message = "Item was deleted succesfully.";
                            formReset = true;
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;
                        }
                        else
                        {
                            Message = "Item was not Deleted.";
                            formReset = false;
                            IsFindButtonDisabled = true;
                            ShowUpdateForm = true;
                        }
                    }
                    break;

            }
        }
    }
}
