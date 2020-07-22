using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class AuthorizedUser
    {
        public int NationId { get; set; }
        public string RulerName { get; set; }
        public string UniqueCode { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is AuthorizedUser otherUser))
                return false;

            return
                NationId == otherUser.NationId &&
                RulerName == otherUser.RulerName &&
                UniqueCode == otherUser.UniqueCode;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = NationId.GetHashCode();
                hashCode = (hashCode * 397) ^ RulerName.GetHashCode();
                hashCode = (hashCode * 397) ^ UniqueCode.GetHashCode();
                return hashCode;
            }
        }
    }
}
