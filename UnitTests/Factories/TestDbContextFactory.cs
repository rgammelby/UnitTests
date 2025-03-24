using Microsoft.EntityFrameworkCore;

namespace UnitTests.Factories
{
    public class TestDbContextFactory
    {
        public static BankContext Create()
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: "TestBankDb")
                .Options;

            return new BankContext(options);
        }
    }
}
