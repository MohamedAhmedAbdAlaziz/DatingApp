using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int SourceUserId, int likedUserId)
        {
         return await _context.Likes.FindAsync(SourceUserId,likedUserId); 
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users= _context.Users.OrderBy(x=>x.UserName).AsQueryable(); 
           var likes= _context.Likes.AsQueryable(); 

           if(likesParams.Predicate =="liked"){
            likes= likes.Where(x=> x.SourceUserId == likesParams.UserId);
            users= likes.Select(x=>x.LikedUser);
           }
           
           if(likesParams.Predicate == "likedBy"){
            likes= likes.Where(x=> x.LikedUserId == likesParams.UserId);
            users= likes.Select(x=>x.SourceUser);
           }
            var likesUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url

            });

            return await PagedList<LikeDto>.CreateAsync(likesUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(x=>x.LikedUsers)
            .FirstOrDefaultAsync(x=>x.Id ==userId);
        }
        public async Task AddUserLike(UserLike userLike)
        {
            await _context.Likes.AddAsync(userLike);
        }
        public async Task DoSomeThing()
        {
            var r=_context.ChangeTracker.Entries();
            var t = 0;
        }

       
    }
}