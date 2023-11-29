using AutoMapper;
using BookStoreMyApp.Models;
using BookStoreMyApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookStoreMyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly BookstoreDBContext _context;
        private readonly IMapper _mapper;

        private readonly JWTSettings _jwtsettings;

        public AuthenticateController(BookstoreDBContext context, IMapper mapper, IOptions<JWTSettings> jwtsettings, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _jwtsettings = jwtsettings.Value;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<UserWithToken> Login([FromBody] LoginRequest loginRequest)
        {
            var user = _context.Users.FirstOrDefault(i => i.Password == loginRequest.Password && i.EmailAddress == loginRequest.EmailAddress);
            if (user != null)
            {
                user.Role = _context.Roles.FirstOrDefault(i => i.RoleId == user.RoleId);
                RefreshToken refreshToken = GenerateRefreshToken();

                user.RefreshTokens.Add(refreshToken);
                _context.SaveChanges();

                UserWithToken userWithToken = new UserWithToken(user);
                userWithToken.RefreshToken = refreshToken.Token;

                userWithToken.AccessToken= GenerateToken(user);
                return Ok(userWithToken);

            }
            return Ok("notFound");
        }
        [Route("CreateUser")]
        [AllowAnonymous]
        [HttpPost]
        public  ActionResult<UserWithToken> PostUser([FromBody]UserViewModel user)

        {
            try
            {
                if (_context.Users.Any(i => i.EmailAddress == user.EmailAddress))
                    return Ok("resigningError");
                var defoultRole = _context.Roles.Where(i => i.RoleName == "user").FirstOrDefault();
                var userToDatabase = _mapper.Map<User>(user);
                userToDatabase.RoleId = defoultRole.RoleId;
                userToDatabase.Role = defoultRole;
                _context.Users.Add(userToDatabase);
                _context.SaveChanges();
                RefreshToken refreshToken = GenerateRefreshToken();
                userToDatabase.RefreshTokens.Add(refreshToken);
                _context.SaveChanges();

                if (user != null)
                {
                    UserWithToken userWithToken = new UserWithToken(userToDatabase);
                    userWithToken.RefreshToken = refreshToken.Token;

                    userWithToken.AccessToken = GenerateToken(userToDatabase);
                    return Ok(userWithToken);

                }
                return BadRequest("User datais not enough");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //GET: api/Auth
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<UserWithToken>> RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            User user = GetUserFromAccesToken(refreshRequest.AccessToken);
            user.Role=_context.Roles.Where(i=>i.RoleId==user.RoleId).FirstOrDefault();

            if (user != null && ValidateRefreshtoken(user, refreshRequest.RefreshToken))
            {
                UserWithToken userWithToken = new UserWithToken(user);



                userWithToken.AccessToken = GenerateToken(user);
                return userWithToken;
            }
            return null;
        }
        private bool ValidateRefreshtoken(User admin, string refreshToken)
        {
            RefreshToken refreshTokenAdmin = _context.RefreshTokens.Where(rt => rt.Token == refreshToken)
                .OrderByDescending(rt => rt.ExpiryDate).FirstOrDefault();

            if (refreshTokenAdmin != null &&
                refreshTokenAdmin.UserId == admin.UserId && refreshTokenAdmin.ExpiryDate > DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();
            var randomNumber = new byte[32];
            using (var ran = RandomNumberGenerator.Create())
            {
                ran.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddDays(2);
            return refreshToken;
        }

        private User GetUserFromAccesToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken securityToken;
                var princple = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = princple.FindFirst(ClaimTypes.Email)?.Value;

                    return _context.Users.Where(u => u.EmailAddress == userId).FirstOrDefault();

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GenerateToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecretKey"]));
            var cridentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.FirstName),
                new Claim(ClaimTypes.Email,user.EmailAddress),
                new Claim(ClaimTypes.GivenName,user.LastName),
                new Claim(ClaimTypes.Surname,user.MiddleName),
                new Claim(ClaimTypes.Role,user.Role.RoleName)
            };
            var token = new JwtSecurityToken(
                _configuration["JWTSettings:ValidIssuer"],
                _configuration["JWTSettings:ValidAudience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: cridentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
