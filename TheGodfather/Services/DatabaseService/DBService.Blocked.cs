﻿#region USING_DIRECTIVES
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Npgsql;
using NpgsqlTypes;
#endregion

namespace TheGodfather.Services.Database
{
    public partial class DBService
    {
        #region BLOCKED_USERS
        public async Task AddBlockedUserAsync(ulong uid, string reason = null)
        {
            await accessSemaphore.WaitAsync();
            try {
                using (var con = await OpenConnectionAsync())
                using (var cmd = con.CreateCommand()) {
                    if (string.IsNullOrWhiteSpace(reason)) {
                        cmd.CommandText = "INSERT INTO gf.blocked_users VALUES (@uid, NULL);";
                        cmd.Parameters.AddWithValue("uid", NpgsqlDbType.Bigint, (long)uid);
                    } else {
                        cmd.CommandText = "INSERT INTO gf.blocked_users VALUES (@uid, @reason);";
                        cmd.Parameters.AddWithValue("uid", NpgsqlDbType.Bigint, (long)uid);
                        cmd.Parameters.AddWithValue("reason", NpgsqlDbType.Varchar, reason);
                    }

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            } finally {
                accessSemaphore.Release();
            }
        }

        public async Task<IReadOnlyList<(ulong, string)>> GetAllBlockedUsersAsync()
        {
            var blocked = new List<(ulong, string)>();

            await accessSemaphore.WaitAsync();
            try {
                using (var con = await OpenConnectionAsync())
                using (var cmd = con.CreateCommand()) {
                    cmd.CommandText = "SELECT * FROM gf.blocked_users;";

                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false)) {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                            blocked.Add(((ulong)(long)reader["uid"], reader["reason"] is DBNull ? null : (string)reader["reason"]));
                    }
                }
            } finally {
                accessSemaphore.Release();
            }

            return blocked.AsReadOnly();
        }

        public async Task RemoveBlockedUserAsync(ulong uid)
        {
            await accessSemaphore.WaitAsync();
            try {
                using (var con = await OpenConnectionAsync())
                using (var cmd = con.CreateCommand()) {
                    cmd.CommandText = "DELETE FROM gf.blocked_users WHERE uid = @uid;";
                    cmd.Parameters.AddWithValue("uid", NpgsqlDbType.Bigint, (long)uid);

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            } finally {
                accessSemaphore.Release();
            }
        }
        #endregion

        #region BLOCKED_CHANNELS
        public async Task AddBlockedChannelAsync(ulong cid, string reason = null)
        {
            await accessSemaphore.WaitAsync();
            try {
                using (var con = await OpenConnectionAsync())
                using (var cmd = con.CreateCommand()) {
                    if (string.IsNullOrWhiteSpace(reason)) {
                        cmd.CommandText = "INSERT INTO gf.blocked_channels VALUES (@cid, NULL);";
                        cmd.Parameters.AddWithValue("cid", NpgsqlDbType.Bigint, (long)cid);
                    } else {
                        cmd.CommandText = "INSERT INTO gf.blocked_channels VALUES (@cid, @reason);";
                        cmd.Parameters.AddWithValue("cid", NpgsqlDbType.Bigint, (long)cid);
                        cmd.Parameters.AddWithValue("reason", NpgsqlDbType.Varchar, reason);
                    }

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            } finally {
                accessSemaphore.Release();
            }
        }

        public async Task<IReadOnlyList<(ulong, string)>> GetAllBlockedChannelsAsync()
        {
            var blocked = new List<(ulong, string)>();

            await accessSemaphore.WaitAsync();
            try {
                using (var con = await OpenConnectionAsync())
                using (var cmd = con.CreateCommand()) {
                    cmd.CommandText = "SELECT * FROM gf.blocked_channels;";

                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false)) {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                            blocked.Add(((ulong)(long)reader["cid"], reader["reason"] is DBNull ? null : (string)reader["reason"]));
                    }
                }
            } finally {
                accessSemaphore.Release();
            }

            return blocked.AsReadOnly();
        }

        public async Task RemoveBlockedChannelAsync(ulong cid)
        {
            await accessSemaphore.WaitAsync();
            try {
                using (var con = await OpenConnectionAsync())
                using (var cmd = con.CreateCommand()) {
                    cmd.CommandText = "DELETE FROM gf.blocked_channels WHERE cid = @cid;";
                    cmd.Parameters.AddWithValue("cid", NpgsqlDbType.Bigint, (long)cid);

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            } finally {
                accessSemaphore.Release();
            }
        }
        #endregion
    }
}
