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
    private readonly INonSecuredObjectSpaceFactory _nonSecuredObjectSpaceFactory;
    private readonly IAuthenticationTokenProvider _tokenProvider;

    public AuthenticationController(IStandardAuthenticationService securityAuthenticationService,INonSecuredObjectSpaceFactory nonSecuredObjectSpaceFactory,IAuthenticationTokenProvider tokenProvider) {
        _securityAuthenticationService = securityAuthenticationService;
        _nonSecuredObjectSpaceFactory = nonSecuredObjectSpaceFactory;
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
    public async Task<ActionResult> LoginAsync([FromBody] [SwaggerRequestBody(@"For example: <br /> { ""userName"": ""Admin"", ""password"": """" }")]
        AuthenticationStandardLogonParameters logonParameters) {
        try {
            var user=await Task.Run(() => _securityAuthenticationService.Authenticate(logonParameters));
            if (user == null) return Unauthorized("User name or password is incorrect.");
            using var objectSpace = _nonSecuredObjectSpaceFactory.CreateNonSecuredObjectSpace<ApplicationUser>();
            var userOid = new Guid(user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var xafUser = objectSpace.FirstOrDefault<ApplicationUser>(u => u.ID == userOid);
            (((ClaimsIdentity)user.Identity)!).AddClaims(new[]{new Claim(nameof(ApplicationUser.ID),xafUser.ID.ToString()),new Claim(nameof(ApplicationUser.IsActive),xafUser.IsActive.ToString()), });
            return new SignInResult(CookieAuthenticationDefaults.AuthenticationScheme,
                user, new AuthenticationProperties { AllowRefresh = true, ExpiresUtc = DateTimeOffset.Now.AddDays(1), IsPersistent = true, });
        } catch(AuthenticationException) {
            return Unauthorized("User name or password is incorrect.");
        }
    }

    [HttpPost(nameof(LogoutAsync))]
    public async Task<SignOutResult> LogoutAsync() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new SignOutResult(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize][HttpGet(nameof(UserInfo))]
    public ActionResult UserInfo() {
        if (HttpContext.User.Identity?.IsAuthenticated == null || !HttpContext.User.Identity.IsAuthenticated)
            return Unauthorized();
        var claims = ((ClaimsIdentity)HttpContext.User.Identity).Claims.ToArray();
        return Ok(new {
            UserName= HttpContext.User.Identity.Name, 
            ID = claims.First(claim => claim.Type == nameof(ApplicationUser.ID)).Value.ToString(),
            IsActive = claims.First(claim => claim.Type == nameof(ApplicationUser.IsActive)).Value.ToString()
        });

    }
}
