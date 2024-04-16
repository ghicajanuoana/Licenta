using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase 
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllRoles() 
        {
            try
            {
                _logger.LogInformation("GetAllRoles");
                var rolesDto = await _roleService.GetAllRolesAsync();

                return Ok(rolesDto);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
