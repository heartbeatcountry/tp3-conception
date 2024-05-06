namespace CineQuebec.Application.Interfaces.Services.Identity;

public interface IPasswordHashingService
{
    string HacherMdp(string mdp);
    bool ValiderMdp(string mdp, string hash);
    bool DoitEtreRehache(string hash);
}