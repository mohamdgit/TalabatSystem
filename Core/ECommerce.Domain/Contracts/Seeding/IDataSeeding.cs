using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Seeding
{
    public interface IDataSeeding
    {
        Task DataSeedAsync();
        Task IdentityDataSeedAsync();

    }
}
