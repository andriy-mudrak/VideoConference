﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Claim>> Login(LoginInputModel model);
        Task<IEnumerable<Claim>> Registration(RegisterRequestViewModel model);
        Task<IEnumerable<Claim>> ExternalHandler(ClaimsPrincipal externalUser);
    }
}