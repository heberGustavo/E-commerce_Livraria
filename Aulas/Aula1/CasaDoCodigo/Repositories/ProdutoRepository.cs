using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationContext context) : base(context)
        {
        }

        public IList<Produto> GetProdutos()
        {
            return context.Set<Produto>().ToList();
        }

        public void SalvarProdutos(List<Livro> livros)
        {
            //Listagem de dados para o banco       
            foreach (var livro in livros)
            {
                //Se não tiver o produto cadastrado, adiciona na lista
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco));
                }
            }
            context.SaveChanges();//Salva no banco
        }
    }
    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
