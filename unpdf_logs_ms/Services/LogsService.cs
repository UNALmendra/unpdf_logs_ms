using unpdf_logs_ms.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace unpdf_logs_ms.Services;

public class LogsService
{
    private readonly IMongoCollection<Log> _logsCollection;

    public LogsService(
        IOptions<unpdf_logs_db_settings> unpdf_logs_db_settings)
    {
        var mongoClient = new MongoClient(
            unpdf_logs_db_settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            unpdf_logs_db_settings.Value.DatabaseName);

        _logsCollection = mongoDatabase.GetCollection<Log>(
            unpdf_logs_db_settings.Value.LogsCollectionName);
    }

    public async Task<List<Log>> GetAsync() =>
        await _logsCollection.Find(_ => true).ToListAsync();

    public async Task<List<Log>> GetAsync(string id) =>
        await _logsCollection.Find(x => x.Doc == id).ToListAsync();

    public async Task CreateAsync(Log newLog) =>
        await _logsCollection.InsertOneAsync(newLog);

}
