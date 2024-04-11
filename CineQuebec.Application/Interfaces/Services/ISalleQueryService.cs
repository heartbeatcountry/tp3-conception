using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services
{
    internal interface ISalleQueryService
    {
        Task<IEnumerable<SalleDto>> ObtenirToutes();
    }
}
