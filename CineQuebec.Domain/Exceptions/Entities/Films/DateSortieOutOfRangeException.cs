namespace CineQuebec.Domain.Exceptions.Entities.Films;

public class DateSortieOutOfRangeException(string message, string paramName)
    : ArgumentOutOfRangeException(paramName, message);