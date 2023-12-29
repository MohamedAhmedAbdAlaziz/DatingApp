using System.Net.WebSockets;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
      [Authorize]
    public class  UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;


        public UsersController(
            IUserRepository userRepository
        , IMapper mapper,
        IPhotoService photoService
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;

        }
        [HttpGet]
   
       public async Task<ActionResult<IEnumerable<MemberDto>>>      
        GetUsers([FromQuery] UserParams userParams)
        {
             var user= await _userRepository.
             GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername= user.UserName;
             
             if(string.IsNullOrEmpty(userParams.Gender))
             userParams.Gender= user.Gender !="male" ? "female" :"male";

            //  var users = await _userRepository
            //   .GetUsersAsync(); 
            //   var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            var users = await _userRepository.GetMembersAsync(userParams  );
            Response.AddPaginationHeader(
              users.CurrentPage
            ,users.PageSize,
           users.TotalCount,
            users.TotalPages);
              return Ok(users);
        } 
      
           [HttpGet("{username}",Name ="GetUser")]
        public  async Task<ActionResult<MemberDto>> GetUser(string username){
              var user = await _userRepository.
              GetMemberAsync(username);
                  //var usersToReturn = _mapper.Map<MemberDto>(user);
              return user;
        }
        [HttpPut]
        public async Task<ActionResult>
         UpdateUser(MemberUpdateDto memberUpdateDto){
         //   var username= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            _mapper.Map(memberUpdateDto,user);
            _userRepository.Update(user);
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failes ti update user");
        }
     [HttpPost("add-photo")]
 public async  Task<ActionResult<PhotoDto>> AddPhoto(IFormFile File)
 {
    var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        var result = await _photoService.AddPhotoAsync(File);
       if(result.Error != null) return BadRequest(result.Error.Message);

    var  photo= new Photo
    {
        Url= result.SecureUrl.AbsoluteUri,
        PublicId= result.PublicId
    };
    if(user.Photos.Count==0){
        photo.IsMain= true;
    }
    user.Photos.Add(photo);
    if(await _userRepository.SaveAllAsync())
      return CreatedAtRoute("GetUser", new {username=user.UserName} , _mapper.Map<PhotoDto>(photo));
        //return _mapper.Map<PhotoDto>(photo);
    return BadRequest("Problem Adding photo");; 
 }
 [HttpPut("set-main-photo/{photoId}")]
 public async Task<ActionResult> SetMainPhoto(int photoId) 
 {
    var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
    var photo= user.Photos.FirstOrDefault(x=> x.Id == photoId);

    if(photo.IsMain) 
    return BadRequest("this is already your main photo");
  photo.IsMain =true;
     var currentMain= user.Photos.FirstOrDefault(x=>x.IsMain  ==true);
   if(currentMain != null) 
   currentMain.IsMain =false;

   if(await _userRepository.SaveAllAsync()) 
     return NoContent();

   return BadRequest("Failed to set main photo");
  }
  [HttpDelete("delete-photo/{photoId}")]
  public async Task<ActionResult> DeletePhoto(int photoId)
  {
    var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
    var photo= user.Photos.FirstOrDefault(x=>x.Id ==photoId);
    if(photo ==null){
        return NotFound();
    }
    if(photo.IsMain) 
    return BadRequest("you cannot delete your main photo");
  if(photo.PublicId != null){
    var result=  await _photoService.DeletePhotoAsync(photo.PublicId);
   if(result.Error != null)
    return BadRequest(result.Error.Message);
  }
  user.Photos.Remove(photo);
  if(await _userRepository.SaveAllAsync()) return Ok();
  return  BadRequest("Failed to delete this photo");
  }
 }  
}