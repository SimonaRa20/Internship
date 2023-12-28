using AutoMapper;
using Blog.Constants;
using Blog.Contracts.Permissions;
using Blog.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermissionsController : Controller
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IMapper _mapper;
        public PermissionsController(IPermissionsRepository permissionsRepository, IMapper mapper)
        {
            _permissionsRepository = permissionsRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetUsersPermissions()
        {
            try
            {
                var usersPermissions = await _permissionsRepository.GetUsersPermissions();
                var permissions = _mapper.Map<List<PermisionResponse>>(usersPermissions);

                if (permissions == null || !permissions.Any())
                {
                    return NotFound("Users or their permissions not found");
                }

                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching users' permissions: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetPermissionsByUserId(int userId)
        {
            try
            {
                var permissions = await _permissionsRepository.GetPermissionsByUserId(userId);
                var permissionsResponse = _mapper.Map<PermisionResponse>(permissions);

                if (permissions == null)
                {
                    return NotFound("Permissions not found");
                }

                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching user's permissions: {ex.Message}");
            }
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdatePermissions(int userId, PermisionRequest updatedPermissions)
        {
            try
            {
                await _permissionsRepository.UpdatePermissions(userId, updatedPermissions);
                await _permissionsRepository.Save();

                return Ok("Permissions updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating permissions: {ex.Message}");
            }
        }
    }
}
