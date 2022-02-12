using FluentMigrator.Runner;
using Items.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Extensions
{
    public static class MigrationBuilder
    {
        public static IHost Migrate(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<MigrationDb>();
                var tablemigrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    databaseService.MigrateDatabase();

                    tablemigrationService.ListMigrations();
                    tablemigrationService.MigrateUp();

                }
                catch(Exception e)
                {
                    throw;
                }
            }
            return host;
        }
    }
}
