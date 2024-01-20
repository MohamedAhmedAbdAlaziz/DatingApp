using API.DTOs;
using API.Entities;
using API.Helpers;
using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
     Task<UserLike> GetUserLike  (int SourceUserId, int likedUserId);
     Task<AppUser> GetUserWithLikes(int userId);
     Task<PagedList<LikeDto>> GetUserLikes 
     (LikesParams likesParams);

          Task DoSomeThing();
        Task AddUserLike(UserLike userLike);

    }

}