using ABCHardwareWebApplication.Domain;
using ABCHardwareWebApplication.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace ABCHardwareWebApplication.Pages
{
    public class DeleteCustomerModel : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public bool IsFindButtonDisabled;
        public bool ShowUpdateForm;
        public string FormId => "DeleteCustomerForm";
        [BindProperty]
        public string Submit { get; set; } = string.Empty;

        public Customer existingCustomer { get; set; } = new();

        [BindProperty]
        public string customerID { get; set; } = string.Empty;

        [BindProperty]
        public string findCustomerID { get; set; } = string.Empty;


        public void OnGet()
        {
        }
        public void OnPost()
        {
            ABCPOS ABCHardware = new();
            ModelState.Clear();
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
                            Message = "Invalid data input. Only numbers are allowed";
                    }
                   // ModelState.Clear();
                    break;

                case "Delete":
                   // ModelState.Clear();

                    if (ModelState.IsValid)
                    {

                        bool Success;

                        Success = ABCHardware.DeleteCustomer(customerID);
                        if (Success == true)
                        {
                            Message = "Customer details was deleted succesfully.";
                            formReset = true;
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;
                        }
                        else
                        {
                            Message = "Customer details was not Deleted.";
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
