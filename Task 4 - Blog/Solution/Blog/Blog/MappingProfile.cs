using AutoMapper;
using Blog.Contracts.Artical;
using Blog.Contracts.Auth;
using Blog.Contracts.Comment;
using Blog.Contracts.Permissions;
using Blog.Contracts.User;
using Blog.Models;

namespace Blog
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Article, ArticleResponse>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<User, UserGetResponse>();
            CreateMap<Permisions, PermisionResponse>();
            CreateMap<UserRegisterRequest, User>();
            CreateMap<User, UserRegisterResponse>();
        }
    }
}
