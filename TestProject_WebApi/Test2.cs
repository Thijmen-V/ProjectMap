using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectMap.WebApi.Controllers;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TestProject_WebApi
{
    [TestClass]
    public class ObjectControllerTests
    {
        [TestMethod]
        public async Task Put_ShouldUpdateObject_WhenObjectExists()
        {
            // Arrange
            var mockRepo = new Mock<IObjectRepository>();
            var mockLogger = new Mock<ILogger<ObjectController>>();
            var controller = new ObjectController(mockRepo.Object, mockLogger.Object);

            var objectId = Guid.NewGuid();
            var existingObject = new Object2D
            {
                Id = objectId,
                PrefabId = "geel",
                EnvironmentId = Guid.NewGuid(),
                PositionX = 1,
                PositionY = 1
            };

            var updatedObject = new Object2D
            {
                Id = objectId,
                PrefabId = "blauw",
                EnvironmentId = existingObject.EnvironmentId,
                PositionX = 5,
                PositionY = 5
            };

            // Repository geeft een bestaand object terug
            mockRepo.Setup(r => r.GetById(objectId)).ReturnsAsync(existingObject);

            // Act
            var result = await controller.Put(objectId, updatedObject);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(objectId, updatedObject), Times.Once);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(updatedObject, okResult.Value);
        }
    }
}
