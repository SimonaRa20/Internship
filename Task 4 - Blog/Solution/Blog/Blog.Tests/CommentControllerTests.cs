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
using Blog.Contracts.Permissions;
using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Blog.Contracts.Comment;

namespace Blog.Tests
{
    public class CommentControllerTests
    {
        private readonly CommentController _commentController;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IPermissionsRepository> _permissionsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Fixture _fixture;

        public CommentControllerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            _mapperMock = new Mock<IMapper>();
            _commentController = new CommentController(_commentRepositoryMock.Object, _mapperMock.Object, _permissionsRepositoryMock.Object);
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task GetArticleComments_WithValidArticleId_ReturnsOkResult()
        {
            // Arrange
            var articleId = 1;
            var comments = _fixture.CreateMany<CommentResponse>().ToList();
            _commentRepositoryMock.Setup(repo => repo.GetCommentsArticle(articleId)).ReturnsAsync(comments);
            _mapperMock.Setup(mapper => mapper.Map<List<CommentResponse>>(It.IsAny<List<Comment>>()))
                .Returns(_fixture.CreateMany<CommentResponse>().ToList());

            // Act
            var result = await _commentController.GetArticleComments(articleId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetArticleComments_WithInvalidArticleId_ReturnsOkResult()
        {
            // Arrange
            var articleId = -1;
            _commentRepositoryMock.Setup(repo => repo.GetCommentsArticle(articleId)).ReturnsAsync((List<CommentResponse>)null);

            // Act
            var result = await _commentController.GetArticleComments(articleId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetArticleComments_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var articleId = 1;
            _commentRepositoryMock.Setup(repo => repo.GetCommentsArticle(articleId)).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _commentController.GetArticleComments(articleId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task AddComment_WithValidDataAndWritePermission_ReturnsOkResult()
        {
            // Arrange
            var commentRequest = _fixture.Create<CommentRequest>();
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteComments = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _commentRepositoryMock.Setup(repo => repo.AddComment(It.IsAny<CommentRequest>())).Returns(Task.CompletedTask);
            _commentRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _commentController.AddComment(commentRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task AddComment_WithInvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var commentRequest = new CommentRequest();

            // Act
            var result = await _commentController.AddComment(commentRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task AddComment_WithNoWritePermission_ReturnsForbidResult()
        {
            // Arrange
            var commentRequest = _fixture.Create<CommentRequest>();
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteComments = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _commentController.AddComment(commentRequest) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeleteComment_WithValidIdAndWritePermission_ReturnsOkResult()
        {
            // Arrange
            var commentId = 1;
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteComments = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _commentRepositoryMock.Setup(repo => repo.DeleteComment(commentId)).Returns(Task.CompletedTask);
            _commentRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _commentController.DeleteComment(commentId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Contains($"Comment with ID {commentId} updated successfully", result.Value.ToString());
        }

        [Fact]
        public async Task DeleteComment_WithNoWritePermission_ReturnsForbidResult()
        {
            // Arrange
            var commentId = 1;
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteComments = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _commentController.DeleteComment(commentId) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateComment_WithValidDataAndWritePermission_ReturnsOkResult()
        {
            // Arrange
            var commentId = 1;
            var text = "Updated text";
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteComments = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _commentRepositoryMock.Setup(repo => repo.GetCommentById(commentId)).ReturnsAsync(new Comment());
            _commentRepositoryMock.Setup(repo => repo.Update(It.IsAny<Comment>())).Returns(Task.CompletedTask);
            _commentRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _commentController.UpdateArtical(commentId, text) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Contains($"Comment with ID {commentId} updated successfully", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateComment_WithNoWritePermission_ReturnsForbidResult()
        {
            // Arrange
            var commentId = 1;
            var text = "Updated text";
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteComments = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _commentController.UpdateArtical(commentId, text) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
