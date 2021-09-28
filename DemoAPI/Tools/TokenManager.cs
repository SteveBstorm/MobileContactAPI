using DataAccessLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoAPI.Tools
{
    public class TokenManager : ITokenManager
    {
        public static string secret = "Ce que vous voulez";
        //Domaine du serveur hébergeant l'api
        public static string issuer = "myapi.com";
        //Domaine du/des serveur(s) appelant l'api
        public static string audience = "myconso.com";

        public string GenerateJWT(User user)
        {
            if (user is null)
                throw new InvalidOperationException();

            //Création des crédentials
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            //creer l'objet accueillant les info utilisateur + info token
            Claim[] myClaims = new[]
            {
                new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user"),
                new Claim("UserId", user.Id.ToString())
            };

            //Generation du token => nuget System.IdentityModel.Tokens.Jwt;
            JwtSecurityToken token = new JwtSecurityToken(
                claims: myClaims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1),
                issuer: issuer,
                audience: audience
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);

        }
    }
}
