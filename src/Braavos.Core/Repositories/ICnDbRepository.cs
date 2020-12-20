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
    }
}
