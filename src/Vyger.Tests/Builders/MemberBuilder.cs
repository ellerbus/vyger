using FluentBuilders.Core;
using Vyger.Common.Models;

namespace Vyger.Tests
{
    ///	<summary>
    ///
    ///	</summary>
    public class MemberBuilder : Builder<Member>
    {
        #region Methods

        public static implicit operator Member(MemberBuilder builder)
        {
            return builder.Create();
        }

        public override Member Create(int seed = 1)
        {
            return base.Create(seed);
        }

        protected override Member Build(int seed)
        {
            Member member = new Member()
            {
                Id = OptInFor(x => x.Id, () => "Id" + seed),
                Email = OptInFor(x => x.Email, () => "Email" + seed),
                Name = OptInFor(x => x.Name, () => "Name" + seed)
            };

            return member;
        }

        #endregion

        #region Properties

        //public MemberBuilder WithId(string id)
        //{
        //    OptInWith(x => x.Id, id);
        //
        //    return this;
        //}

        //public MemberBuilder WithEmail(string email)
        //{
        //    OptInWith(x => x.Email, email);
        //
        //    return this;
        //}

        //public MemberBuilder WithName(string name)
        //{
        //    OptInWith(x => x.Name, name);
        //
        //    return this;
        //}

        #endregion
    }
}