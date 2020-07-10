using Braavos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories
{
    public interface IBraavosRepository
    {
        Task<AuthorizedUser> Authorize(AuthRequest authRequest);
    }
}
