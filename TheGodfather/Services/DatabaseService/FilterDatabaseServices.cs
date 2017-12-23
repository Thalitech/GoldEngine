﻿#region USING_DIRECTIVES
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Npgsql;
using NpgsqlTypes;
#endregion

namespace TheGodfather.Services
{
    public partial class DatabaseService
    {
        public async Task<IReadOnlyList<Tuple<ulong, string>>> GetAllGuildFiltersAsync()
        {
            await _sem.WaitAsync();
            var filters = new List<Tuple<ulong, string>>();

            using (var con = new NpgsqlConnection(_connectionString))
            using (var cmd = con.CreateCommand()) {
                await con.OpenAsync().ConfigureAwait(false);

                cmd.CommandText = "SELECT * FROM gf.filters;";

                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false)) {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        filters.Add(new Tuple<ulong, string>((ulong)(long)reader["gid"], (string)reader["filter"]));
                }
            }

            _sem.Release();
            return filters.AsReadOnly();
        }

        public async Task<IReadOnlyList<string>> GetFiltersForGuildAsync(ulong gid)
        {
            await _sem.WaitAsync();
            var filters = new List<string>();

            using (var con = new NpgsqlConnection(_connectionString))
            using (var cmd = con.CreateCommand()) {
                await con.OpenAsync().ConfigureAwait(false);

                cmd.CommandText = "SELECT * FROM gf.filters WHERE gid = @gid;";
                cmd.Parameters.AddWithValue("gid", NpgsqlDbType.Bigint, gid);

                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false)) {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        filters.Add((string)reader["filter"]);
                }
            }

            _sem.Release();
            return filters.AsReadOnly();
        }

        public async Task AddFilterAsync(ulong gid, string filter)
        {
            await _sem.WaitAsync();

            using (var con = new NpgsqlConnection(_connectionString))
            using (var cmd = con.CreateCommand()) {
                await con.OpenAsync().ConfigureAwait(false);

                cmd.CommandText = "INSERT INTO gf.filters(gid, filter) VALUES (@gid, @filter);";
                cmd.Parameters.AddWithValue("gid", NpgsqlDbType.Bigint, gid);
                cmd.Parameters.AddWithValue("filter", NpgsqlDbType.Varchar, filter);

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }

            _sem.Release();
        }

        public async Task DeleteFilterAsync(ulong gid, string filter)
        {
            await _sem.WaitAsync();

            using (var con = new NpgsqlConnection(_connectionString))
            using (var cmd = con.CreateCommand()) {
                await con.OpenAsync().ConfigureAwait(false);

                cmd.CommandText = "DELETE FROM gf.filters WHERE gid = @gid AND filter = @filter;";
                cmd.Parameters.AddWithValue("gid", NpgsqlDbType.Bigint, gid);
                cmd.Parameters.AddWithValue("filter", NpgsqlDbType.Varchar, filter);

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }

            _sem.Release();
        }

        public async Task DeleteAllGuildFiltersAsync(ulong gid)
        {
            await _sem.WaitAsync();

            using (var con = new NpgsqlConnection(_connectionString))
            using (var cmd = con.CreateCommand()) {
                await con.OpenAsync().ConfigureAwait(false);

                cmd.CommandText = "DELETE FROM gf.filters WHERE gid = @gid;";
                cmd.Parameters.AddWithValue("gid", NpgsqlDbType.Bigint, gid);

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }

            _sem.Release();
        }
    }
}
