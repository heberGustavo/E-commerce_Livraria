using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly ApplicationContext context;
        private readonly IProdutoRepository produtoRepository;

        public DataService(ApplicationContext context, IProdutoRepository produtoRepository)
        {
            this.context = context;
            this.produtoRepository = produtoRepository;
        }

        public void InicializaDB()
        {
            context.Database.EnsureCreated();
            
            List<Livro> livros = GetLivros();

            produtoRepository.SalvarProdutos(livros);
        }

        private static List<Livro> GetLivros()
        {
            //Ler arquivo JSON
            var json = File.ReadAllText("livros.json");
            //Converter para lista de objetos
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }
    }
        
}
