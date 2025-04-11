using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectMap.WebApi.Controllers;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace TestProject_WebApi
{
    [TestClass]
    public class EnvironmentControllerTests
    {
        [TestMethod]
        public void Post_ShouldAddEnvironmentWithCorrectOwner()
        {
            // Arrange
            var mockRepo = new Mock<IEnvironmentRepository>();
            var mockObjectRepo = new Mock<IObjectRepository>();
            var mockAuthService = new Mock<IAuthenticationService>();
            var mockLogger = new Mock<ILogger<EnvironmentController>>();

            var controller = new EnvironmentController(
                mockRepo.Object,
                mockObjectRepo.Object,
                mockAuthService.Object,
                mockLogger.Object
            );

            var fakeUserId = Guid.NewGuid().ToString();
            mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);

            var environment = new Environment2D
            {
                Name = "TestEnv",
                MaxHeight = 50,
                MaxLength = 100
            };

            // Act
            var result = controller.Post(environment);

            // Assert
            mockRepo.Verify(r => r.Add(It.Is<Environment2D>(e =>
                e.Name == "TestEnv" &&
                e.MaxHeight == 50 &&
                e.MaxLength == 100 &&
                e.OwnerUserId == Guid.Parse(fakeUserId) &&
                e.id != Guid.Empty
            )), Times.Once);

            Assert.AreEqual("TestEnv", result.Name);
            Assert.AreEqual(50, result.MaxHeight);
            Assert.AreEqual(100, result.MaxLength);
            Assert.AreEqual(Guid.Parse(fakeUserId), result.OwnerUserId);
            Assert.AreNotEqual(Guid.Empty, result.id);
        }
    }
}
