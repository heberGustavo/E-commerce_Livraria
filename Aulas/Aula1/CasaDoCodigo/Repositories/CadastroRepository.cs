using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICadastroRepository
    {
        Cadastro Update(int cadastroId, Cadastro cadastroNovo);
    }

    public class CadastroRepository : BaseRepository<Cadastro>, ICadastroRepository
    {
        public CadastroRepository(ApplicationContext context) : base(context)
        {
        }

        public Cadastro Update(int cadastroId, Cadastro cadastroNovo)
        {
            var cadastroDB = dbSet.Where(c => c.Id == cadastroId)
                .SingleOrDefault();

            if(cadastroDB == null)
            {
                throw new ArgumentNullException("cadastro");
            }

            cadastroDB.Update(cadastroNovo);
            context.SaveChanges();

            return cadastroDB;
        }
    }
}
