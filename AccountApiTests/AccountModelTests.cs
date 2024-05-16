using library;

namespace AccountApi.Tests
{
    [TestClass]
    public class AccountModelTests
    {
        [TestMethod]
        public void AccountModel_SetPropertiesAndGetValues()
        {
            // Arrange
            var account = new Account();

            // Act
            account.Id = 1;
            account.OwnerName = "Benny";
            account.Balance = 300.00f;

            // Assert
            Assert.AreEqual(1, account.Id);
            Assert.AreEqual("Benny", account.OwnerName);
            Assert.AreEqual(300.00f, account.Balance);
        }
    }
}