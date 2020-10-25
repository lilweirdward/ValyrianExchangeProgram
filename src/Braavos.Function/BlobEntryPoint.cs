using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Braavos.Core.Parsers;
using Braavos.Core.Parsers.DataObjects;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Braavos.Function
{
    public class BlobEntryPoint
    {
        private readonly IDataParser<CnNation> _dataParser;

        public BlobEntryPoint(IDataParser<CnNation> dataParser) => _dataParser = dataParser;

        [FunctionName(nameof(CnNationsImporter))]
        public async Task CnNationsImporter([BlobTrigger("nations/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"CnNationsImporter trigger function processing blob... \n Name:{name} \n Size: {myBlob.Length} Bytes");

            // Convert CSV to data
            var allNationData = new List<CnNation>();
            try
            {
                await foreach (var fileRecord in _dataParser.Parse(myBlob))
                    allNationData.Add(fileRecord);
            }
            catch (Exception e)
            {
                log.LogError($"Critical error encountered while parsing blob. \n Message: {e.Message}\n StackTrace: {e.StackTrace}");
                return;
            }

            log.LogInformation($"File successfully parsed. {allNationData.Count} records found.");

            // Upload data to DB

            log.LogInformation($"{nameof(CnNationsImporter)} trigger function completed successfully!");
        }
    }
}
