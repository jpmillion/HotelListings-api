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
        private const string _provider = "HotelListingApi";
        private const string _tokenName = "RefreshToken";

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

            return new AuthResponseDto
            {
                Token = await GenerateToken(apiUser),
                UserId = apiUser.Id,
                RefreshToken = await CreateRefreshToken(apiUser)
                
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

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken tokenContent = tokenHandler.ReadJwtToken(authResponseDto.Token);

            string username = tokenContent.Claims.ToList().FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email)?.Value;
            ApiUser user = await _userManager.FindByNameAsync(username);

            if (user == null || authResponseDto.UserId != user.Id)
            {
                return null;
            }

            bool isValidRefreshToken = await _userManager.VerifyUserTokenAsync(user, _provider, _tokenName, authResponseDto.RefreshToken);

            if (isValidRefreshToken)
            {
                return new AuthResponseDto
                {
                    Token = await GenerateToken(user),
                    UserId = user.Id,
                    RefreshToken = await CreateRefreshToken(user),
                };
            }

            await _userManager.UpdateSecurityStampAsync(user);

            return null;
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

        private async Task<string> CreateRefreshToken(ApiUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, _provider, _tokenName);
            string newRefreshToken = await _userManager.GenerateUserTokenAsync(user, _provider, _tokenName);
            await _userManager.SetAuthenticationTokenAsync(user, _provider, _tokenName, newRefreshToken);
            return newRefreshToken;
        }
    }
}
