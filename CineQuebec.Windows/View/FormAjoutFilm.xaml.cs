using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CineQuebec.Application.Interfaces.DbContext;
using MongoDB.Driver;
using CineQuebec.Persistence.DbContext;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;


namespace CineQuebec.Windows.View
   {





    /// <summary>
    /// Logique d'interaction pour AdminHomeControl.xaml
    /// </summary>
    public partial class FormAjoutFilm : UserControl
    {
        public FormAjoutFilm()
        {
            InitializeComponent();
            InitialiserFormulaire();
        }

        private void InitialiserFormulaire()
        {
            txtTitreFilm.Clear();
            txtDureeFilm.Clear();
            cbCategorie.SelectedIndex = 0;

            foreach (ListBoxItem item in listBoxActeursFilm.Items)
            {
                item.IsSelected = false;
            }

            foreach (ListBoxItem item in listBoxRealisateursFilm.Items)
            {
                item.IsSelected = false;
            }
        }

        private async Task btnAjouterFilm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AjouterNouveauFilmAsync();
            }
            catch (Exception ex)
            {
                lblMessageErreur.Content = ex.Message;       
            }
        }

        private async Task AjouterNouveauFilmAsync()
        {
            using var unitOfWork = new UnitOfWork(null as IMongoDatabase);

            var filmCreationService = new FilmCreationService(unitOfWork);

            string titre = txtTitreFilm.Text;
            string description = txtDescriptionFilm.Text;
            CategorieFilm categorie = (CategorieFilm)cbCategorie.SelectedItem;
            DateTime dateDeSortieInternationale = dpDateSortie.SelectedDate ?? DateTime.MinValue;
            List<Acteur> acteurs = listBoxActeursFilm.SelectedItems.Cast<Acteur>().ToList();
            List<Realisateur> realisateurs = listBoxRealisateursFilm.SelectedItems.Cast<Realisateur>().ToList();
            ushort duree = Convert.ToUInt16(txtDureeFilm.Text);

            var nouvFilm = await filmCreationService.CreerFilm(titre, description, categorie, dateDeSortieInternationale, acteurs, realisateurs, duree);

            await unitOfWork.SauvegarderAsync();
        }





    }
}
