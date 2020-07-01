using Braavos.Entities;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Braavos.Repository
{
    public class GoogleSheetsRepository : IBraavosRepository
    {
        public async Task<List<Nation>> GetCreditAccounts()
        {
            var service = new SheetsService(new BaseClientService.Initializer
            {
                ApplicationName = "Braavos.Repository",
                ApiKey = "AIzaSyDxWWMLw4OjQGIqZbOOnTff_H0QtJEFBOM"
            });

            var spreadsheetId = "1lFh6N2L0XNECk1uNBEsSpLNwBrCr8ovrPs-w-_2YB90";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, "VAid!C2:T");

            var response = await request.ExecuteAsync();

            var results = response.Values.Where(row => row.Count() == 18).Select(row => new Nation
            {
                Name = row[0].ToString(),
                Ruler = row[1].ToString(),
                Account = new Account
                {
                    Credit = Convert.ToInt32(row[16]),
                    Debt = Convert.ToInt32(row[17])
                }
            });

            return results.ToList();
        }
    }
}
