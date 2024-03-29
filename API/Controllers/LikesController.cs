
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository
         ,  ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
             var sourceUserId= User.GetUserId();
             var likedUser= await _userRepository.GetUserByUsernameAsync(username);
             var sourcesUser= await _likesRepository.GetUserWithLikes(sourceUserId);
             
             if(likedUser ==null) return NotFound();

             if(sourcesUser.UserName == username) return BadRequest("YOu can not like yourself");
             var userLike= await _likesRepository.GetUserLike(sourceUserId,likedUser.Id);
 
             if(userLike != null)
              return BadRequest("you already like this user");

       userLike = new UserLike{
        SourceUserId= sourceUserId,
     LikedUserId= likedUser.Id
       };
            if (sourcesUser.LikedByUsers == null)
            {
                sourcesUser.LikedByUsers= new List<UserLike>();
            }
            //   sourcesUser.LikedByUsers.Add(userLike);
       await    _likesRepository.AddUserLike( userLike);
         await   _likesRepository.DoSomeThing();
         if (await _userRepository.SaveAllAsync()){
          return Ok();
         }     
return BadRequest("Falied to like user");

        } 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLike([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId  = User.GetUserId();
            var users=  await _likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

    }
}