
using UnitTests.Bank;

namespace UnitTests.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        private string _name { get; set; }
        private string _email { get; set; }
        private string _password { get; set; }
        private List<BankAccount> _accounts { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }


        public List<BankAccount> accounts => _accounts;

        public Customer() { }

        public Customer(string name, string email, string password)
        {
            _name = name;
            _email = email;
            _password = password;
            _accounts = new List<BankAccount>();
        }

        public void AddAccount(BankAccount account)
        {
            _accounts.Add(account);
        }
    }
}
