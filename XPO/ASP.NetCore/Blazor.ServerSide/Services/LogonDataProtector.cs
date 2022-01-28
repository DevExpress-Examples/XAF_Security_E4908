using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Blazor.ServerSide.Services {
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class LogonDataProtector : ILogonDataProtector {
        public static int ExpirationMilliSeconds { get; set; }
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITimeLimitedDataProtector timeLimitedProtector;

        static LogonDataProtector() {
            ExpirationMilliSeconds = 15000;
        }
        public LogonDataProtector(IDataProtectionProvider dataProtectionProvider, IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
            var dataProtector = dataProtectionProvider.CreateProtector("DevExpress.ExpressApp.Blazor.Services.SignInService");
            timeLimitedProtector = dataProtector.ToTimeLimitedDataProtector();
        }
        public string Protect(string data) {
            var protectedData = GetDataProtector().Protect(data, lifetime: TimeSpan.FromMilliseconds(ExpirationMilliSeconds));
            return HttpUtility.UrlEncode(protectedData);
        }
        public string Unprotect(string protectedData) {
            var _protectedData = HttpUtility.UrlDecode(protectedData);
            return GetDataProtector().Unprotect(_protectedData);
        }

        private ITimeLimitedDataProtector GetDataProtector() {
            var dataProtector = timeLimitedProtector;
            string tlsToken = GetTlsTokenBinding();
            if(!String.IsNullOrEmpty(tlsToken)) {
                dataProtector = dataProtector.CreateProtector(tlsToken);
            }
            return dataProtector;
        }
        private string GetTlsTokenBinding() {
            var binding = httpContextAccessor.HttpContext?.Features.Get<ITlsTokenBindingFeature>()?.GetProvidedTokenBindingId();
            return binding == null ? null : Convert.ToBase64String(binding);
        }
    }
}
