using CasaDoCodigo.Models.ViewModels;

namespace CasaDoCodigo.Models
{
    //Contem o item de pedido que foi alterado
    public class UpdateQuantidadeResponse
    {
        public UpdateQuantidadeResponse(ItemPedido itemPedido, CarrinhoViewModel carrinhoViewModel)
        {
            ItemPedido = itemPedido;
            CarrinhoViewModel = carrinhoViewModel;
        }

        public ItemPedido ItemPedido { get; }
        public CarrinhoViewModel CarrinhoViewModel { get; }

    }
}
