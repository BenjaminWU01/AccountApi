using AccountApi.Controllers;
using library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace AccountApi.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        private DbContextOptions<AccountContext> _dbContextOptions;

        [TestInitialize]
        public void Initialize()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            _dbContextOptions = new DbContextOptionsBuilder<AccountContext>()
                .UseSqlServer("Server =.\\SQLExpress; Database = AccountDbTest; Trusted_Connection = true; TrustServerCertificate = true;")
                .Options;

            using (var context = new AccountContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [TestMethod]
        public async Task GetAccount_Returns_Account_IfExists()
        {
            // Arrange
            var account = new Account { OwnerName = "Benny", Balance = 300.00f };

            using (var context = new AccountContext(_dbContextOptions))
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }

            var controller = new AccountsController(new AccountContext(_dbContextOptions));

            // Act
            var result = await controller.GetAccount(1);

            // Assert
            Assert.IsNotNull(result);

            if (result.Result is OkObjectResult okObjectResult)
            {
                var accountResult = (Account)okObjectResult.Value;

                Assert.AreEqual(1, accountResult.Id);
                Assert.AreEqual(account.OwnerName, accountResult.OwnerName);
                Assert.AreEqual(account.Balance, accountResult.Balance);
            }
            else if (result.Result is NotFoundResult)
            {
                Assert.Fail("Expected an Account but received NotFound.");
            }
            else
            {
                Assert.Fail("Unexpected ActionResult type.");
            }
        }

        [TestMethod]
        public async Task GetAccount_Returns_NotFound_IfAccountDoesNotExist()
        {
            // Arrange
            using (var context = new AccountContext(_dbContextOptions))
            {
                var controller = new AccountsController(context);

                // Act
                var result = await controller.GetAccount(999);

                // Assert
                Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
            }
        }
    }
}