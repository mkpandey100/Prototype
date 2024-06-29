using Dapper;
using Microsoft.Data.SqlClient;
using Prototype.Lord.Application.Interfaces;
using System.Data;

namespace Prototype.Lord.Infrastructure.Persistance.Repository;

public class DapperRepository : IDapperRepository
{
    private readonly string _connectionstring;
    private readonly int _timeOut;

    public DapperRepository(string connectionstring)
    {
        _connectionstring = connectionstring;
        _timeOut = 300;
    }

    public async Task<int> CountAsync(string sp, DynamicParameters parms = null)
    {
        using IDbConnection db = new SqlConnection(_connectionstring);
        return await db.ExecuteScalarAsync<int>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut);
    }

    public async Task<T> QueryAsync<T>(string sp, DynamicParameters parms = null)
    {
        using IDbConnection db = new SqlConnection(_connectionstring);
        return (await db.QueryAsync<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut)).FirstOrDefault();
    }

    public async Task<List<T>> QueryAllAsync<T>(string sp, DynamicParameters parms = null)
    {
        using IDbConnection db = new SqlConnection(_connectionstring);
        return (await db.QueryAsync<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut)).ToList();
    }

    public async Task<(object, List<T>)> QueryAllAsync<T>(string sp, DynamicParameters parms, string outputParameter)
    {
        using IDbConnection db = new SqlConnection(_connectionstring);
        var result = (await db.QueryAsync<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut)).ToList();
        if (!string.IsNullOrEmpty(outputParameter))
        {
            var output = parms.Get<object>(outputParameter);
            return (output, result);
        }
        return (null, result);
    }

    public async Task<int> ExecuteAsync(string sp, DynamicParameters parms = null)
    {
        int result;
        using IDbConnection db = new SqlConnection(_connectionstring);
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            using var tran = db.BeginTransaction();
            try
            {
                result = await db.ExecuteAsync(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }

        return result;
    }

    public async Task<(object, int)> ExecuteAsync(string sp, DynamicParameters parms, string outputParameter)
    {
        int result;
        object output;
        using IDbConnection db = new SqlConnection(_connectionstring);
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            using var tran = db.BeginTransaction();
            try
            {
                result = await db.ExecuteAsync(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                tran.Commit();
                output = parms.Get<object>(outputParameter);
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }

        return (output, result);
    }
}