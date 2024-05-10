namespace CineQuebec.Windows;

public interface IGestionnaireExceptions
{
    void GererException(Exception exception);
    void GererException(Action action);
    Task GererExceptionAsync(Func<Task> action);
}