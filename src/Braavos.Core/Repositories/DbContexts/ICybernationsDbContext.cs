using Braavos.Core.Repositories.DataObjects;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories.DbContexts
{
    public interface ICybernationsDbContext
    {
        DbSet<Nation> Nations { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
