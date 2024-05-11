namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class NbRealisateursOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(paramName, message);