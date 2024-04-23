using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineQuebec.Domain.Interfaces.Entities.Films
{
    public interface INoteFilm : IEntite
    {

        Guid IdUtilisateur { get; }
        Guid IdFilm{ get; }
        ushort Note {  get; set; }
        void SetUtilisateur(Guid pIdUtilisateur);
        void SetFilm(Guid pIdFilm);
        void SetNoteFilm(ushort pNoteObtenue);



    }
}
