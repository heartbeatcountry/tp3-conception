namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class CategorieGuidNullException(string paramName, string message) : ArgumentNullException(paramName, message);