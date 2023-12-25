using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
  private readonly DataContext _context;
  private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public AccountController(DataContext context , 
        ITokenService tokenService,
         IMapper mapper
         )
        {
            _tokenService = tokenService;
            _mapper = mapper;

            _context = context;
        }
        [HttpPost("register")]
       public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
        
        if(await UserExists(registerDto.Username))
            return BadRequest("User is taken");
       var  user= _mapper.Map<AppUser>(registerDto);
                
        using var hmac = new HMACSHA512();
       
            user.UserName=registerDto.Username;
            user.PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt=  hmac.Key;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDto{
            Username= user.UserName,
            Token =  _tokenService.CreateToken(user),
            KnownAs =user.KnownAs
              };
       }
      [HttpPost("login")]
       public async Task<ActionResult<UserDto>> login(LoginDto loginDto){
        
        var user = await _context.Users.Include(p=>p.Photos)
        .SingleOrDefaultAsync(x=>x.UserName==loginDto.UserName.ToLower());
          
          if(user == null)
          return  Unauthorized("Invalid userName");
          using var hmac= new HMACSHA512(user.PasswordSalt);

          var ComputeHash= hmac.
          ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

          for (int i = 0; i < ComputeHash.Length; i++)
          {
            if(ComputeHash[i] != user.PasswordHash[i])
            return Unauthorized("Invalid passord");
          }            
    
         return new UserDto
         {
            Username= user.UserName,
            Token =  _tokenService.CreateToken(user),
            PhotoUrl= user.Photos.
            FirstOrDefault(x=>x.IsMain)?.Url,
          KnownAs =user.KnownAs
        };
       }
   
     private async Task<bool> UserExists(string username){
    return await _context.Users.AnyAsync(x=> x.UserName== username.ToLower());
    }
     
    
}

}