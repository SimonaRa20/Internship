

using Route =  Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Blog.Contracts.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Blog.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, IUserRepository userRepository, IMapper mapper)
        {
            _config = config;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserData(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                UserGetResponse response = _mapper.Map<UserGetResponse>(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(int id, UserGetResponse updatedProfile)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = updatedProfile.UserName;
                user.Email = updatedProfile.Email;

                await _userRepository.UpdateUser(user);
                await _userRepository.Save();

                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
                {
                    return BadRequest("New password and confirm password do not match.");
                }

                if (string.IsNullOrWhiteSpace(changePasswordRequest.NewPassword))
                {
                    return BadRequest("Invalid new password.");
                }

                var hashedPassword = HashPassword(changePasswordRequest.NewPassword);

                await _userRepository.ChangePassword(id, hashedPassword);
                await _userRepository.Save();

                return Ok("Password updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_config["Salt"]);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashedPassword;
        }
    }
}
