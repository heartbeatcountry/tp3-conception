using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
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
using CineQuebec.Application.Interfaces.Services;
using MongoDB.Driver;


namespace CineQuebec.Windows.View
   {
    /// <summary>
    /// Logique d'interaction pour AdminHomeControl.xaml
    /// </summary>
    public partial class FormAjoutFilm : UserControl
    {
        private readonly IFilmCreationService _filmCreationService;

        public FormAjoutFilm(IFilmCreationService filmCreationService)
        {
            _filmCreationService = filmCreationService;
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

        private async Task AjouterNouveauFilmAsync()
        {

            string titre = txtTitreFilm.Text;
            string description = txtDescriptionFilm.Text;

            DateTime dateDeSortieInternationale = dpDateSortie.SelectedDate ?? DateTime.MinValue;
            //List<Acteur> acteurs = listBoxActeursFilm.SelectedItems.Cast<Acteur>().ToList();
            //List<Realisateur> realisateurs = listBoxRealisateursFilm.SelectedItems.Cast<Realisateur>().ToList();
            var duree = Convert.ToUInt16(txtDureeFilm.Text, CultureInfo.InvariantCulture);

            var nouvFilm = await _filmCreationService.CreerFilm(titre, description, Guid.NewGuid(), dateDeSortieInternationale, Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>(), duree);
        }

        private async void BtnEnregistrerFilm_OnClick(object sender, RoutedEventArgs e)
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
    }
}
