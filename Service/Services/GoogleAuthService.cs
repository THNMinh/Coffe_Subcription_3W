using Core.Interfaces.Services;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Service.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IConfiguration _config;

        public GoogleAuthService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
            {
                _config["Authentication:Google:ClientId"]
            }
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
    }
}
