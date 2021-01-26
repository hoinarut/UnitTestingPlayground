using System;
using System.IO;
using IamService.BusinessLogic.Helpers;
using IamService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
    public class DIFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; set; }
        private static bool _dbInitialized;

        public DIFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<IamDbContext>(options => options.UseSqlite("Data Source = IamServiceTest.db"));
            serviceCollection.AddSingleton<SecurityHelper>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            serviceCollection.AddSingleton(typeof(IConfiguration), configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            SetupDb();

        }

        private void SetupDb()

        {
            if (!_dbInitialized)
            {
                var context = ServiceProvider.GetService<IamDbContext>();

                context.Database.EnsureCreated();
                _dbInitialized = true;

            }
        }

        public void Dispose()
        {
            var context = ServiceProvider.GetService<IamDbContext>();
            context.Database.EnsureDeleted();
        }
    }
}