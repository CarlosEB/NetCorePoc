using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCorePoc.Infrastructure.CrossCutting.Security;

namespace NetCorePoc.Api
{
    public partial class Startup
    {
        /// <summary>
        /// Add the jwt bearer authentication
        /// </summary>
        /// <param name="services">The services collection</param>
        private void ConfigureServiceAuth(IServiceCollection services)
        {            
            var authSettings = Configuration.GetSection("TokenAuthSettings").Get<TokenAuthSettings>();

            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(o => { o.TokenValidationParameters = GetTokenValidationParameters(authSettings); });
        }

        /// <summary>
        /// Method used to configure the JSON Web Token authentication with options
        /// </summary>
        /// <param name="app">Application builder</param>
        private void ConfigureJwtTokenAuthentication(IApplicationBuilder app)
        {
            var authSettings = Configuration.GetSection("TokenAuthSettings").Get<TokenAuthSettings>();

            app.UseAuthentication();
            
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(CreateTokenOptions(authSettings, new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Key)))));
        }

        /// <summary>
        /// Method used to create the token options that will be used by middleware to create new JWT tokens and token params injection
        /// </summary>
        /// <param name="authSettings"></param>
        /// <param name="signingKey"></param>
        /// <returns></returns>
        private static TokenProviderOptions CreateTokenOptions(TokenAuthSettings authSettings, SymmetricSecurityKey signingKey)
        {
            return new TokenProviderOptions
            {
                TokenCreationPath = authSettings.TokenCreationPath,
                Audience = authSettings.SiteUrl,
                Issuer = authSettings.SiteUrl,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };
        }

        /// <summary>
        /// Method used to configure the Token validation parameters
        /// </summary>
        /// <returns>Validation parameters</returns>
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
    }
}
