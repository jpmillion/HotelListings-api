using AutoMapper;
using HotelListingsApi.Contracts;
using HotelListingsApi.Data;
using HotelListingsApi.Dto.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListingsApi.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager) 
        { 
            _mapper = mapper;
            _userManager = userManager;
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
    }
}
