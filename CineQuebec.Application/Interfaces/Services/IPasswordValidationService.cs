namespace CineQuebec.Application.Interfaces.Services;

public interface IPasswordValidationService
{
    public void ValiderMdpEstSecuritaire(string mdp);
}