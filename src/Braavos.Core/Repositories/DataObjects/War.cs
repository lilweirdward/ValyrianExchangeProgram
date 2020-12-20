using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Repositories.DataObjects
{
    public class War
    {
        public int DeclaringID { get; set; }
        public string DeclaringRuler { get; set; }
        public string DeclaringNation { get; set; }
        public string DeclaringAlliance { get; set; }
        public int DeclaringAllianceID { get; set; }
        public string DeclaringTeam { get; set; }
        public int ReceivingID { get; set; }
        public string ReceivingRuler { get; set; }
        public string ReceivingNation { get; set; }
        public string ReceivingAlliance { get; set; }
        public int ReceivingAllianceID { get; set; }
        public string ReceivingTeam { get; set; }
        public string WarStatus { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public int WarID { get; set; }
        public int Destruction { get; set; }
        public int AttackPercent { get; set; }
        public int DefendPercent { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        //public object Alliance { get; set; }
    }
}
