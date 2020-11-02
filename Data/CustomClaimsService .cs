using IdentityModel;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarRental.Data
{
    public class CustomClaimsService : DefaultClaimsService
    {
        public CustomClaimsService(IProfileService profile
            , ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }
        protected override IEnumerable<Claim> GetStandardSubjectClaims(ClaimsPrincipal subject)
        {
            var claims = base.GetStandardSubjectClaims(subject);
            var newClaims = new List<Claim>(claims)
            {
                new Claim(JwtClaimTypes.Name, subject.Identity.Name)
            };
            return newClaims;
        }
    }
}
