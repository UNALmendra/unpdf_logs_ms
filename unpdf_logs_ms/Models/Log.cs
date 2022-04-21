using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace unpdf_logs_ms.Models;

public class Log
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Doc")]
    [JsonPropertyName("Doc")]
    public string Doc { get; set; }
    public string? Description { get; set; }
    public string User { get; set; }
    public DateTime Date { get; set; }

}
