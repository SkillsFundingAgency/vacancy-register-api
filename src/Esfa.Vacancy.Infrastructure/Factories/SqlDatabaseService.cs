﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Factories
{
    public class SqlDatabaseService
    {
        private readonly IProvideSettings _settings;
        private readonly ILog _logger;

        public SqlDatabaseService(IProvideSettings settings, ILog logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public SqlConnection GetConnection()
        {
            var connectionString =
                _settings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            return new SqlConnection(connectionString);
        }

        public async Task<T> ExecuteWithRetryAsync<T>(string operationName, Func<SqlConnection, Task<T>> asyncFunc)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Database operation {operationName} failed with error: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            var result = await retry.ExecuteAsync(async () =>
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();

                    return await asyncFunc(conn);
                }
            });

            return result;
        }
    }
}