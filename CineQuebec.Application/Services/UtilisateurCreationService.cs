using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text.RegularExpressions;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services;

public partial class UtilisateurCreationService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IPasswordValidationService passwordValidationService,
    IPasswordHashingService passwordHashingService) : ServiceAvecValidation, IUtilisateurCreationService
{
    private readonly EmailAddressAttribute _emailAddressAttribute = new();
    private readonly IEnumerable<Role> _rolesParDefaut = [Role.Utilisateur];

    public async Task<Guid> CreerUtilisateurAsync(string prenom, string nom, string courriel, string mdp)
    {
        prenom = prenom.Trim();
        nom = nom.Trim();
        courriel = courriel.Trim().ToLowerInvariant();
        mdp = mdp.Trim();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        await EffectuerValidations(unitOfWork, prenom, nom, courriel, mdp);

        IUtilisateur utilisateur = await CreerUtilisateur(unitOfWork, prenom, nom, courriel, mdp);
        await unitOfWork.SauvegarderAsync();

        return utilisateur.Id;
    }

    [GeneratedRegex("^(?:[a-zA-Z]|[à-ü]|[À-Ü]|[- ])+$", RegexOptions.IgnoreCase)]
    private static partial Regex PrenomNomRegex();

    private async Task EffectuerValidations(IUnitOfWork unitOfWork, string prenom, string nom, string courriel,
        string mdp)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderSyntaxePrenom(prenom),
            ValiderSyntaxeNom(nom),
            ValiderSyntaxeCourriel(courriel),
            ValiderSyntaxeMdp(mdp),
            await ValiderCourrielEstUnique(unitOfWork, courriel)
        );
    }

    private async Task<IUtilisateur> CreerUtilisateur(IUnitOfWork unitOfWork, string prenom, string nom,
        string courriel, string mdp)
    {
        string hashMdp = passwordHashingService.HacherMdp(mdp);
        Utilisateur utilisateur = new(prenom, nom, courriel, hashMdp, _rolesParDefaut);
        return await unitOfWork.UtilisateurRepository.AjouterAsync(utilisateur);
    }

    private static IEnumerable<ArgumentException> ValiderSyntaxePrenom(string prenom)
    {
        if (string.IsNullOrWhiteSpace(prenom))
        {
            yield return new ArgumentException("Le prénom ne doit pas être vide.", nameof(prenom));
        }

        if (!PrenomNomRegex().IsMatch(prenom))
        {
            yield return new ArgumentException(
                "Le prénom ne doit contenir que des lettres, accents, tirets ou espaces.", nameof(prenom));
        }
    }

    private static IEnumerable<ArgumentException> ValiderSyntaxeNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            yield return new ArgumentException("Le nom ne doit pas être vide.", nameof(nom));
        }

        if (!PrenomNomRegex().IsMatch(nom))
        {
            yield return new ArgumentException("Le nom ne doit contenir que des lettres, accents, tirets ou espaces.",
                nameof(nom));
        }
    }

    private IEnumerable<ArgumentException> ValiderSyntaxeCourriel(string courriel)
    {
        if (string.IsNullOrWhiteSpace(courriel))
        {
            yield return new ArgumentException("L'adresse courriel ne doit pas être vide.", nameof(courriel));
        }

        if (!_emailAddressAttribute.IsValid(courriel))
        {
            yield return new ArgumentException("L'adresse courriel n'est pas valide.", nameof(courriel));
        }
    }

    private IEnumerable<SecurityException> ValiderSyntaxeMdp(string mdp)
    {
        try
        {
            passwordValidationService.ValiderMdpEstSecuritaire(mdp);
            return [];
        }
        catch (AggregateException ex) when (ex.InnerExceptions.Any(ie => ie is SecurityException))
        {
            return ex.InnerExceptions.OfType<SecurityException>();
        }
    }

    private static async Task<ArgumentException?> ValiderCourrielEstUnique(IUnitOfWork unitOfWork, string courriel)
    {
        return await unitOfWork.UtilisateurRepository.ExisteAsync(u => u.Courriel == courriel)
            ? new ArgumentException("Un utilisateur avec cette adresse courriel existe déjà.", nameof(courriel))
            : null;
    }
}