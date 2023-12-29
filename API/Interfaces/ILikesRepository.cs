using API.DTOs;
using API.Entities;
using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ILikeRepository
    {
     Task<UserLike> GetUserLike  (int SourceUserId, int likedUserId);
     Task<AppUser> GetUserWithLikes(int userId);
     Task<IEnumerable<LikeDto>> GetUserLikes 
     (string predicate, int userId);

    }
}