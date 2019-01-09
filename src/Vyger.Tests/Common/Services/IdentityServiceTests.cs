using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vyger.Common;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Tests.Common.Services
{
    [TestClass]
    public class IdentityServiceTests : BaseServiceTests<IdentityService>
    {
        #region Helpers/Test Initializers

        private class IdentityProfile : IIdentityProfile
        {
            public Member Member { get; set; }

            public string Email => Member.Email;

            public string Name => Member.Name;
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void IdentityService_CreateClaimsPrincipal_Should_CreateInactiveMember()
        {
            //  arrange
            var member = CreateA.Member.Create();

            var profile = new IdentityProfile() { Member = member };

            //  act
            var principal = SubjectUnderTest.CreateClaimsPrincipal(profile, "X");

            //  assert
            principal.Identity.Name.Should().Be(member.Email);

            principal.Claims.First(x => x.Type == Constants.GoogleTokenClaim).Value.Should().Be("X");
            principal.Claims.First(x => x.Type == ClaimTypes.Email).Value.Should().Be(member.Email);
        }

        #endregion
    }
}