using AutoMapper;
using HotelListingsApi.Contracts;
using HotelListingsApi.Data;
using HotelListingsApi.Dto.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingsApi.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration) 
        { 
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginDto login)
        {
            ApiUser apiUser = await _userManager.FindByEmailAsync(login.Email);
            if (apiUser == null) 
            {
                return null;
            }

            bool isValidUser = await _userManager.CheckPasswordAsync(apiUser, login.Password);

            if (!isValidUser)
            {
                return null;
            }

            string token = await GenerateToken(apiUser);

            return new AuthResponseDto
            {
                Token = token,
                UserId = apiUser.Id
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            ApiUser user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded) 
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }

        private async Task<String> GenerateToken(ApiUser user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);

            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims).Union(roleClaims);

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
