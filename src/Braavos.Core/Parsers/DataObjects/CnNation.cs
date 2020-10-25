using CsvHelper.Configuration.Attributes;
using System;
using System.Globalization;

namespace Braavos.Core.Parsers.DataObjects
{
    public class CnNation
    {
        [Name("Nation ID")]
        public int NationId { get; set; }

        [Name("Ruler Name")]
        public string RulerName { get; set; }

        [Name("Nation Name")]
        public string NationName { get; set; }

        [Name("Alliance")]
        public string Alliance { get; set; }

        [Name("Alliance ID")]
        public string AllianceId { get; set; }

        [Name("Alliance Date")]
        public DateTime AllianceDate { get; set; }

        [Name("Alliance Status")]
        public string AllianceStatus { get; set; }

        [Name("Government Type")]
        public string GovernmentType { get; set; }

        [Name("Religion")]
        public string Religion { get; set; }

        [Name("Team")]
        public string Team { get; set; }

        [Name("Created")]
        public DateTime Created { get; set; }

        [Name("Technology")]
        public decimal Technology { get; set; }

        [Name("Infrastructure")]
        public decimal Infrastructure { get; set; }

        [Name("Base Land")]
        public decimal BaseLand { get; set; }

        [Name("War Status")]
        public string WarStatus { get; set; }

        [Name("Resource 1")]
        public string Resource1 { get; set; }

        [Name("Resource 2")]
        public string Resource2 { get; set; }

        [Name("Votes")]
        public int Votes { get; set; }

        [Name("Strength")]
        public decimal Strength { get; set; }

        [Name("DEFCON")]
        public int Defcon { get; set; }

        [Name("Base Soldiers")]
        [NumberStyles(NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign)]
        public int BaseSoldiers { get; set; }

        [Name("Tanks")]
        [NumberStyles(NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign)]
        public int Tanks { get; set; }

        [Name("Cruise")]
        [NumberStyles(NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign)]
        public int Cruise { get; set; }

        [Name("Nukes")]
        public int Nukes { get; set; }

        [Name("Activity")]
        public string Activity { get; set; }
    }
}
