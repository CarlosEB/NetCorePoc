using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace NetCorePoc.Infrastructure.CrossCutting.Security
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly TokenAuthSettings _settings;

        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options, IOptions<TokenAuthSettings> settings)
        {
            _next = next;
            _options = options.Value;
            _settings = settings.Value;

            ThrowIfInvalidOptions(_options);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

        }

        /// <summary>
        /// Method fired from .net core to execute the midleware
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(_options.TokenCreationPath, StringComparison.Ordinal))
            {
                return _next(context);
            }

            try
            {
                if ((!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)) return BadRequest(context);
                return CreateToken(context);
            }
            catch
            {
                return BadRequest(context);
            }
        }

        /// <summary>
        /// Method used to get a claim from token
        /// </summary>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public string GetClaim(HttpContext context, string claimType)
        {
            var claim = ((ClaimsIdentity)context.User.Identity).Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }

        /// <summary>
        /// Method used to return the list of claims if token is valid
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Valid claims list</returns>
        private IEnumerable<Claim> ValidateToken(HttpContext context)
        {
            var validated = new JwtSecurityTokenHandler().ValidateToken(GetTokenFromHeader(context), GetTokenValidationParameters(_settings), out SecurityToken validatedToken);
            var result = new ReadOnlyCollection<ClaimsIdentity>(validated.Identities.ToList());

            return (result.Any()) ? result.First().Claims.ToList() : null;
        }

        /// <summary>
        /// Method used to retrieve the Bearer token from header
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Bearer token or empty</returns>
        private static string GetTokenFromHeader(HttpContext context)
        {
            var authorization = context.Request.Headers["Authorization"].ToString();

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authorization.Substring("Bearer ".Length).Trim();

            return string.Empty;
        }

        /// <summary>
        /// Method used to create a bad request return
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>bad request context</returns>
        private static Task BadRequest(HttpContext context)
        {
            context.Response.StatusCode = 400;
            return context.Response.WriteAsync("Bad request.");
        }

        /// <summary>
        /// Method used to create an object with token validation parameters
        /// </summary>
        /// <param name="authSettings">token authentication settings</param>
        /// <returns>TokenValidationParameters</returns>
        private TokenValidationParameters GetTokenValidationParameters(TokenAuthSettings authSettings)
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
                ValidAudience = authSettings.SiteUrl,
                ValidIssuer = authSettings.SiteUrl,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false
            };
        }

        /// <summary>
        /// Method used to create a JWT token
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns>Token</returns>
        private async Task CreateToken(HttpContext context)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "NetCoreAccess"),
                new Claim(JwtRegisteredClaimNames.Jti, await _options.NonceGenerator()),
                new Claim("user", context.Request.Form["user"]),
                new Claim("account", context.Request.Form["account"]),
            };

            var response = new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(CreateJwtSecurityToken(claims, context.Request.Form["NetCoreAccess_key"])),
                expires_in = DateTime.UtcNow.AddDays((int)_options.Expiration.TotalDays)
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        /// <summary>
        /// Method used to create a Security token
        /// </summary>
        /// <param name="claims">List of claims</param>
        /// <returns>JwtSecurityToken object</returns>
        private JwtSecurityToken CreateJwtSecurityToken(Claim[] claims, string key)
        {
            var now = DateTime.UtcNow;

            return new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256));
        }

        /// <summary>
        /// Method used to validate the token options
        /// </summary>
        /// <param name="options"></param>
        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.TokenCreationPath))
                throw new ArgumentNullException(nameof(TokenProviderOptions.TokenCreationPath));

            if (string.IsNullOrEmpty(options.Issuer))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));

            if (string.IsNullOrEmpty(options.Audience))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));

            if (options.Expiration == TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
        }
    }
}
