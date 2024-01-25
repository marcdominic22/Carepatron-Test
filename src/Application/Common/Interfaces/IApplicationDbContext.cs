using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Domain;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Client> Clients { get; set; }
        IDbContextTransaction BeginTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
