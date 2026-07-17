using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Infraestrutura.Db;

namespace Test.Infraestrutura.Db
{
    [TestClass]
    public class DbContextoTest
    {
        [TestMethod]
        public void TestResolucaoViaContainerDI_DeveResolverDbContextoSemAmbiguidade()
        {
            // ARRANGE - registra DbContexto exatamente como o Program.cs faz via AddDbContext<DbContexto>.
            // IConfiguration é registrado aqui porque, em produção, o host do ASP.NET Core
            // (WebApplicationBuilder) já registra IConfiguration automaticamente.
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.AddDbContext<DbContexto>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            var provider = services.BuildServiceProvider();

            // ACT - resolve DbContexto através do container de DI real do ASP.NET Core
            // (ActivatorUtilities só enxerga construtores públicos; se houvesse dois
            // construtores públicos com 1 parâmetro cada, isso lançaria
            // InvalidOperationException: "constructors are ambiguous")
            DbContexto? context = null;
            Exception? excecao = null;
            try
            {
                context = provider.GetRequiredService<DbContexto>();
            }
            catch (Exception ex)
            {
                excecao = ex;
            }

            // ASSERT
            Assert.IsNull(excecao, excecao?.ToString());
            Assert.IsNotNull(context);
        }
    }
}
