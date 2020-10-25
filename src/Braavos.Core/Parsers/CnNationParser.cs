using Braavos.Core.Parsers.DataObjects;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Braavos.Core.Parsers
{
    public class CnNationParser : IDataParser<CnNation>
    {
        public async IAsyncEnumerable<CnNation> Parse(Stream dataStream)
        {
            using (var reader = new StreamReader(dataStream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "|" }))
            {
                await foreach (var nationRecord in csv.GetRecordsAsync<CnNation>())
                    yield return nationRecord;
            }
        }
    }
}
