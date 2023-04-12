using System.Security.Claims;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Authentication;
using DevExpress.ExpressApp.Security.Authentication.ClientServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.BusinessObjects;

namespace WebAPI.API.Security;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase {
    private readonly IStandardAuthenticationService _securityAuthenticationService;
    private readonly ISecurityProvider _securityProvider;
    private readonly IAuthenticationTokenProvider _tokenProvider;

    public AuthenticationController(IStandardAuthenticationService securityAuthenticationService, ISecurityProvider securityProvider, IAuthenticationTokenProvider tokenProvider) {
        _securityAuthenticationService = securityAuthenticationService;
        _securityProvider = securityProvider;
        _tokenProvider = tokenProvider;
    }

    [HttpPost("Authenticate")]
    [SwaggerOperation("Checks if the user with the specified logon parameters exists in the database. If it does, authenticates this user.", "Refer to the following help topic for more information on authentication methods in the XAF Security System: <a href='https://docs.devexpress.com/eXpressAppFramework/119064/data-security-and-safety/security-system/authentication'>Authentication</a>.")]
    public IActionResult Authenticate(
        [FromBody]
        [SwaggerRequestBody(@"For example: <br /> { ""userName"": ""Admin"", ""password"": """" }")]
        AuthenticationStandardLogonParameters logonParameters
    ) {
        try {
            return Ok(_tokenProvider.Authenticate(logonParameters));
        }
        catch(AuthenticationException) {
            return Unauthorized("User name or password is incorrect.");
        }
    }

    [HttpPost(nameof(LoginAsync))]
    [SwaggerOperation("Checks if the user with the specified logon parameters exists in the database. If it does, authenticates this user.", "Refer to the following help topic for more information on authentication methods in the XAF Security System: <a href='https://docs.devexpress.com/eXpressAppFramework/119064/data-security-and-safety/security-system/authentication'>Authentication</a>.")]
    public async Task<IActionResult> LoginAsync([FromBody] [SwaggerRequestBody(@"For example: <br /> { ""userName"": ""Admin"", ""password"": """" }")]
        AuthenticationStandardLogonParameters logonParameters) {
        try {
            var user = _securityAuthenticationService.Authenticate(logonParameters);
            if (user == null) return Unauthorized("User name or password is incorrect.");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                user, new AuthenticationProperties { AllowRefresh = true, ExpiresUtc = DateTimeOffset.Now.AddDays(1), IsPersistent = true, });
            return Ok();
        } catch(AuthenticationException) {
            return Unauthorized("User name or password is incorrect.");
        }
    }

    [HttpPost(nameof(LogoutAsync))]
    public async Task LogoutAsync() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet(nameof(UserInfo))]
    public ActionResult UserInfo() {
        var xafUser = (ApplicationUser)_securityProvider.GetSecurity().User;
        return Ok(new {
            UserName = HttpContext.User.Identity.Name,
            XafUserId = xafUser.ID,
            LoginProviderUserId = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value,
        xafUser.Email
        }); ;
    }
}
