using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Enums;
using Common;
using DataAccessLayer.Models;
using Newtonsoft.Json;
using DataAccessLayer;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly DataContext _internshipContext;

        CommonStrings common = new CommonStrings();

        public UserController(IUserService userService, ILogger<UserController> logger, DataContext internshipContext)
        {
            _userService = userService;
            _logger = logger;
            _internshipContext = internshipContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _internshipContext.Users
                .FirstOrDefaultAsync(x => x.Username == userObj.Username);

            if (user == null)
                return NotFound(new { Message = "User not found!" });


            if (userObj.Password != user.Password)
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }


            user.Token = CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _internshipContext.SaveChangesAsync();

            
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
            
        }
        
        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }
        

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                //new Claim(ClaimTypes.Role, user.Role),   ????
                new Claim(ClaimTypes.Name,$"{user.Username}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _internshipContext.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _internshipContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");
            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _internshipContext.SaveChangesAsync();
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        [HttpPost]
        [Route("addUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogError(common.NullUser);
                return StatusCode(SpecialCodes.Status702NotFound, common.NullUser);
            }

            if (string.IsNullOrEmpty(userDto.Username))
            {
                _logger.LogError(common.InvalidName);
                return StatusCode(SpecialCodes.Status702NotFound, common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(userDto, Formatting.Indented);
                _logger.LogInformation($"AddUser for {serializedContent}");
                if (!await _userService.AddUserAsync(userDto))
                {
                    return StatusCode(SpecialCodes.Status700HasUniqueName, common.EnterUniqueName);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogError(common.NullUser);
                return StatusCode(SpecialCodes.Status702NotFound, common.NullUser);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(userDto, Formatting.Indented);
                _logger.LogInformation($"UpdateUser for {serializedContent}");
                var response = await _userService.UpdateUserAsync(userDto);
                if (response == ValidationResult.Null)
                {
                    _logger.LogError(common.InvalidName);
                    return StatusCode(SpecialCodes.Status702NotFound, common.InvalidName);
                }
                else if (response == ValidationResult.InUse)
                {
                    _logger.LogError(common.EnterUniqueName);
                    return StatusCode(SpecialCodes.Status700HasUniqueName, common.EnterUniqueName);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {

                _logger.LogInformation("GetAllUsers");
                var userDto = await _userService.GetAllUsersAsync();

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            if (id == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"GetUserById by location id: {id}");
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return StatusCode(SpecialCodes.Status702NotFound, common.NotFoundUser);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(int id)
        {
            if (id == 0)
            {
                _logger.LogError(common.InvalidId);
                return StatusCode(SpecialCodes.Status702NotFound, common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"DeleteUserById by user id: {id}");
                await _userService.DeleteUserByIdAsync(id);

                return Ok(common.RemovedUser);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getUsersPagedAndFiltered")]
        public async Task<IActionResult> GetAllUsersPagedAsync([FromQuery] UserParameters userParameters)
        {
            if (userParameters == null)
            {
                _logger.LogError(common.NullDeviceParameters);
                return BadRequest(common.NullDeviceParameters);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(userParameters, Formatting.Indented);
                _logger.LogInformation($"GetAllUsersPaged for {serializedContent}");
                var users = await _userService.GetUsersFilteredPagedAsync(userParameters);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}