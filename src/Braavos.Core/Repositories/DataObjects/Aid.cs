using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Repositories.DataObjects
{
    public class Aid
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
        public string Status { get; set; }
        public decimal Money { get; set; }
        public decimal Technology { get; set; }
        public int Soldiers { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public int AidID { get; set; }
        public RecentActivity RecentActivity { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        //public object Alliance { get; set; }
    }
}
