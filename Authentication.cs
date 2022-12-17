using Azure;
using kobowebmvp_backend_dotnet.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace kobowebmvp_backend_dotnet
{

    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }

    public class Authentication
    {
        private const string V = "JWT:Secret";
        private IConfiguration _configuration;
        public string? Email;
        public string Password;
        public byte[]?  passwordHash;
        public byte[]? passwordSalt;
        public RefreshToken?  _RefreshToken;
        public DateTime TokenCreated;
        public DateTime TokenExpires;


        public Authentication(string email, string password, IConfiguration configuration)
        {
            Email = email;
            Password = password;
            _configuration = configuration;
            CreatePasswordHash(password);

        }


        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            // Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            string token = newRefreshToken.Token;
            TokenCreated = newRefreshToken.Created;
            TokenExpires = newRefreshToken.Expires;
         
        }


        public string CreateToken(User user)
        {
            if (user != null)
            {
                //create claims details based on the user information
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim("UserId", user.UserID.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("Lastname", user.LastName),
                        new Claim("UserType", user.UserType),
                        new Claim("Email", user.Email)
                    };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                    );



                string jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            else
            {
                return null;
            }
        }

        private void CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }

}
