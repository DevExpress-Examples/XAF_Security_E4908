using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Authentication;
using DevExpress.ExpressApp.Security.Authentication.ClientServer;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.API.Security;

public class JwtTokenProviderService : IAuthenticationTokenProvider {
    readonly IStandardAuthenticationService _securityAuthenticationService;
    readonly IConfiguration _configuration;

    public JwtTokenProviderService(IStandardAuthenticationService securityAuthenticationService, IConfiguration configuration) {
        _securityAuthenticationService = securityAuthenticationService;
        _configuration = configuration;
    }
    public string Authenticate(object logonParameters) {
        ClaimsPrincipal user = _securityAuthenticationService.Authenticate(logonParameters);

        if(user != null) {
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:IssuerSigningKey"]));
            var token = new JwtSecurityToken(
                //issuer: configuration["Authentication:Jwt:Issuer"],
                //audience: configuration["Authentication:Jwt:Audience"],
                claims: user.Claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        throw new AuthenticationException("User name or password is incorrect.");
    }
}
