using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Infrastructure
{
    public class FunctionOptions
    {
        public string GoogleSheetsApiKey { get; set; }
        public string GoogleSheetsSpreadsheetId { get; set; }
        public string DbConnectionString { get; set; }
    }
}
