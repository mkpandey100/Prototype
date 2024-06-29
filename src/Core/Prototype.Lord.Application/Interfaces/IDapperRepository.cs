using Dapper;

namespace Prototype.Lord.Application.Interfaces;

public interface IDapperRepository
{
    Task<int> CountAsync(string sp, DynamicParameters param = null);

    Task<T> QueryAsync<T>(string sp, DynamicParameters param = null);

    Task<List<T>> QueryAllAsync<T>(string sp, DynamicParameters param = null);

    Task<(object, List<T>)> QueryAllAsync<T>(string sp, DynamicParameters param, string outputParameter);

    Task<int> ExecuteAsync(string sp, DynamicParameters param = null);

    Task<(object, int)> ExecuteAsync(string sp, DynamicParameters param, string outputParameter);
}