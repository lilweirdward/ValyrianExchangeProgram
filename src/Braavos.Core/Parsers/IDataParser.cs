using System.Collections.Generic;
using System.IO;

namespace Braavos.Core.Parsers
{
    public interface IDataParser
    {
        IAsyncEnumerable<T> Parse<T>(Stream dataStream);
    }
}
