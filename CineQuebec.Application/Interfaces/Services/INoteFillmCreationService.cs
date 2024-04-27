using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineQuebec.Application.Interfaces.Services
{
    public interface INoteFillmCreationService
    {
        Task<Guid> CreerNoteFilm(Guid pIdUtilisateur, Guid pIdFilm, ushort pNote);

    }
}
