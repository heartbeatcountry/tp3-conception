namespace CineQuebec.Domain.Interfaces.Entities.Abstract;

public interface IEntite
{
    Guid Id { get; }
    string ToString();
}