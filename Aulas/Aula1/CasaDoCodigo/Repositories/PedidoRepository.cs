using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        Pedido GetPedido();
        void AddItem(string codigo);
        UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido);

        Pedido UpdateCadastro(Cadastro cadastro);
    }

    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {

        private readonly IHttpContextAccessor contextAccessor;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICadastroRepository cadastroRepository;

        public PedidoRepository(ApplicationContext context, 
            IHttpContextAccessor contextAccessor, 
            IItemPedidoRepository itemPedidoRepository,
            ICadastroRepository cadastroRepository) : base(context)
        {
            this.contextAccessor = contextAccessor;
            this.itemPedidoRepository = itemPedidoRepository;
            this.cadastroRepository = cadastroRepository;
        }

        public void AddItem(string codigo)
        {
            //Verifica se existe algum produto com esse codigo
            var produto = context.Set<Produto>()
                .Where(p => p.Codigo == codigo)
                .SingleOrDefault();

            if (produto == null)
            {
                throw new ArgumentException("Pedido não encontrado!");
            }

            var pedido = GetPedido();

            //Verifica se o item já existe no pedido
            var itemPedido = context.Set<ItemPedido>()
                .Where(i => i.Produto.Codigo == codigo
                    && i.Pedido.Id == pedido.Id)
                .SingleOrDefault();

            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                context.Set<ItemPedido>()
                    .Add(itemPedido);

                context.SaveChanges();
            }
        }

        public Pedido GetPedido()
        {
            //Verifica qual o id gravado na sessão, se não tiver, cria um novo
            var pedidoId = GetPedidoId();
            var pedido = dbSet
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Include(p => p.Cadastro) 
                .Where(p => p.Id == pedidoId)
                .SingleOrDefault(); //SingleOrDefault -> se encontra o pedido retorna, senão retorna um nulo sem dar erro

            if(pedido == null)
            {
                pedido = new Pedido();
                dbSet.Add(pedido);
                context.SaveChanges();
                SetPedidoId(pedido.Id);
            }

            //Se tiver alguma informação
            return pedido;
              
        }

        // Inteiro que pode ser nulo
        private int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDB = itemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDB != null)
            {
                //Metodo para adicionar 
                itemPedidoDB.ItemQuantidade(itemPedido.Quantidade);
                context.SaveChanges();

                var carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens);

                return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
                   
            }
            throw new ArgumentException("Item pedido não encontrado");

        }

        public Pedido UpdateCadastro(Cadastro cadastro)
        {
            var pedido = GetPedido();

            cadastroRepository.Update(pedido.Cadastro.Id, cadastro);

            return pedido;
        }
    }
}
