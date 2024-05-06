namespace CineQuebec.Application.Interfaces.Services.Identity;

public interface IPasswordValidationService
{
    public void ValiderMdpEstSecuritaire(string mdp);
}