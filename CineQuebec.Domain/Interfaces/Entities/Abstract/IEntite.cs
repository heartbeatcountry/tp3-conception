namespace CineQuebec.Domain.Interfaces.Entities.Abstract;

public interface IEntite
{
    Guid Id { get; }
    void SetId(Guid id);
    string ToString();
}