﻿using HotelListingsApi.Dto.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListingsApi.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto user);
        Task<bool> Login(LoginDto login);
    }
}