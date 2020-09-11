using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Niche.Core.Contexts
{
    public class DbContextFactory
    {
        readonly IConfiguration configuration;
        public DbContextFactory(IConfiguration appConfiguration)
        {
            configuration = appConfiguration;
        }
        public NicheContext GetContext(string dbConfigName = "ProdConnection")
        {
            var builder = new DbContextOptionsBuilder<NicheContext>()
                .UseSqlServer(configuration.GetConnectionString(dbConfigName), configAction =>
                {
                    configAction.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });

            var _context = new NicheContext(builder.Options);

            var dbConnextion = _context.Database.GetDbConnection();

            if (dbConnextion.State == System.Data.ConnectionState.Closed)
                dbConnextion.Open();

            return _context;
        }
    }
    
}
