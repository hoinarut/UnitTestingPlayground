using IamService;
using IamService.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Fixtures
{
    public class ApiWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContext = services.BuildServiceProvider().GetRequiredService<IamDbContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                //SeedData(dbContext);
            });
        }

        //private void SeedData(IamDbContext dbContext)
        //{
        //    var scripts = Directory.GetFiles("DbSeed");
        //    foreach (var script in scripts)
        //    {
        //        var scriptContent = new StreamReader(script).ReadToEnd();
        //        dbContext.Database.ExecuteSqlRaw(scriptContent);
        //    }
        //}
    }
}
