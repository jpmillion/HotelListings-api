using HotelListingsApi.Contracts;
using HotelListingsApi.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager auth, ILogger<AccountController> logger) 
        {
            _logger = logger;
            _authManager = auth;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email}");
            IEnumerable<IdentityError> errors = await _authManager.Register(userDto);

            try
            {
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)} - User Registration attempt for {userDto.Email}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {
            _logger.LogInformation($"{login.Email} attempting to login in");
            try
            {
                AuthResponseDto authResponseDto = await _authManager.Login(login);

                if (authResponseDto == null)
                {
                    return Unauthorized();
                }

                return Ok(authResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{login.Email} Failed to login: {ex.Message}");
                return Problem($"Login failed", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            AuthResponseDto authResponseDto = await _authManager.VerifyRefreshToken(request);

            if (authResponseDto == null)
            {
                return Unauthorized();
            }

            return Ok(authResponseDto);
        }
    }
}
