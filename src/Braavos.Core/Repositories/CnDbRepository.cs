using Braavos.Core.Infrastructure;
using Braavos.Core.Repositories.DataObjects;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories
{
    public class CnDbRepository : ICnDbRepository
    {
        private readonly string _connectionString;

        public CnDbRepository(IOptions<FunctionOptions> options) => _connectionString = options.Value.DbConnectionString;

        public async Task UpsertNations(IReadOnlyCollection<Nation> data)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("SELECT public.upsert_or_delete_cn_data(@nations_data)", new
                {
                    nations_data = JsonSerializer.Serialize(data)
                });

                //await connection.OpenAsync();
                //using (var command = new NpgsqlCommand("upsert_or_delete_cn_data", connection))
                //{
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.Parameters.AddWithValue("nations_data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonSerializer.Serialize(data));

                //    await command.ExecuteNonQueryAsync();
                //}
            }
        }
    }
}
