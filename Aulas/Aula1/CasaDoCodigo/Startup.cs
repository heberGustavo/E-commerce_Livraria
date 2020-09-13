using System;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Adicionar serviços
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDistributedMemoryCache(); //Para manter as informações na memoria conforme vai navegando
            services.AddSession(); //Para poder manter o ID do Pedido ao longo da navegação

            //... 'appsettings.json' -> Passo 1 e 2
            //Passo 4 - Ultimo - (Banco)- Contexto
            string connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

            //Passo 5 - Geração das tabelas a partir do Modelo - Entity
            /*
             *  - Ferramentas;
             *  - Gerenciador de pacote NuGet
             *  - Console do Gerenciador de Pacote
             *    -> 'Add-Migration Inicial'
             *        - Criou uma pasta chamada Migration
             *    -> 'Update-Database -verbose'
             *       - Cria a tabela no SQL Server 
             *  
             */

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddTransient<ICadastroRepository, CadastroRepository>();
            services.AddTransient<IItemPedidoRepository, ItemPedidoRepository>(); 

        }

        //Consumir serviços
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{codigo?}");
            });

            //Ao rodar o app o banco de dados é criado
            serviceProvider
                .GetService<IDataService>()
                .InicializaDB();
        }
    }
}
