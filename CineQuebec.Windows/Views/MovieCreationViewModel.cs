using System.Windows.Controls;
using System.Windows;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Persistence.DbContext;

using MongoDB.Driver;

using Stylet;

namespace CineQuebec.Windows.Views;




public class MovieCreationViewModel(
    INavigationController navigationController,
    IFilmCreationService filmCreationService) : Screen
{


    public string titreFilm { get; set; }
    public string descriptionFilm { get; set; }
    public string dureeFilm { get; set; }

    public string messageErreur { get; set; }




    public void NavigateToAdminHome()
    {
        navigationController.NavigateTo<AdminHomeViewModel>();
    }


    private void InitialiserFormulaire()
    {
        titreFilm = "";
        dureeFilm = "";
        //categorie.SelectedIndex = 0;

        //foreach (ListBoxItem item in listBoxActeursFilm.Items)
        //{
        //    item.IsSelected = false;
        //}

        //foreach (ListBoxItem item in listBoxRealisateursFilm.Items)
        //{
        //    item.IsSelected = false;
        //}
    }



    private async Task AjouterNouveauFilmAsync()
    {
        //using var unitOfWork = new UnitOfWork(null as IMongoDatabase);

        //var filmCreationService = new FilmCreationService(unitOfWork);

        string titre = titreFilm;
        string description = descriptionFilm;
        //CategorieFilm categorie = (CategorieFilm)cbCategorie.SelectedItem;
        //DateTime dateDeSortieInternationale = dpDateSortie.SelectedDate ?? DateTime.MinValue;
        //DateOnly dateDeSortieDateOnly = DateOnly.FromDateTime(dateDeSortieInternationale);
        //List<Acteur> acteurs = listBoxActeursFilm.SelectedItems.Cast<Acteur>().ToList();
        //List<Realisateur> realisateurs = listBoxRealisateursFilm.SelectedItems.Cast<Realisateur>().ToList();
        //ushort duree = Convert.ToUInt16(dureeFilm);

        //var nouvFilm = await filmCreationService.CreerFilm(titre, description, categorie, dateDeSortieDateOnly, acteurs, realisateurs, duree);

        //await unitOfWork.SauvegarderAsync();
    }






}