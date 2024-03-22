using MongoDB.Bson;

namespace CineQuebec.Windows.DAL.Data;

public class Abonne
{
    public ObjectId Id { get; set; }
    public string Username { get; set; }
    public DateTime DateAdhesion { get; set; }
}