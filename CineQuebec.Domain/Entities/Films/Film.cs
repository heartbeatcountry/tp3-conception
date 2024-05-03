using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Film : Entite, IComparable<Film>, IFilm
{
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
        SetActeursParId(ActeursParId.Union(acteurs));
    }

    public void AddRealisateurs(IEnumerable<Guid> realisateurs)
    {
        SetRealisateursParId(RealisateursParId.Union(realisateurs));
    }

    public void SetCategorie(Guid categorie)
    {
        if (categorie == Guid.Empty)
        {
            throw new ArgumentException("Le guid de la catégorie ne peut pas être nul.", nameof(categorie));
        }

        IdCategorie = categorie;
    }

    public void SetDateSortieInternationale(DateTime dateSortieInternationale)
    {
        if (dateSortieInternationale == DateTime.MinValue)
        {
            throw new ArgumentOutOfRangeException(nameof(dateSortieInternationale),
                $"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}.");
        }

        DateSortieInternationale = dateSortieInternationale;
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("La description ne peut pas être vide.", nameof(description));
        }

        Description = description.Trim();
    }

    public void SetDureeEnMinutes(ushort duree)
    {
        if (duree == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duree), "Le film doit durer plus de 0 minutes.");
        }

        DureeEnMinutes = duree;
    }

    public void SetTitre(string titre)
    {
        if (string.IsNullOrWhiteSpace(titre))
        {
            throw new ArgumentException("Le titre ne peut pas être vide.", nameof(titre));
        }

        Titre = titre.Trim();
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