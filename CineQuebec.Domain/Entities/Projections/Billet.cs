using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Domain.Entities.Projections;

public class Billet : Entite, IBillet
{
    public Billet(Guid idProjection, Guid idUtilisateur)
    {
        SetUtilisateur(idUtilisateur);
        SetProjection(idProjection);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Billet(Guid id, Guid idProjection, Guid idUtilisateur) : this(idProjection, idUtilisateur)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
    }

    public Guid IdProjection { get; private set; }
    public Guid IdUtilisateur { get; private set; }


    private void SetProjection(Guid idProjection)
    {
        if (idProjection == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(idProjection), "Le guid de la projection ne peut pas être nul.");
        }

        IdProjection = idProjection;
    }

    private void SetUtilisateur(Guid idUtilisateur)
    {
        if (idUtilisateur == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(idUtilisateur), "Le guid de l'utilisateur ne peut pas être nul.");
        }

        IdUtilisateur = idUtilisateur;
    }
}