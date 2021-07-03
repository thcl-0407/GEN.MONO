using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using Extension;

namespace JwtManager
{
    public class JwtAuthentication : IJwtAuthentication
    {
        private string _privateKey;

        public JwtAuthentication(string privateKey)
        {
            _privateKey = privateKey;
        }

        public string GenerateAccessToken(TokenIdentity identity)
        {
            if (identity.IsValid() == false)
            {
                throw new ArgumentNullException();
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes(_privateKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Sid, identity.UserId),
                        new Claim(ClaimTypes.Name, identity.FullName),
                        new Claim(ClaimTypes.Gender, identity.Gender),
                        new Claim(ClaimTypes.DateOfBirth, identity.DateOfBirth),
                        new Claim(ClaimTypes.Email, identity.Email.IsNullOrEmpty()?"":identity.Email),
                        new Claim(ClaimTypes.OtherPhone, identity.PhoneNo.IsNullOrEmpty()?"":identity.PhoneNo),
                        new Claim(ClaimTypes.Role, identity.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1)                        
                };

                var symmetricSecurityKey = new SymmetricSecurityKey(tokenKey);
                tokenDescriptor.SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
   
                return tokenHandler.WriteToken(token);
            }
        }

        public bool isValidToken(string CurrentToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal claims = tokenHandler.ValidateToken(CurrentToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_privateKey))
                }, out SecurityToken securityToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string GetValue(GetClaimType getClaimType, string CurrentToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string value = "";

            try
            {
                ClaimsPrincipal claims = tokenHandler.ValidateToken(CurrentToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_privateKey))
                }, out SecurityToken securityToken);

                switch (getClaimType)
                {
                    case GetClaimType.USER_ID:
                        value = claims.FindFirst(ClaimTypes.Sid).Value;
                        break;
                    case GetClaimType.USER_NAME:
                        value = claims.FindFirst(ClaimTypes.Name).Value;
                        break;
                    case GetClaimType.GENDER:
                        value = claims.FindFirst(ClaimTypes.Gender).Value;
                        break;
                    case GetClaimType.DATE_OF_BIRTH:
                        value = claims.FindFirst(ClaimTypes.DateOfBirth).Value;
                        break;
                    case GetClaimType.EMAIL:
                        value = claims.FindFirst(ClaimTypes.Email).Value;
                        break;
                    case GetClaimType.ROLE:
                        value = claims.FindFirst(ClaimTypes.Role).Value;
                        break;
                    case GetClaimType.PHONE_NO:
                        value = claims.FindFirst(ClaimTypes.OtherPhone).Value;
                        break;
                }
            }
            catch
            {
                throw new SecurityTokenException();
            }

            return value;
        }    
    }
}
