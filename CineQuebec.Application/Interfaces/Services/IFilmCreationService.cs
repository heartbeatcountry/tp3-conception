using CineQuebec.Domain.Entities.Films;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineQuebec.Application.Interfaces.Services
{
    public interface IFilmCreationService
    {
        Task<Film> CreerFilm(string titre, string description, CategorieFilm categorie, DateTime dateDeSortieInternationale, List<Acteur> acteurs, List<Realisateur> realisateurs, ushort duree);
    }
}
