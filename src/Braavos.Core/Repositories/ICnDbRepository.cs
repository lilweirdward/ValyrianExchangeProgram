using Braavos.Core.Repositories.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories
{
    public interface ICnDbRepository
    {
        Task UpsertNations(IReadOnlyCollection<Nation> data, string dataFileName);
        Task UpsertWar(IReadOnlyCollection<War> data, string dataFileName);
        Task UpsertAid(IReadOnlyCollection<Aid> data, string dataFileName);
        Task UpsertAlliances(IReadOnlyCollection<Alliance> data, string dataFileName);
    }
}
