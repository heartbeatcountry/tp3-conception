namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class NbActeursOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(paramName, message);