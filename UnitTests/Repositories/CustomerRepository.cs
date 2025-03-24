using UnitTests.Interfaces;
using UnitTests.Customers;
using UnitTests.Factories;

namespace UnitTests.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Dictionary<string, Customer> _customers = new();

        public Customer Create(Customer customer)
        {
            using BankContext context = TestDbContextFactory.Create();
            context.Customers.Add(customer);
            context.SaveChanges();

            _customers[customer.Id.ToString()] = customer;
            return customer;
        }

        public Customer? GetById(string userId)
        {
            _customers.TryGetValue(userId, out var customer);
            return customer;
        }
    }
}
