using Microsoft.Extensions.Options;
using Prototype.Lord.Application.Dto.UserDto;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Prototype.Lord.Infrastructure.Identity.Extensions
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        /// <summary>
        /// Generates claim with email, userid.
        /// </summary>
        /// <param name="claimDto"></param>
        /// <returns></returns>
        public ClaimsIdentity GenerateClaimsIdentity(ClaimDto claimDto)
        {
            try
            {
                return new ClaimsIdentity(new GenericIdentity(claimDto.Email, "Token"), AddToClaimList(claimDto));
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Serializes encoded token with expiry to json string.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponseDto> GenerateJwt(AppUserDto userDetails)
        {
            try
            {
                string authToken = await GenerateEncodeDtoken(userDetails.ClaimsIdentity);
                return new AuthenticationResponseDto
                {
                    AccessToken = authToken,
                    ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Creates a base64 encoded token with provided user claims.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        private async Task<string> GenerateEncodeDtoken(ClaimsIdentity identity)
        {
            string email = identity.Claims.Single(c => c.Type == ClaimTypes.Email).Value;

            var claimList = new List<Claim>() {
                identity.FindFirst(Authentication.JwtId),
                identity.FindFirst(Authentication.IsAdmin),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claimList,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }



        /// <summary>
        /// Adds userid to claim list.
        /// </summary>
        /// <param name="claimDto"></param>
        /// <returns></returns>
        private IEnumerable<Claim> AddToClaimList(ClaimDto claimDto)
        {
            yield return new Claim(Authentication.JwtId, claimDto.Id.ToString());
            yield return new Claim(ClaimTypes.Email, claimDto.Email);
            yield return new Claim(Authentication.IsAdmin, claimDto.IsAdmin.ToString().ToLower());
        }

        /// <summary>
        /// Throws exceptions for invalid JwtIssuerOptions.
        /// </summary>
        /// <param name="options"></param>
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(options.JtiGenerator));
        }

    }
}
