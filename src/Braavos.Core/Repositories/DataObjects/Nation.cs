using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Repositories.DataObjects
{
    public class Nation
    {
        public int NationId { get; set; }
        public string RulerName { get; set; }
        public string NationName { get; set; }
        public int? AllianceId { get; set; }
        public DateTime? AllianceDate { get; set; }
        public string AllianceStatus { get; set; }
        public GovernmentType GovernmentType { get; set; }
        public Religion Religion { get; set; }
        public Team Team { get; set; }
        public DateTime Created { get; set; }
        public decimal Technology { get; set; }
        public decimal Infrastructure { get; set; }
        public decimal BaseLand { get; set; }
        public int Votes { get; set; }
        public int Defcon { get; set; }
        public int BaseSoldiers { get; set; }
        public int Tanks { get; set; }
        public int CruiseMissiles { get; set; }
        public int Nukes { get; set; }
        public RecentActivity RecentActivity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        //public object Alliance { get; set; }
    }
}
