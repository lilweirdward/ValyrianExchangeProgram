using Braavos.Core.Entities;
using Braavos.Core.Infrastructure;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories
{
    public class GoogleSheetsRepository : IBraavosRepository
    {
        private readonly string _gSheetsSpreadsheetId;
        private readonly SheetsService _sheetsService;

        public GoogleSheetsRepository(IOptions<FunctionOptions> options)
        {
            _gSheetsSpreadsheetId = options.Value.GoogleSheetsSpreadsheetId;

            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                ApplicationName = "Braavos",
                ApiKey = options.Value.GoogleSheetsApiKey
            });
        }

        public async Task<AuthorizedUser> Authorize(AuthRequest authRequest)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "VAid!C2:G");
            var response = await request.ExecuteAsync();

            var users = response.Values
                .Where(row => row.Count() == 5) // Avoid rows with no values since it jacks up the API
                .Select(row => new AuthorizedUser
                {
                    NationId = Convert.ToInt32(row[0]),
                    RulerName = row[2].ToString(),
                    UniqueCode = row[4].ToString()
                });

            return users.FirstOrDefault(user => user.UniqueCode == authRequest.UniqueCode && (user.NationId == authRequest.NationId || user.RulerName == authRequest.RulerName));
        }

        //public async Task<List<Nation>> GetCreditAccounts()
        //{
        //    var service = new SheetsService(new BaseClientService.Initializer
        //    {
        //        ApplicationName = "Braavos.Repository",
        //        ApiKey = "junk"
        //    });

        //    var spreadsheetId = "1lFh6N2L0XNECk1uNBEsSpLNwBrCr8ovrPs-w-_2YB90";
        //    var request = service.Spreadsheets.Values.Get(spreadsheetId, "VAid!C2:T");

        //    var response = await request.ExecuteAsync();

        //    var results = response.Values.Where(row => row.Count() == 18).Select(row => new Nation
        //    {
        //        Name = row[0].ToString(),
        //        Ruler = row[1].ToString(),
        //        Account = new Account
        //        {
        //            Credit = Convert.ToInt32(row[16]),
        //            Debt = Convert.ToInt32(row[17])
        //        }
        //    });

        //    return results.ToList();
        //}
    }
}
