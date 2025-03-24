using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Bank;

namespace UnitTests.Interfaces
{
    public interface IBankFactory
    {
        public BankAccount Create();
    }
}
