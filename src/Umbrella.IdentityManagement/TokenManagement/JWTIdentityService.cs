using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Umbrella.IdentityManagement.Claims;
using Umbrella.IdentityManagement.ClientAuthentication;
using Umbrella.IdentityManagement.ErrorManagement;
using Umbrella.IdentityManagement.TokenManagement.Configuration;
using Umbrella.UserManagement;
using IUserRepository = Umbrella.UserManagement.IUserRepository;

namespace Umbrella.IdentityManagement.TokenManagement
{
    /// <summary>
    /// Authentication to implement JWT absic logic
    /// </summary>
    public class JwtIdentityService : IIdentityService
    {
        #region Fields
        /// <summary>
        /// Logger component
        /// </summary>
        readonly ILogger _Logger;
        readonly IUserRepository _UserRepository;
        readonly IClaimProvider _ClaimProvider;
        readonly JwtSettings _JwtOptions;
        readonly List<ClientSettings> _ClientSettings;
        #endregion

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userRepository"></param>
        /// <param name="claimprovider"></param>
        /// <param name="options"></param>
        /// <param name="clients"></param>
        public JwtIdentityService(ILogger logger, IUserRepository userRepository, IClaimProvider claimprovider, JwtSettings options, IEnumerable<ClientSettings> clients)
        {
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this._ClaimProvider = claimprovider ?? throw new ArgumentNullException(nameof(claimprovider));
            this._JwtOptions = options ?? throw new ArgumentNullException(nameof(options));
            this._ClientSettings = clients == null ? new List<ClientSettings>() : clients.ToList();
            if (!this._ClientSettings.Any())
                throw new ArgumentNullException(nameof(clients));
        }

        #region Private methods

        string GenerateToken(List<Claim> claims)
        {
            this._Logger.LogInformation("Start {Method}", nameof(GenerateToken));
            // converts the key in byte to crete proper symmetric key
            var symmetricKey = this._JwtOptions.GenerateSecurityKey();
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            // generate the token
            TimeSpan expiration = TimeSpan.FromMinutes(this._JwtOptions.TokenValidityInMinutes);
            var token = new JwtSecurityToken(
                                issuer: this._JwtOptions.ValidIssuer,
                                audience: this._JwtOptions.ValidAudience,
                                claims: claims,
                                expires: DateTime.Now.Add(expiration),
                                signingCredentials: signingCredentials);
            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            this._Logger.LogInformation("End {Method}", nameof(GenerateToken));
            return rawToken;
        }

        UserDto? GetUserByUsernameAndPassword(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            return this._UserRepository.GetByKey(username);
        }

        #endregion

        /// <summary>
        /// it tries to authenticate cliant application. if successfull, you get the token in response
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public AuthenticationResponse AuthenticateClient(string clientId, string secret)
        {
            if (String.IsNullOrEmpty(clientId))
                throw new ArgumentNullException(nameof(clientId));
            if (String.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret));

            this._Logger.LogInformation("Start {Method}", nameof(AuthenticateClient));

            // check clients
            var exsistingClient = this._ClientSettings.SingleOrDefault(x => x.ClientID.Equals(clientId, StringComparison.InvariantCultureIgnoreCase)
                                                                            && x.SecretID.Equals(secret, StringComparison.InvariantCultureIgnoreCase));
            if (exsistingClient == null)
                throw new IdentityValidationException($"Client {clientId} unauthorized!");

            // identify proper list of Claims
            var claims = this._ClaimProvider.GetByIdentityName(clientId, exsistingClient.ApplicationID).ToList();

            // create claimsIdentity
            var identity = new ClaimsIdentity(claims, "jwt");

            // returns the token
            var token = GenerateToken(claims);
            this._Logger.LogInformation("End {Method}", nameof(AuthenticateClient));
            return new AuthenticationResponse()
            { 
                Token = token, 
                Identity = identity
            };
        }
        /// <summary>
        /// Autnethicates the client by basic auth, generating back the token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>JWT Token</returns>
        public AuthenticationResponse Authenticate(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            // find user by credentials. if not found, return null token
            var user = GetUserByUsernameAndPassword(username, password);
            if (user == null)
            {
                this._Logger.LogWarning("User {Username} not found", username);
                return new AuthenticationResponse();
            }
            
            // gets the user claims
            var claims = this._ClaimProvider.GetByIdentityName(username,"DEFAULT").ToList();
            // create claimsIdentity
            var identity = new ClaimsIdentity(claims, "jwt");

            // create a token and returin it
            var token = GenerateToken(claims);
            this._Logger.LogInformation("End {Method}", nameof(Authenticate));
            return new AuthenticationResponse()
            {
                Token = token,
                Identity = identity
            };
        }
        /// <summary>
        /// Validates the token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenValidationResponse ValidateToken(string token)
        {
            // set validation parameters
            var validationParameters = this._JwtOptions.ToValidationParameters();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var userInfo = this._UserRepository.GetByKey(userId);
                if(userInfo == null)
                    throw new InvalidOperationException($"Unable to find valid user {userId}");
                return new TokenValidationResponse(jwtToken.Claims, userInfo);
            }
            catch (SecurityTokenValidationException ex)
            {
                return new TokenValidationResponse("ERR-01", "Token is invalid! " + ex.Message);
            }
            catch (Exception ex)
            {
                return new TokenValidationResponse("ERR-02", "Unexpected error during token validation: " + ex.Message);
            }
        }
    }
}