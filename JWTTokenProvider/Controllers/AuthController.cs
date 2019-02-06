using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JWTTokenProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTTokenProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // POST api/Auth/Token
        [HttpPost("token")]
        public ActionResult Token(TokenModelInPut inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // search for the username and password
            if (inputModel.Username == "username" && inputModel.Password == "pass")
            {
                var claimData = new[]
                {
                    new Claim(ClaimTypes.Name, inputModel.Username)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSecurityKey"]));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                //var signingCredentials2 = new SigningCredentials(new RsaSecurityKey(new RSACryptoServiceProvider(2048).ExportParameters(true)), SecurityAlgorithms.RsaSha256Signature);
                var expiryDate = DateTime.UtcNow.AddDays(30);
                var token = new JwtSecurityToken(
                        issuer: "mysite.com",
                        audience: "mysite.com",
                        expires: expiryDate,
                        claims: claimData,
                        signingCredentials: signingCredentials
                    );

                TokenModelOutPut tokenModel = new TokenModelOutPut()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiryDate = expiryDate
                };

                return Ok(tokenModel);
            }
            else
            {
                return NotFound();
            }
        }
    }
}