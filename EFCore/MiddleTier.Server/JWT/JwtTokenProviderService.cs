using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Authentication.ClientServer;
using Microsoft.IdentityModel.Tokens;

namespace MiddleTier.Server.JWT;

public class JwtTokenProviderService : IAuthenticationTokenProvider {
    readonly SignInManager signInManager;
    readonly IConfiguration configuration;
    public JwtTokenProviderService(SignInManager signInManager, IConfiguration configuration) {
        this.signInManager = signInManager;
        this.configuration = configuration;
    }
    public string Authenticate(object logonParameters) {
        var result = signInManager.AuthenticateByLogonParameters(logonParameters);
        if(result.Succeeded) {
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:IssuerSigningKey"]));
            var token = new JwtSecurityToken(
                //issuer: configuration["Authentication:Jwt:Issuer"],
                //audience: configuration["Authentication:Jwt:Audience"],
                claims: result.Principal.Claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        if(result.Error is IUserFriendlyException) {
            ExceptionDispatchInfo.Throw(result.Error);
        }
        throw new AuthenticationException("Internal server error");
    }
}
