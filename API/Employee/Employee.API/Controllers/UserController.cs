using Employee.API.BindingModel;
using Employee.API.DTO;
using Employee.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Employee.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {


        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JWTConfig _jWTConfig ; 


        public UserController(ILogger<UserController> logger,
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
           IOptions<JWTConfig> jwtconfig)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _jWTConfig = jwtconfig.Value;


        }

        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                var user = new AppUser()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return await Task.FromResult("User has been Registered");
                }

                return await Task.FromResult(
                    string.Join(",", result.Errors.Select(
                    x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }

        }
         
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAllUser")]

        public async Task<object> GetAllUser()
        {
            try
            {
                var users = _userManager.Users
                    .Select(x => new UserDto(
                        x.FullName, x.Email, x.UserName, x.DateCreated
                        ));
                return await Task.FromResult(users);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }

        [HttpPost("Login")]

        public async Task<object> Login([FromBody] loginBindingModel model)
        {

             try
            {
                if (ModelState.IsValid)
                {
                      

                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Paswword, false, false);
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.FindByEmailAsync(model.Email);
                        var user = new UserDto(appUser.FullName, appUser.Email, 
                            appUser.UserName, appUser.DateCreated);
                        user.Token =  GenerateToken(appUser);
                        return await Task.FromResult(user);
                        

                    }
                }
                    return await Task.FromResult("Invalid Email or password");
                

            }
            

            catch (Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
         }

    
        private string GenerateToken(AppUser user)
        {
            var jwTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jWTConfig.Key);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
                ,SecurityAlgorithms.HmacSha256)

            };
            var token = jwTokenHandler.CreateToken(tokenDescription);
            return jwTokenHandler.WriteToken(token);
        }

    }
} 
