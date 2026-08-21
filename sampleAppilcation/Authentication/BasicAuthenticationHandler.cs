using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;


namespace BasicAuthApi.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            System.Text.Encodings.Web.UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Authorization header is missing"));
            }

            try
            {
                var authenticationHeader =
                    AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (!authenticationHeader.Scheme.Equals(
                    "Basic",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(
                        AuthenticateResult.Fail("Invalid authentication scheme"));
                }

                var credentials = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authenticationHeader.Parameter ?? ""));

                var parts = credentials.Split(':', 2);

                if (parts.Length != 2)
                {
                    return Task.FromResult(
                        AuthenticateResult.Fail("Invalid credentials"));
                }

                var username = parts[0];
                var password = parts[1];

                if (username != "testusername" || password != "admin123")
                {
                  
                    return Task.FromResult(
                        AuthenticateResult.Fail("Invalid username or password"));
                    throw new Exception("Check creedntials");
                }

                var claims = new[]
                {
                    new System.Security.Claims.Claim(
                        System.Security.Claims.ClaimTypes.Name,
                        username),

                    new System.Security.Claims.Claim(
                        System.Security.Claims.ClaimTypes.Role,
                        "General User")
                };

                var identity = new System.Security.Claims.ClaimsIdentity(
                    claims,
                    Scheme.Name);

                var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(
                    principal,
                    Scheme.Name);

                return Task.FromResult(
                    AuthenticateResult.Success(ticket));
            }
            catch
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Invalid Authorization header"));
            }
        }
    }
}