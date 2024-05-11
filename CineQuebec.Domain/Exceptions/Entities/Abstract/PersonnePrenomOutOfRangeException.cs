namespace CineQuebec.Domain.Exceptions.Entities.Abstract;

public class PersonnePrenomOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(message, paramName);