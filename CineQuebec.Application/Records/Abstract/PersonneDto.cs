namespace CineQuebec.Application.Records.Abstract;

public abstract record class PersonneDto(Guid Id, string Prenom, string Nom) : EntityDto(Id);