using System.Text;

using CineQuebec.Application.Interfaces.Services;

using Geralt;

namespace CineQuebec.Application.Services;

public class PasswordHashingService : IPasswordHashingService
{
    // OWASP recommande un minimum de 2 itérations à 19 MiB de mémoire:
    //     https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
    //
    // Geralt recommande un minimum de 3 itérations à 64 MiB de mémoire:
    //     https://www.geralt.xyz/password-hashing

    private const int NbIterations = 3;
    private const int TailleMemoireMiB = 64;
    private const int TailleMemoireKiB = TailleMemoireMiB * 1024;

    public bool DoitEtreRehache(string hash)
    {
        Span<byte> octetsHash = Encoding.UTF8.GetBytes(hash);

        return Argon2id.NeedsRehash(octetsHash, NbIterations, TailleMemoireKiB);
    }

    public string HacherMdp(string mdp)
    {
        Span<byte> octetsHash = stackalloc byte[Argon2id.MaxHashSize];
        Span<byte> octetsMdp = Encoding.UTF8.GetBytes(mdp);

        Argon2id.ComputeHash(octetsHash, octetsMdp, NbIterations, TailleMemoireKiB);

        return Encoding.UTF8.GetString(octetsHash);
    }

    public bool ValiderMdp(string mdp, string hash)
    {
        Span<byte> octetsHash = Encoding.UTF8.GetBytes(hash);
        Span<byte> octetsMdp = Encoding.UTF8.GetBytes(mdp);

        return Argon2id.VerifyHash(octetsHash, octetsMdp);
    }
}