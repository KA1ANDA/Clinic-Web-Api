using ClinicWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicWebApi.Auth
{

    public interface IJwtManager
    {
        public Token GetToken(User user);
    }

    public class JwtManager : IJwtManager
    {


        private readonly IConfiguration configuration;


        public JwtManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public Token GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID" , user.Id.ToString() , ClaimValueTypes.Integer),
                    new Claim("Role" , user.Role.ToString() , ClaimValueTypes.String),



                }),

                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenData = tokenHandler.CreateToken(tokenDescriptor);
            var token = new Token { AccessToken = tokenHandler.WriteToken(tokenData), Role = user.Role};
            return token;
        }

    }

    public class Token
    {
        public string? AccessToken { get; set; }
        public int? Role { get; set; }



    }
}
