using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataStoreService;
using DataStoreService.Controllers;

namespace DataStoreService.Tests.Controllers
{
    [TestClass]
    public class ProfileControllerTest
    {
        [TestMethod]
        public void CreateProfile()
        {
            // Arrange
            var controller = new ProfileController();

            // Act
            // TODO: this will fail when attempting to build the link,
            // need to mock the HttpRequestMessage
            //var result = controller.CreateProfile( "{\"\":\"\",\"\":\"\"}");
            
            // Assert
            //Assert.IsNotNull(result);
        }
    }
}
