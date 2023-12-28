using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Blog.Contracts.Artical;
using Blog.Contracts.Permissions;
using Blog.Controllers;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Blog.Tests
{
    public class ArticleControllerTests
    {
        private readonly ArticleController _articleController;
        private readonly Mock<IArticleRepository> _articleRepositoryMock;
        private readonly Mock<IPermissionsRepository> _permissionsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Fixture _fixture;

        public ArticleControllerTests()
        {
            _articleRepositoryMock = new Mock<IArticleRepository>();
            _permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            _mapperMock = new Mock<IMapper>();
            _articleController = new ArticleController(_articleRepositoryMock.Object, _mapperMock.Object, _permissionsRepositoryMock.Object);
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Fact]
        public async Task GetArticals_ReturnsOkResult()
        {
            // Arrange
            var articles = _fixture.CreateMany<Article>().AsQueryable();
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(articles);
            _mapperMock.Setup(mapper => mapper.Map<List<ArticleResponse>>(It.IsAny<IQueryable<Article>>()))
                .Returns(_fixture.CreateMany<ArticleResponse>().ToList());

            // Act
            var result = await _articleController.GetArticals() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsType<List<ArticleResponse>>(result.Value);
        }

        [Fact]
        public async Task GetArticals_WhenNoArticles_ReturnsNotFoundResult()
        {
            // Arrange
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync((IQueryable<Article>)null);

            // Act
            var result = await _articleController.GetArticals() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Articals not found", result.Value);
        }

        [Fact]
        public async Task GetArticals_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _articleController.GetArticals() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while fetching articals", result.Value.ToString());
        }

        [Fact]
        public async Task GetArticlasOrderByRank_ReturnsOkResult()
        {
            // Arrange
            var articles = _fixture.CreateMany<Article>().AsQueryable();
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(articles);
            _mapperMock.Setup(mapper => mapper.Map<List<ArticleResponse>>(It.IsAny<IQueryable<Article>>()))
                .Returns(_fixture.CreateMany<ArticleResponse>().ToList());

            // Act
            var result = await _articleController.GetArticalsOrderByRank() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsType<List<ArticleResponse>>(result.Value);
        }

        [Fact]
        public async Task GetArticlasOrderByRank_WhenNoArticles_ReturnsNotFoundResult()
        {
            // Arrange
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync((IQueryable<Article>)null);

            // Act
            var result = await _articleController.GetArticalsOrderByRank() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Articals not found", result.Value);
        }

        [Fact]
        public async Task GetArticlasOrderByRank_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _articleController.GetArticalsOrderByRank() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while fetching articals", result.Value.ToString());
        }

        [Fact]
        public async Task GetArticalsByComments_ReturnsOkResult()
        {
            // Arrange
            var articles = _fixture.CreateMany<Article>().AsQueryable();
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(articles);
            _mapperMock.Setup(mapper => mapper.Map<List<ArticleResponse>>(It.IsAny<IQueryable<Article>>()))
                .Returns(_fixture.CreateMany<ArticleResponse>().ToList());

            // Act
            var result = await _articleController.GetArticalsByComments() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetArticalsByComments_WhenNoArticles_ReturnsNotFoundResult()
        {
            // Arrange
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync((IQueryable<Article>)null);

            // Act
            var result = await _articleController.GetArticalsByComments() as ObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Articals not found", result.Value);
        }

        [Fact]
        public async Task GetArticalsByComments_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            _articleRepositoryMock.Setup(repo => repo.GetAll()).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _articleController.GetArticalsByComments() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while fetching articals", result.Value.ToString());
        }
        [Fact]
        public async Task CreateArtical_WithValidDataAndWritePermission_ReturnsOkResult()
        {
            // Arrange
            var articleRequest = _fixture.Create<ArticleRequest>();
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteArticals = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _articleRepositoryMock.Setup(repo => repo.Add(It.IsAny<Article>())).ReturnsAsync(new Article());
            _articleRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _articleController.CreateArtical(articleRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Article created successfully", result.Value);
        }

        [Fact]
        public async Task CreateArtical_WithInvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var articleRequest = new ArticleRequest();

            // Act
            var result = await _articleController.CreateArtical(articleRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while creating the article", result.Value.ToString());
        }

        [Fact]
        public async Task CreateArtical_WithNoWritePermission_ReturnsForbidResult()
        {
            // Arrange
            var articleRequest = _fixture.Create<ArticleRequest>();
            var userId = 123;
            var userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteArticals = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _articleController.CreateArtical(articleRequest) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetArtical_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var articalId = 1;
            var article = _fixture.Create<Article>();
            _articleRepositoryMock.Setup(repo => repo.GetById(articalId)).ReturnsAsync(article);
            _mapperMock.Setup(mapper => mapper.Map<ArticleResponse>(It.IsAny<Article>()))
                .Returns(_fixture.Create<ArticleResponse>());

            // Act
            var result = await _articleController.GetArtical(articalId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsType<ArticleResponse>(result.Value);
        }

        [Fact]
        public async Task GetArtical_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var articalId = -1;
            _articleRepositoryMock.Setup(repo => repo.GetById(articalId)).ReturnsAsync((Article)null);

            // Act
            var result = await _articleController.GetArtical(articalId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains($"Article with ID {articalId} not found", result.Value.ToString());
        }

        [Fact]
        public async Task GetArtical_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var articalId = 1;
            _articleRepositoryMock.Setup(repo => repo.GetById(articalId)).ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await _articleController.GetArtical(articalId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while deleting the article", result.Value.ToString());
        }
        [Fact]
        public async Task DeleteArtical_WithValidIdAndWritePermission_ReturnsOkResult()
        {
            // Arrange
            var articalId = 1;
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteArticals = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _articleRepositoryMock.Setup(repo => repo.Delete(articalId)).Returns(Task.CompletedTask);
            _articleRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _articleController.DeleteArtical(articalId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Contains($"Article with ID {articalId} deleted successfully", result.Value.ToString());
        }


        [Fact]
        public async Task DeleteArtical_WithNoWritePermission_ReturnsForbidResult()
        {
            // Arrange
            var articalId = 1;
            var userId = 123;
            var userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteArticals = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _articleController.DeleteArtical(articalId) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateArtical_WithValidDataAndWritePermission_ReturnsOkResult()
        {
            // Arrange
            var articalId = 1;
            var articleRequest = _fixture.Create<ArticleRequest>();
            var userId = 123;
            var userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteArticals = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _articleRepositoryMock.Setup(repo => repo.GetById(articalId)).ReturnsAsync(_fixture.Create<Article>());
            _articleRepositoryMock.Setup(repo => repo.Update(It.IsAny<Article>())).Returns(Task.CompletedTask);
            _articleRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _articleController.UpdateArtical(articalId, articleRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Contains($"Article with ID {articalId} updated successfully", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateArtical_WithInvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var articalId = 1;
            var articleRequest = new ArticleRequest();

            // Act
            var result = await _articleController.UpdateArtical(articalId, articleRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("An error occurred while updating the article", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateArtical_WithNoWritePermission_ReturnsForbidResult()
        {
            // Arrange
            var articalId = 1;
            var articleRequest = _fixture.Create<ArticleRequest>();
            var userId = 123;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { WriteArticals = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _articleController.UpdateArtical(articalId, articleRequest) as ForbidResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task VoteArtical_WithValidDataAndRatePermission_ReturnsOkResult()
        {
            // Arrange
            var articleId = 1;
            var userId = 123;
            var voteValue = 1;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { RateArticals = true };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);
            _articleRepositoryMock.Setup(repo => repo.VoteArtical(articleId, userId, voteValue)).ReturnsAsync(true);

            // Act
            var result = await _articleController.VoteArticle(articleId, userId, voteValue) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task VoteArtical_WithNoRatePermission_ReturnsForbidResult()
        {
            // Arrange
            var articleId = 1;
            var userId = 123;
            var voteValue = 1;
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var claimsPrincipal = new ClaimsPrincipal(userIdentity);

            _articleController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var permissions = new PermisionResponse { RateArticals = false };
            _permissionsRepositoryMock.Setup(repo => repo.GetPermissionsByUserId(userId)).ReturnsAsync(permissions);

            // Act
            var result = await _articleController.VoteArticle(articleId, userId, voteValue) as ForbidResult;

            // Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}