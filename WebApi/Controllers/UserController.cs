using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Services;
using Common;
using DataAccessLayer.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        CommonStrings common = new CommonStrings();

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
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