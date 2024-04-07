using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Authentication;
using WebApi.Controllers;

namespace WebApi.UnitTests
{
    /*
    [TestClass]
    public class GetLocationByIdAsyncTests
    {
        private static Mock<ILogger<LocationController>> mock = new Mock<ILogger<LocationController>>();
        ILogger<LocationController> logger = mock.Object;

        [TestMethod]
        public void GetLocationByIdAsync_InvalidLocationId_ReturnNull()
        {
            // Arrange
            var locationService = new Mock<ILocationService>();
            locationService.Setup(l => l.GetLocationByIdAsync(3)).Returns(Task.FromResult<LocationAddDto>(null));

            var controller = new LocationController(locationService.Object, logger);

            //Action
            var actionResult = controller.GetLocationByIdAsync(3).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));

            var returnedObjctModel = actionResult as ObjectResult;

            Assert.AreEqual(702, returnedObjctModel.StatusCode);
            Assert.AreEqual("Location not found !", returnedObjctModel.Value);
        }

        [TestMethod]
        public void GetLocationByIdAsync_InvalidLocationId_ReturnBadRequest()
        {
            // Arrange
            var locationService = new Mock<ILocationService>();
            locationService.Setup(l => l.GetLocationByIdAsync(3))
            .ThrowsAsync(new NullReferenceException("Could not find any location for id: '3'!"));

            var controller = new LocationController(locationService.Object, logger);
            // Act
            var actionResult = controller.GetLocationByIdAsync(3).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

            var returnedObjectModel = actionResult as BadRequestObjectResult;

            Assert.AreEqual(400, returnedObjectModel.StatusCode);
            Assert.ReferenceEquals(new NullReferenceException("Could not find any location for id: '3'!"), returnedObjectModel.Value);
        }

        [TestMethod]
        public void GetLocationById_ReturnOk()
        {
            //Arange
            var locationService = new Mock<ILocationService>();
            locationService.Setup(l => l.GetLocationByIdAsync(1)).ReturnsAsync(new LocationAddDto
            {
                LocationId= 1,
                Name = "Test",
                EmailAlertsActive = true,
                EmailRecipient = "EmailRecipient",
                ContactEmail = "ContactEmail",
                Address = "Address",
                City = "City",
                Country = "Country",
                Phone = "0746184165"
            });

            var controller = new LocationController(locationService.Object, logger);

            //Action
            var actionResult = controller.GetLocationByIdAsync(1).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));

            var returnedOjectModel = actionResult as OkObjectResult;

            Assert.AreEqual(200, returnedOjectModel.StatusCode);

            var returnedObjectValue = returnedOjectModel.Value as LocationDto;

            Assert.IsNotNull(returnedObjectValue);

            locationService.Verify(d => d.GetLocationByIdAsync(1), Times.Once);
        }
    }
    */
}
