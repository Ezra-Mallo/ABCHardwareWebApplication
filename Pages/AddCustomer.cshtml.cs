using ABCHardwareWebApplication.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace ABCHardwareWebApplication.Pages
{
    public class AddCustomerModel : PageModel
    {
        public string Message { get; set; } = string.Empty;
        public bool formReset;
        public string FormId = "customerForm";

        [BindProperty]
        public string firstName { get; set; }

        [BindProperty]
        public string lastName { get; set; }

        [BindProperty]
        public string address { get; set; }

        [BindProperty]
        public string city { get; set; }

        [BindProperty]
        public string province { get; set; }

        [BindProperty]
        public string postalCode { get; set; }


        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

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




            if (ModelState.IsValid)
            {
                bool Success;
                ABCPOS ABCHardware = new();
                Customer aCustomer= new()
                {
                    FirstName = firstName, 
                    LastName = lastName,  
                    Address = address, 
                    City = city, 
                    Province = province, 
                    PostalCode = postalCode.ToUpper()
                };
                Success = ABCHardware.CreateCustomer(aCustomer);
                if (Success == true)
                {
                    Message = "New Customer was added succesfully.";
                    formReset = true;
                    return RedirectToPage();
                }
                else
                {
                    Message = "New Customer was not added.";
                    formReset = false;
                }
            }
            else
            {
                Message = "Invalid input data.";
                formReset = false;
            }
            return Page();
        }
    }
}
