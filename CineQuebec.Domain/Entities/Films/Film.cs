using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Exceptions.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Film : Entite, IComparable<Film>, IFilm
{
    public const byte MinRealisateurs = 1;
    public const byte MaxRealisateurs = 6;
    public const byte MinActeurs = 1;
    public const byte MaxActeurs = 128;
    public const byte DureeMinimum = 1;
    public const byte DureeMaximum = 255;
    public const byte LongueurMinTitre = 1;
    public const byte LongueurMaxTitre = 128;
    public const byte LongueurMinDescription = 1;
    public const ushort LongueurMaxDescription = 1024;

    private readonly HashSet<Guid> _acteursParId = [];
    private readonly HashSet<Guid> _realisateursParId = [];
    private DateOnly _dateSortieInternationale = DateOnly.MinValue;

    public Film(string titre, string description, Guid idCategorie, DateTime dateSortieInternationale,
        IEnumerable<Guid> acteursParId, IEnumerable<Guid> realisateursParId, ushort dureeEnMinutes)
    {
        SetTitre(titre);
        SetDescription(description);
        SetCategorie(idCategorie);
        SetDateSortieInternationale(dateSortieInternationale);
        AddActeurs(acteursParId);
        AddRealisateurs(realisateursParId);
        SetDureeEnMinutes(dureeEnMinutes);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Film(Guid id, string titre, string description, Guid idCategorie, DateTime dateSortieInternationale,
        IEnumerable<Guid> acteursParId, IEnumerable<Guid> realisateursParId, ushort dureeEnMinutes,
        float? noteMoyenne, uint nombreDeNotes) : this(titre,
        description, idCategorie, dateSortieInternationale, acteursParId, realisateursParId, dureeEnMinutes)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
        SetNoteMoyenne(noteMoyenne);
        SetNombreDeNotes(nombreDeNotes);
    }

    public int CompareTo(Film? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        int dateComparison = DateSortieInternationale.CompareTo(other.DateSortieInternationale);
        return dateComparison != 0 ? dateComparison : string.Compare(Titre, other.Titre, StringComparison.Ordinal);
    }

    public uint NombreDeNotes { get; private set; }

    public string Titre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid IdCategorie { get; private set; }

    public DateTime DateSortieInternationale
    {
        get => _dateSortieInternationale.ToDateTime(TimeOnly.MinValue);
        private set => _dateSortieInternationale = DateOnly.FromDateTime(value);
    }

    public IEnumerable<Guid> ActeursParId { get; private set; } = [];
    public IEnumerable<Guid> RealisateursParId { get; private set; } = [];
    public ushort DureeEnMinutes { get; private set; }
    public float? NoteMoyenne { get; private set; }

    public void AddActeurs(IEnumerable<Guid> acteurs)
    {
        HashSet<Guid> nouvActeurs = ActeursParId.Union(acteurs).ToHashSet();

        if (nouvActeurs.Count is < MinActeurs or > MaxActeurs)
        {
            throw new NbActeursOutOfRangeException(
                $"Le film doit avoir entre {MinActeurs} et {MaxActeurs} acteurs.",
                nameof(acteurs));
        }

        SetActeursParId(nouvActeurs);
    }

    public void AddRealisateurs(IEnumerable<Guid> realisateurs)
    {
        HashSet<Guid> nouvRealisateurs = RealisateursParId.Union(realisateurs).ToHashSet();

        if (nouvRealisateurs.Count is < MinRealisateurs or > MaxRealisateurs)
        {
            throw new NbRealisateursOutOfRangeException(
                $"Le film doit avoir entre {MinRealisateurs} et {MaxRealisateurs} réalisateurs.",
                nameof(realisateurs));
        }

        SetRealisateursParId(nouvRealisateurs);
    }

    public void SetCategorie(Guid categorie)
    {
        if (categorie == Guid.Empty)
        {
            throw new CategorieGuidNullException(nameof(categorie), "Le guid de la catégorie ne peut pas être nul.");
        }

        IdCategorie = categorie;
    }

    public void SetDateSortieInternationale(DateTime dateSortieInternationale)
    {
        if (dateSortieInternationale == DateTime.MinValue)
        {
            throw new DateSortieOutOfRangeException(
                $"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}.",
                nameof(dateSortieInternationale));
        }

        DateSortieInternationale = dateSortieInternationale;
    }

    public void SetDescription(string description)
    {
        description = description.Trim();

        if (description.Length is < LongueurMinDescription or > LongueurMaxDescription)
        {
            throw new DescriptionOutOfRangeException(
                $"La description doit contenir entre {LongueurMinDescription} et {LongueurMaxDescription} caractères.",
                nameof(description));
        }

        Description = description;
    }

    public void SetDureeEnMinutes(ushort duree)
    {
        if (duree is < DureeMinimum or > DureeMaximum)
        {
            throw new DureeOutOfRangeException($"Le film doit durer entre {DureeMinimum} et {DureeMaximum} minutes.",
                nameof(duree));
        }

        DureeEnMinutes = duree;
    }

    public void SetTitre(string titre)
    {
        titre = titre.Trim();

        if (titre.Length is < LongueurMinTitre or > LongueurMaxTitre)
        {
            throw new TitreOutOfRangeException(
                $"Le titre doit contenir entre {LongueurMinTitre} et {LongueurMaxTitre} caractères.",
                nameof(titre));
        }

        Titre = titre;
    }

    public override string ToString()
    {
        return $"{Titre} ({DateSortieInternationale.Year})";
    }

    public void SetActeursParId(IEnumerable<Guid> acteurs)
    {
        _acteursParId.Clear();
        _acteursParId.UnionWith(acteurs);
        ActeursParId = _acteursParId.ToArray().AsReadOnly();
    }

    public void SetRealisateursParId(IEnumerable<Guid> realisateurs)
    {
        _realisateursParId.Clear();
        _realisateursParId.UnionWith(realisateurs);
        RealisateursParId = _realisateursParId.ToArray().AsReadOnly();
    }

    public void AjouterNote(byte note)
    {
        float? sommeNotes = NoteMoyenne != null ? NoteMoyenne! * NombreDeNotes : 0;
        float? nouvelleMoyenne = (sommeNotes + note) / (NombreDeNotes + 1);

        SetNoteMoyenne(nouvelleMoyenne);
        SetNombreDeNotes(NombreDeNotes + 1);
    }

    public void ModifierNote(byte ancienneNote, byte nouvelleNote)
    {
        float? sommeNotes = NoteMoyenne != null ? NoteMoyenne! * NombreDeNotes : 0;
        float? nouvelleMoyenne = (sommeNotes - ancienneNote + nouvelleNote) / NombreDeNotes;

        SetNoteMoyenne(nouvelleMoyenne);
    }


    public new bool Equals(Entite? autre)
    {
        return base.Equals(autre) || (autre is Film film &&
                                      string.Equals(Titre, film.Titre,
                                          StringComparison.OrdinalIgnoreCase) && DateSortieInternationale.Year ==
                                      film.DateSortieInternationale.Year && DureeEnMinutes == film.DureeEnMinutes);
    }

    private void SetNoteMoyenne(float? noteFilm)
    {
        if (noteFilm is < NoteFilm.NoteMinimum or > NoteFilm.NoteMaximum)
        {
            throw new ArgumentOutOfRangeException(nameof(noteFilm),
                $"La note moyenne doit être entre {NoteFilm.NoteMinimum} et {NoteFilm.NoteMaximum}.");
        }

        NoteMoyenne = noteFilm;
    }

    private void SetNombreDeNotes(uint nombreNotes)
    {
        NombreDeNotes = nombreNotes;
    }
}