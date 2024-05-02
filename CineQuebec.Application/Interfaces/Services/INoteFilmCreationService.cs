using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineQuebec.Application.Interfaces.Services
{
    public interface INoteFilmCreationService
    {
        Task<Guid> NoterFilm(Guid pIdFilm, byte pNote);

    }
}
