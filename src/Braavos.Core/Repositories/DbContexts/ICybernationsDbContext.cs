using Braavos.Core.Repositories.DataObjects;
using Braavos.Core.Repositories.DbContexts.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories.DbContexts
{
    public interface ICybernationsDbContext
    {
        DbSet<Nation> Nations { get; set; }

        Task InsertTempData(IReadOnlyCollection<TodaysNationData> todaysNationData);
        Task InsertTempData(IReadOnlyCollection<TodaysWarData> todaysWarData);
        Task InsertTempData(IReadOnlyCollection<TodaysAidData> todaysAidData);
        Task InsertTempData(IReadOnlyCollection<TodaysAllianceData> todaysAllianceData);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task ExecuteSqlCommand(string sql, params object[] parameters);
    }
}
