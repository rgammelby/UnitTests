using UnitTests.Customers;
using UnitTests.Interfaces;
using UnitTests.Repositories;
using System.Text.RegularExpressions;
using UnitTests.Factories;
using System.Text.Json;
using System.Security.Claims;

namespace UnitTests.Services
{
    public class CustomerService : ICustomerService
    {
        private CustomerRepository _repository;

        public CustomerService(CustomerRepository repo)
        {
            _repository = repo;
        }

        // Validates customer data and passes on to repository
        public Customer Create(string json)
        {
            // Initialise Customer object as deserialised JSON from Controller
            Customer customer = JsonSerializer.Deserialize<Customer>(json);

            // Validate Customer name and e-mail address
            bool customerNameValidated = ValidateCustomerName(customer.Name);
            bool customerEmailValidated = ValidateCustomerEmail(customer.Email);

            // If both name and e-mail address are successfully validated,
            if (customerNameValidated && customerEmailValidated)
            {
                // Pass the deserialised Customer object to the repository for DB Customer Creation
                return _repository.Create(customer);
            } else
            {
                // If not validated, throw an InvalidOperationException
                throw new InvalidOperationException("Customer name or e-mail address is invalid. ");
            }
        }

        // Validates customer name based on Regex pattern
        public bool ValidateCustomerName(string name)
        {
            string pattern = @"^(?!.*([a-zA-Zæøåäöë'-])\1\1)[a-zA-Zæøåäöë'-]+(?: [a-zA-Zæøåäöë'-]+)*$";

            return Regex.IsMatch(name, pattern);
        }

        // Validates customer e-mail address based on Regex pattern
        public bool ValidateCustomerEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Retrieves customer profile
        public Customer GetCustomerProfile(string userId)
        {
            // Throw exception if userId is invalid
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            // If not, pass to GetById function in repository
            var customer = _repository.GetById(userId);

            // Throw exception if the returned Customer is null
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer profile not found.");
            }

            // Otherwise return customer
            return customer;
        }

    }
}