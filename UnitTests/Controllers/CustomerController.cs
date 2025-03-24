using UnitTests.Customers;
using UnitTests.Services;
using UnitTests.Factories;
using System.Text.Json;
using UnitTests.Interfaces;

namespace UnitTests.Controllers
{
    public class CustomerController
    {
        private CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        public Customer Create(string json)
        {
            return _service.Create(json);
        }
    }
}
