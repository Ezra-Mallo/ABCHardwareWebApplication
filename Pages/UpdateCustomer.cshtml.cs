using ABCHardwareWebApplication.Domain;
using ABCHardwareWebApplication.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace ABCHardwareWebApplication.Pages
{
    public class UpdateCustomerModel : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public bool IsFindButtonDisabled;
        public bool ShowUpdateForm;
        public string FormId => "updateCustomerForm";

        [BindProperty]
        public string Submit { get; set; } = string.Empty;
        public Customer existingCustomer { get; set; } = new();


        [BindProperty]
        public string findCustomerID { get; set; } = string.Empty;


        [BindProperty]
        public int customerID { get; set; }

        [BindProperty]
        public string firstName { get; set; } = string.Empty;


        [BindProperty]        
        public string lastName { get; set; } = string.Empty;

        [BindProperty]
        public string address { get; set; } = string.Empty;

        [BindProperty]
        public string city { get; set; } = string.Empty;


        [BindProperty]
        public string province { get; set; } = string.Empty;


        [BindProperty]
        public string postalCode { get; set; } = string.Empty;


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
                    // ModelState.Clear();

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

                case "Modify":
                    if (string.IsNullOrEmpty(firstName) || firstName.Length < 2 || firstName.Length > 25 || !Regex.IsMatch(firstName, "^[A-Za-z ]{2,25}$"))
                    {
                        ModelState.AddModelError("firstName", "First Name must be between 2 to 25 alphabets.");
                    }

                    if (string.IsNullOrEmpty(lastName) || lastName.Length < 2 || lastName.Length > 25 || !Regex.IsMatch(lastName, "^[A-Za-z ]{2,25}$"))
                    {
                        ModelState.AddModelError("lastName", "Last Name must be between 2 to 25 alphabets.");
                    }

                    if (string.IsNullOrEmpty(address) || address.Length < 10 || address.Length > 25 || !Regex.IsMatch(address, "^[A-Za-z0-9 ,]{10,25}$"))
                    {
                        ModelState.AddModelError("address", "Address must be between 10 to 25 numbers and alphabets.");
                    }

                    if (string.IsNullOrEmpty(city) || city.Length < 5 || city.Length > 25 || !Regex.IsMatch(city, "^[A-Za-z ]{5,25}$"))
                    {
                        ModelState.AddModelError("city", "City must be between 5 to 25 alphabets.");
                    }

                    if (string.IsNullOrEmpty(province) || province.Length < 5 || province.Length > 25 || !Regex.IsMatch(province, "^[A-Za-z ]{5,25}$"))
                    {
                        ModelState.AddModelError("province", "Province must be between 5 to 25 alphabets.");
                    }

                    if (string.IsNullOrEmpty(postalCode) || !Regex.IsMatch(postalCode, "^[A-Za-z][0-9][A-Za-z] [0-9][A-Za-z][0-9]$"))
                    {
                        ModelState.AddModelError("postalCode", "PostalCode must follow the pattern X9X 9X9.");
                    }

                    // ModelState.Clear();
                    if (ModelState.IsValid)
                    {
                        bool Success;

                        Customer aCustomer= new()
                        {
                            CustomerID = customerID,
                            FirstName = firstName, 
                            LastName = lastName,  
                            Address = address, 
                            City = city, 
                            Province = province, 
                            PostalCode = postalCode
                        };
                        Success = ABCHardware.UpdateCustomer(aCustomer);
                        if (Success == true)
                        {
                            Message = "Customer details was Updated succesfully.";
                            formReset = true;
                            IsFindButtonDisabled = false;
                            ShowUpdateForm = false;

                        }
                        else
                        {
                            Message = "Customer details was not Updated.";
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
