using CineQuebec.Windows.DAL.Data;
using MongoDB.Driver;

namespace CineQuebec.Windows.DAL;

public class DatabasePeleMele
{
    private readonly IMongoClient mongoDBClient;
    private readonly IMongoDatabase database;

    public DatabasePeleMele(IMongoClient client = null)
    {
        mongoDBClient = client ?? OuvrirConnexion();
        database = ConnectDatabase();
    }

    private IMongoClient OuvrirConnexion()
    {
        MongoClient dbClient = null;
        try
        {
            dbClient = new MongoClient("mongodb://localhost:27017/");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Impossible de se connecter à la base de données " + ex.Message, "Erreur");
        }

        return dbClient;
    }

    private IMongoDatabase ConnectDatabase()
    {
        IMongoDatabase db = null;
        try
        {
            db = mongoDBClient.GetDatabase("TP2DB");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Impossible de se connecter à la base de données " + ex.Message, "Erreur");
        }

        return db;
    }

    public List<Abonne> ReadAbonnes()
    {
        var abonnes = new List<Abonne>();

        try
        {
            var collection = database.GetCollection<Abonne>("Abonnes");
            abonnes = collection.Aggregate().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Impossible d'obtenir la collection " + ex.Message, "Erreur");
        }

        return abonnes;
    }
}