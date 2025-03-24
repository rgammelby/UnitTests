using Microsoft.EntityFrameworkCore;

namespace UnitTests.Bank
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int Balance { get; private set; }

        public int AccountNumber { get; private set; }
        public BankAccount()
        {
            Balance = 10000;
            //_accountNumber = GenerateBankAccountNumber();
            AccountNumber = 10000;
        }

        public int Withdraw(int amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Cannot withdraw nothing or a negative amount. ");
            }
            else if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds. ");
            }
            else
            {
                Balance -= amount;
            }
            return Balance;
        }

        public int Deposit(int amount)
        {
            Balance += amount;
            return Balance;
        }

        public int GetBalance()
        {
            return Balance;
        }

        public int GetAccountNumber()
        {
            return AccountNumber;
        }

        private int GenerateBankAccountNumber()
        {
            Random r = new Random();
            int randomNumber = r.Next(10000, 100000);

            return randomNumber;
        }
    }
}
