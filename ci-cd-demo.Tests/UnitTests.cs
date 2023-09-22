using ci_cd_demo.Controllers;

namespace ci_cd_demo.Tests
{
    [TestClass]
    public class MyTests
    {
        [TestMethod]
        public void AddMethod_ReturnsSum()
        {
            // Arrange
            var controller = new CalculatorController();

            // Act
            var result = controller.Add(2, 3);

            // Assert
            Assert.AreEqual(6, result);
        }
    }
}
