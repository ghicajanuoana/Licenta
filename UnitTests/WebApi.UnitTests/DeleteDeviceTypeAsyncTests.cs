using BusinessLogicLayer.Interfaces;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Controllers;

namespace WebApi.UnitTests
{
    [TestClass]
    public class DeleteDeviceTypeAsyncTests
    {
        private static Mock<ILogger<DeviceTypesController>> mock = new Mock<ILogger<DeviceTypesController>>();
        private static ILogger<DeviceTypesController> logger = mock.Object;

        [TestMethod]
        public void DeleteDeviceTypeAsync_InvalidDeviceTypeId_ReturnBadRequest()
        {
            // Arrange
            var deviceTypeService = new Mock<IDeviceTypeService>();


            var controller = new DeviceTypesController(deviceTypeService.Object,logger);

            // Act
            var actionResult = controller.DeleteDeviceTypeByIdAsync(0).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

            var returnedObjectModel = actionResult as BadRequestObjectResult;

            Assert.AreEqual(400, returnedObjectModel.StatusCode);
            Assert.AreEqual("Invalid device type id !", returnedObjectModel.Value);
        }

        [TestMethod]
        public void DeleteDeviceTypeAsync_ValidDeviceTypeId_CallServiceMethodAndReturnOk()
        {
            // Arrange
            var deviceTypeService = new Mock<IDeviceTypeService>();

            var controller = new DeviceTypesController(deviceTypeService.Object, logger);

            // Act
            var actionResult = controller.DeleteDeviceTypeByIdAsync(2).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));

            var returnedObjectModel = actionResult as OkObjectResult;

            Assert.AreEqual(200, returnedObjectModel.StatusCode);
            Assert.AreEqual("Device type removed successfully", returnedObjectModel.Value);

            deviceTypeService.Verify(d => d.DeleteDeviceTypeByIdAsync(2), Times.Once);
            deviceTypeService.Verify(d => d.DeleteDeviceTypeByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void DeleteDeviceTypeAsync_ValidDeviceTypeId_ServiceMethodThrowsException()
        {
            // Arrange
            var deviceTypeService = new Mock<IDeviceTypeService>();

            deviceTypeService.Setup(d => d.DeleteDeviceTypeByIdAsync(2))
                .ThrowsAsync(new DivideByZeroException("Some error here"));

            var controller = new DeviceTypesController(deviceTypeService.Object,logger);

            try
            {
                // Act
                controller.DeleteDeviceTypeByIdAsync(2).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.AreEqual("Some error here", ex.Message);
            }
        }
    }
}
