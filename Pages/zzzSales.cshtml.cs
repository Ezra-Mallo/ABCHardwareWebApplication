using ABCHardwareWebApplication.Domain;
using ABCHardwareWebApplication.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ABCHardwareWebApplication.Pages
{
    public class SalesModel : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public bool IsFindButtonDisabled;
        public bool ShowUpdateForm;
        public string FormId => "updateCustomerForm";
        [BindProperty]
        public string Submit { get; set; } = string.Empty;
        public Customer existingCustomer { get; set; } = new();
        public Item existingItem { get; set; } = new();

        [BindProperty]
        [Required(ErrorMessage = "Customer ID  is required.")]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "Only number is allowed.")]
        public string findCustomerID { get; set; } = string.Empty;



        [BindProperty]
        public int customerID { get; set; }

        [BindProperty]
        [StringLength(25, ErrorMessage = "First Name must be between 2 to 25 alphabets.")]
        [Required(ErrorMessage = "First Name must be between 2 to 25 alphabets.")]
        [RegularExpression("^[A-Za-z]{2,25}$", ErrorMessage = "First Name must be between 2 to 25 alphabets.")]
        public string firstName { get; set; } = string.Empty;


        [BindProperty]
        [StringLength(25, ErrorMessage = "Last Name must be between 2 to 25 alphabets.")]
        [Required(ErrorMessage = "Last Name must be between 2 to 25 alphabets.")]
        [RegularExpression("^[A-Za-z]{2,25}$", ErrorMessage = "Last Name must be between 2 to 25 alphabets.")]

        public string lastName { get; set; } = string.Empty;

        [BindProperty]
        [StringLength(25, ErrorMessage = "Address must be between 10 to 25 numbers and alphabets.")]
        [Required(ErrorMessage = "Address must be between 10 to 25 numbers and alphabets.")]
        [RegularExpression("^[A-Za-z0-9 ,-]{10,25}$", ErrorMessage = "Address must be between 10 to 25 numbers and alphabets.")]
        public string address { get; set; } = string.Empty;

        [BindProperty]
        [StringLength(25, ErrorMessage = "City must be between 5 to 25 alphabets.")]
        [Required(ErrorMessage = "City must be between 5 to 25 alphabets.")]
        [RegularExpression("^[A-Za-z0-9]{5,25}$", ErrorMessage = "City must be between 5 to 25 alphabets.")]
        public string city { get; set; } = string.Empty;


        [BindProperty]
        [StringLength(25, ErrorMessage = "Province must be between 5 to 25 alphabets.")]
        [Required(ErrorMessage = "Province  must be between 5 to 25 alphabets.")]
        [RegularExpression("^[A-Za-z0-9]{5,25}$", ErrorMessage = "Province must be between 5 to 25 alphabets.")]
        public string province { get; set; } = string.Empty;


        [BindProperty]
        [StringLength(7, ErrorMessage = "PostalCode must follow the pattern X9X 9X9.")]
        [Required(ErrorMessage = "PostalCode must follow the pattern X9X 9X9.")]
        [RegularExpression("^[A-Z][0-9][A-Z] [0-9][A-Z][0-9]$", ErrorMessage = "PostalCode must follow the pattern X9X 9X9.")]
        public string postalCode { get; set; } = string.Empty;
        [BindProperty]
        public string itemCode { get; set; } = string.Empty;


        public void OnGet()
        {
        }

        public void OnPost()
        {
            ABCPOS ABCHardware = new();
            switch (Submit)
            {
                case "Find":
                   // ModelState.Clear();
                    if (ModelState.IsValid)
                    {
                        existingCustomer = ABCHardware.FindCustomer(findCustomerID);
                        if (existingCustomer != null)
                        {
                            IsFindButtonDisabled = true;
                            ShowUpdateForm = true;
                            Message = "Below are the details of the Customer to be modified.";
                            //return RedirectToPage();
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
                        IsFindButtonDisabled = false;
                        ShowUpdateForm = false;
                        Message = "Records do not exist.";
                    }
                   // ModelState.Clear();
                    break;

                case "Add":
                   // ModelState.Clear();
                    if (ModelState.IsValid)
                    {
                        existingItem = ABCHardware.FindItem(itemCode);
                        if (existingItem != null)
                        {
                            IsFindButtonDisabled = true;
                            ShowUpdateForm = true;
                            Message = "Below are the details of the Customer to be modified.";
                            //return RedirectToPage();
                        }
                        else
                        {
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;
                            Message = "Records do not exist.";
                        }
                    }
                    break;


            }
        }

    }
}
