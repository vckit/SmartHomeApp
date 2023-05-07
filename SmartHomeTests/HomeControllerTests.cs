using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartHomeApp.Controllers;
using SmartHomeApp.Models;
using Xunit;
using Microsoft.EntityFrameworkCore.Query.Internal;


namespace SmartHomeTests
{
    public class HomeControllerTests
    {

        private readonly List<Device> _devices;
        private readonly List<DeviceStatus> _deviceStatuses;
        private readonly Mock<SmartHomeContext> _mockContext;

        public HomeControllerTests()
        {
            _devices = new List<Device>
            {
                new Device { DeviceId = 1, DeviceName = "Device1", Location = "Living room", StatusId = 1 },
                new Device { DeviceId = 2, DeviceName = "Device2", Location = "Kitchen", StatusId = 2 }
            };

            _deviceStatuses = new List<DeviceStatus>
            {
                new DeviceStatus { StatusId = 1, StatusName = "Status1" },
                new DeviceStatus { StatusId = 2, StatusName = "Status2" }
            };

            _mockContext = new Mock<SmartHomeContext>();
            _mockContext.Setup(c => c.Devices).Returns(BuildMockDbSet(_devices).Object);
            _mockContext.Setup(c => c.DeviceStatuses).Returns(BuildMockDbSet(_deviceStatuses).Object);
        }
        private static Mock<DbSet<T>> BuildMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new AsyncQueryProvider<T>(queryableData.Provider));
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            mockDbSet.As<IAsyncEnumerable<T>>().Setup(d => d.GetAsyncEnumerator(default)).Returns(new AsyncEnumerator<T>(queryableData.GetEnumerator()));

            return mockDbSet;
        }

        [Fact]
        public async Task Index_ReturnsViewAndCorrectModel()
        {
            // Arrange
            var controller = new HomeController(_mockContext.Object);

            // Act
            var result = await controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<(IEnumerable<Device>, DeviceFilterViewModel)>(viewResult.Model);
            Assert.Equal(_devices.Count, model.Item1.Count());

        }

        [Fact]
        public async Task Index_FiltersBySearchString()
        {
            // Arrange
            var controller = new HomeController(_mockContext.Object);
            var filterViewModel = new DeviceFilterViewModel { SearchString = "Device1" };

            // Act
            var result = await controller.Index(filterViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<(IEnumerable<Device>, DeviceFilterViewModel)>(viewResult.Model);
            Assert.Single(model.Item1);
            Assert.Contains(_devices[0], model.Item1);
        }

        [Fact]
        public async Task Index_FiltersByStatusId()
        {
            // Arrange
            var controller = new HomeController(_mockContext.Object);
            var filterViewModel = new DeviceFilterViewModel { StatusId = 1 };

            // Act
            var result = await controller.Index(filterViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<(IEnumerable<Device>, DeviceFilterViewModel)>(viewResult.Model);
            Assert.Single(model.Item1);
            Assert.Contains(_devices[0], model.Item1);
        }

        [Fact]
        public async Task Index_FiltersByInstallationDate()
        {
            // Arrange
            var controller = new HomeController(_mockContext.Object);
            _devices[0].InstallationDate = DateTime.Now.Date;
            var filterViewModel = new DeviceFilterViewModel { InstallationDate = DateTime.Now.Date };

            // Act
            var result = await controller.Index(filterViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<(IEnumerable<Device>, DeviceFilterViewModel)>(viewResult.Model);
            Assert.Single(model.Item1);
            Assert.Contains(_devices[0], model.Item1);
        }

        [Fact]
        public async Task Index_ReturnsAllDevices_WhenFilterIsNull()
        {
            // Arrange
            var controller = new HomeController(_mockContext.Object);

            // Act
            var result = await controller.Index(new DeviceFilterViewModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<(IEnumerable<Device>, DeviceFilterViewModel)>(viewResult.Model);
            Assert.Equal(_devices.Count, model.Item1.Count());
        }

        [Fact]
        public async Task Index_ReturnsCorrectFilterViewModel_WhenFilterIsNull()
        {
            // Arrange
            var controller = new HomeController(_mockContext.Object);

            // Act
            var result = await controller.Index(new DeviceFilterViewModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<(IEnumerable<Device>, DeviceFilterViewModel)>(viewResult.Model);
            Assert.Null(model.Item2.SearchString);
            Assert.Null(model.Item2.InstallationDate);
            Assert.Null(model.Item2.StatusId);
        }
    }
}
