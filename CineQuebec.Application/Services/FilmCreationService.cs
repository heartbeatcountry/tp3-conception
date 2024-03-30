using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineQuebec.Application.Services
{
    public class FilmCreationService (IUnitOfWork unitOfWork) : IFilmCreationService
    {
     

        public async Task<Film> CreerFilm(string titre, string description, CategorieFilm categorie, DateTime dateDeSortieInternationale, List<Acteur> acteurs, List<Realisateur> realisateurs, ushort duree)
        {
            Film film = new Film(titre, description, categorie, dateDeSortieInternationale, acteurs, realisateurs, duree);
            var filmCree = await unitOfWork.FilmRepository.AjouterAsync(film);
            return filmCree;
        }
    }
}
