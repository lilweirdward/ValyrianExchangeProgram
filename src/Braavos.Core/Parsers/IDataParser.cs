using System.Collections.Generic;
using System.IO;

namespace Braavos.Core.Parsers
{
    public interface IDataParser<T>
    {
        IAsyncEnumerable<T> Parse(Stream dataStream);
    }
}
