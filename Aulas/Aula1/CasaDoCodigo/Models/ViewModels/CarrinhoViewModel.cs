using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewModels
{
    //View model nunca vai ser mapeada como tabela para o banco de dados
    public class CarrinhoViewModel
    {
        public CarrinhoViewModel(IList<ItemPedido> itens)
        {
            this.itens = itens;
        }

        public IList<ItemPedido> itens { get; } //Somente leitura
        public decimal total => itens.Sum(i => i.Quantidade * i.PrecoUnitario); 

    }
}
