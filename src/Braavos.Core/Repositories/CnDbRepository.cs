using Braavos.Core.Infrastructure;
using Braavos.Core.Repositories.DataObjects;
using Braavos.Core.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories
{
    public class CnDbRepository : ICnDbRepository
    {
        private readonly ICybernationsDbContext _cnDbContext;

        public CnDbRepository(ICybernationsDbContext cnDbContext) => _cnDbContext = cnDbContext;

        public async Task UpsertNations(IReadOnlyCollection<Nation> data)
        {
            ((DbContext)_cnDbContext).ChangeTracker.AutoDetectChangesEnabled = false;

            _cnDbContext.Nations.AddRange(data);
            await _cnDbContext.SaveChangesAsync();
        }
    }
}
