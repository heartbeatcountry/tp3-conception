namespace CineQuebec.Application.Records.Abstract;

public abstract record PersonneDto(Guid Id, string Prenom, string Nom) : EntityDto(Id);