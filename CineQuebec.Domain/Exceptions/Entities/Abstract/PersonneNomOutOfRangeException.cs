namespace CineQuebec.Domain.Exceptions.Entities.Abstract;

public class PersonneNomOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(paramName, message);