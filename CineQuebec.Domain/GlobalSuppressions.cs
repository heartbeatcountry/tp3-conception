// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target = "~M:CineQuebec.Domain.Entities.Films.Acteur.#ctor(System.Guid,System.String,System.String)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target = "~M:CineQuebec.Domain.Entities.Films.CategorieFilm.#ctor(System.Guid,System.String)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target =
            "~M:CineQuebec.Domain.Entities.Films.Film.#ctor(System.Guid,System.String,System.String,System.Guid,System.DateTime,System.Collections.Generic.IEnumerable{System.Guid},System.Collections.Generic.IEnumerable{System.Guid},System.UInt16,System.Nullable{System.Single},System.UInt32)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target = "~M:CineQuebec.Domain.Entities.Films.NoteFilm.#ctor(System.Guid,System.Guid,System.Guid,System.Byte)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target = "~M:CineQuebec.Domain.Entities.Films.Realisateur.#ctor(System.Guid,System.String,System.String)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target = "~M:CineQuebec.Domain.Entities.Projections.Billet.#ctor(System.Guid,System.Guid,System.Guid)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target =
            "~M:CineQuebec.Domain.Entities.Projections.Projection.#ctor(System.Guid,System.Guid,System.Guid,System.DateTime,System.Boolean)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target = "~M:CineQuebec.Domain.Entities.Projections.Salle.#ctor(System.Guid,System.Byte,System.UInt16)")]
[assembly:
    SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Requis pour EF Core",
        Scope = "member",
        Target =
            "~M:CineQuebec.Domain.Entities.Utilisateurs.Utilisateur.#ctor(System.Guid,System.String,System.String,System.String,System.String,System.Collections.Generic.IEnumerable{CineQuebec.Domain.Entities.Utilisateurs.Role},System.Collections.Generic.IEnumerable{System.Guid},System.Collections.Generic.IEnumerable{System.Guid},System.Collections.Generic.IEnumerable{System.Guid},System.Collections.Generic.IEnumerable{System.Guid})")]