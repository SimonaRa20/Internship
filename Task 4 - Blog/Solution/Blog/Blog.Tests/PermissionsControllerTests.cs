using AutoFixture.AutoMoq;
using AutoFixture;
using AutoMapper;
using Blog.Controllers;
using Blog.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Constants;
using Blog.Contracts.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Blog.Tests
{
    public class PermissionsControllerTests
    {
        private readonly PermissionsController _permissionsController;
        private readonly Mock<IPermissionsRepository> _permissionsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Fixture _fixture;

        public PermissionsControllerTests()
        {
            _permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            _mapperMock = new Mock<IMapper>();
            _permissionsController = new PermissionsController(_permissionsRepositoryMock.Object, _mapperMock.Object);
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task GetUsersPermissions_WithValidRole_ReturnsOkResult()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetUsersPermissions()).ReturnsAsync(new List<PermisionResponse>());
            _mapperMock.Setup(mapper => mapper.Map<List<PermisionResponse>>(It.IsAny<List<PermisionResponse>>()))
                .Returns(_fixture.CreateMany<PermisionResponse>().ToList());

            // Act
            var result = await _permissionsController.GetUsersPermissions() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsType<List<PermisionResponse>>(result.Value);
        }

        [Fact]
        public async Task GetUsersPermissions_WithNoPermissions_ReturnsNotFoundResult()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetUsersPermissions()).ReturnsAsync((List<PermisionResponse>)null);

            // Act
            var result = await _permissionsController.GetUsersPermissions() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Users or their permissions not found", result.Value.ToString());
        }

        [Fact]
        public async Task GetUsersPermissions_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetUsersPermissions()).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _permissionsController.GetUsersPermissions() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while fetching users' permissions", result.Value.ToString());
        }
        [Fact]
        public async Task GetPermissionsByUserId_WithValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(new PermisionResponse()); // Mock the GetPermissionsByUserId method to return permissions
            _mapperMock.Setup(mapper => mapper.Map<PermisionResponse>(It.IsAny<PermisionResponse>()))
                .Returns(_fixture.Create<PermisionResponse>());

            // Act
            var result = await _permissionsController.GetPermissionsByUserId(userId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsType<PermisionResponse>(result.Value);
        }

        [Fact]
        public async Task GetPermissionsByUserId_WithInvalidUserId_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = -1;

            // Act
            var result = await _permissionsController.GetPermissionsByUserId(userId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Permissions not found", result.Value.ToString());
        }

        [Fact]
        public async Task GetPermissionsByUserId_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = 1;
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _permissionsController.GetPermissionsByUserId(userId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while fetching user's permissions", result.Value.ToString());
        }
        [Fact]
        public async Task UpdatePermissions_WithValidDataAndAdminRole_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var updatedPermissions = _fixture.Create<PermisionRequest>();
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, Role.Admin)
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _permissionsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _permissionsRepositoryMock.Setup(repo => repo.UpdatePermissions(userId, updatedPermissions)).Returns(Task.CompletedTask);
            _permissionsRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _permissionsController.UpdatePermissions(userId, updatedPermissions) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Contains("Permissions updated successfully", result.Value.ToString());
        }
    }
}
