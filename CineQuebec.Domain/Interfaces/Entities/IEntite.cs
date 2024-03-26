namespace CineQuebec.Domain.Interfaces.Entities;

public interface IEntite
{
	Guid Id { get; }
	void SetId(Guid id);
	string ToString();
}