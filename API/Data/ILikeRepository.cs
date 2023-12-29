using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Data
{
    public class LikeRepository : ILikeRepository
    {
        public Task<UserLike> GetUserLike(int SourceUserId, int likedUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> GetUserWithLikes(int userId)
        {
            throw new NotImplementedException();
        }
    }
}