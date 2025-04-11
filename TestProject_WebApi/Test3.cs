using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectMap.WebApi.Controllers;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject_WebApi
{
    [TestClass]
    public class EnvironmentControllerNotFoundTests
    {
        [TestMethod]
        public async Task Delete_ShouldReturnNotFound_WhenEnvironmentDoesNotExist()
        {
            // Arrange
            var mockEnvRepo = new Mock<IEnvironmentRepository>();
            var mockObjRepo = new Mock<IObjectRepository>();
            var mockAuth = new Mock<IAuthenticationService>();
            var mockLogger = new Mock<ILogger<EnvironmentController>>();

            var controller = new EnvironmentController(
                mockEnvRepo.Object,
                mockObjRepo.Object,
                mockAuth.Object,
                mockLogger.Object
            );

            string environmentName = "NietBestaand";

            // GetByName geeft lege lijst terug (dus: environment bestaat niet)
            mockEnvRepo.Setup(r => r.GetByName(environmentName))
                       .ReturnsAsync(new List<Environment2D?>());

            // Act
            var result = await controller.Delete(environmentName);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var value = notFoundResult.Value as Dictionary<string, string>;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.ContainsKey("message"));
            Assert.AreEqual("Environment not found", value["message"]);
        }
    }
}
