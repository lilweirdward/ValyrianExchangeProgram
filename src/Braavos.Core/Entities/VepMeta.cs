using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class VepMeta
    {
        public int? SellerDebtOverride { get; set; }
        public bool? SiteIsUnderMaintenance { get; set; }

        public VepMeta() { }

        public VepMeta(int debtOverride) => SellerDebtOverride = debtOverride;
    }
}
