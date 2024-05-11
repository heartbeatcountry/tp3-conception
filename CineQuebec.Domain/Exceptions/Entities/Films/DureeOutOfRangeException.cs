namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class DureeOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(paramName, message);