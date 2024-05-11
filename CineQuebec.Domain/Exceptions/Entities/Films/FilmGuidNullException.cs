namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class FilmGuidNullException(string message, string paramName) : ArgumentNullException(paramName, message);