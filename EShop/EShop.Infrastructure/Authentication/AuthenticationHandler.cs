using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EShop.Infrastructure.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        //private readonly JwtSecurityTokenHandler _jwtSecurityTokenhandler = new JwtSecurityTokenHandler();
        //private readonly JwtOptions _options;
        //private readonly SecurityKey _issuerSigninKey;
        //private readonly SigningCredentials _credentials;
        //private readonly JwtHeader _jwtHeader;
        //private readonly TokenValidationParameters _tokenValidationParameters;
        //private readonly IConfiguration _configuration;

        private readonly JwtOptions _jwtOptions;
        public AuthenticationHandler(IConfiguration configuration)
        {
            //_options = new JwtOptions();
            //configuration.GetSection("jwt").Bind(_options);
            //_issuerSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            //_credentials = new SigningCredentials(_issuerSigninKey, SecurityAlgorithms.HmacSha256);
            //_jwtHeader = new JwtHeader(_credentials);
            //_tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateAudience = false,
            //    ValidIssuer = _options.Issuer,
            //    IssuerSigningKey = _issuerSigninKey
            //};

            _jwtOptions = new JwtOptions
            {
                ExpiryMinutes = int.Parse(configuration.GetSection("jwt:ExpiryMinutes").Value),
                SecretKey = configuration.GetSection("jwt:SecretKey").Value,
                Issuer = configuration.GetSection("jwt:Issuer").Value
            };
        }

        public JwtAuthToken Create(string userId)
        {
            //var nowUtc = DateTime.UtcNow;
            //var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);
            //var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            //var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            //var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            //var payload = new JwtPayload
            //{
            //    {"sub",userId },
            //    {"iss", _options.Issuer },
            //    {"iat",now },
            //    {"exp", exp },
            //    {"unique_name", userId }
            //};
            //var jwt = new JwtSecurityToken(_jwtHeader, payload);
            //var token = _jwtSecurityTokenhandler.WriteToken(jwt);
            //var JsonToken = new JwtAuthToken
            //{
            //    Token = token,
            //    Expires = exp
            //};

            //return JsonToken;

            DateTime now = DateTime.UtcNow;
            Claim[] claims = new Claim[]
            {
                new Claim("Username", userId)
            };

            DateTime expires = now.AddMinutes(_jwtOptions.ExpiryMinutes);
            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var JsonToken = new JwtAuthToken
            {
                Token = token,
                Expires = _jwtOptions.ExpiryMinutes
            };
            return JsonToken;
        }
    }
}
