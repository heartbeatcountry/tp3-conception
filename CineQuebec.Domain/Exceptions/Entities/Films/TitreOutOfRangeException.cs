namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class TitreOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(paramName, message);