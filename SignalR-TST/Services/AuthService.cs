using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalR_TST.DTOs;
using SignalR_TST.Helpers;
using SignalR_TST.Interface;
using SignalR_TST.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace SignalR_TST.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
        }

        public async Task<AuthModel> GetJwtToken(LoginDto model)
        {
            var authModel = new AuthModel();
            ApplicationUser user;

       
                user = await _userManager.FindByNameAsync(model.Identifier);
         

            var pass = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user is null || !pass)
            {
                authModel.Massage = "Invalid CREDENTIALS!";
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            authModel.Id = user.Id;
            authModel.Username = user.UserName;
            authModel.Roles = rolesList.ToList();
            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            return authModel;
        }

        public async Task<AuthModel> RegisterAsync(UserDTO model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new AuthModel { Massage = "Username is already registered!" };
            }
            //if (!Enum.TryParse<UserType>(model.UserType.ToString(), out var userType))
            //{
            //    return new AuthModel { Massage = "Invalid user type provided!" };
            //}


            var data = _mapper.Map<ApplicationUser>(model);


            var result = await _userManager.CreateAsync(data, model.Password);   
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {

                    errors += $"{error.Description} ,";
                }
                return new AuthModel { Massage = errors };
            }
            await _userManager.AddToRoleAsync(data, "User");
            var jwtSecurityToken = await CreateJwtToken(data);
            return new AuthModel
            {
                Id = data.Id,             
                ExpiresOn = jwtSecurityToken.ValidTo,
                Name = data.Name,
                Username = data.UserName,                
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
     
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
