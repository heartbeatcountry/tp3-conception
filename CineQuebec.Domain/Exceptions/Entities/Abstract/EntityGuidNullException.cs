namespace CineQuebec.Domain.Exceptions.Entities.Abstract;

public class EntityGuidNullException(string message, string paramName) : ArgumentNullException(paramName, message);