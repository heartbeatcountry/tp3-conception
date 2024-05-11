namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class NomAffichageOutOfRangeException(string message, string paramName)
    : ArgumentNullException(paramName, message);