class Carrinho {

    clickIncremento(btn) {
        let data = this.getData(btn);
        data.Quantidade++;
        this.postQuantidade(data);
    }

    clickDecremento(btn) {
        let data = this.getData(btn);
        data.Quantidade--;
        this.postQuantidade(data);
    }

    updateQuantidade(input) {
        let data = this.getData(input);
        this.postQuantidade(data);
    }

    getData(elemento) {
        var linhaDoItem = $(elemento).parents('[item-id]'); //Pega os pais que tem  'item-id'
        var itemId = $(linhaDoItem).attr('item-id'); //Pega o id 
        var quantidade = $(linhaDoItem).find('input').val(); //Pega valor

        return {
            Id: itemId,
            Quantidade: quantidade
        };
    }

    postQuantidade(data) {

        var token = $('[name=__RequestVerificationToken]').val();

        let headers = {};
        headers['RequestVerificationToken'] = token;


        $.ajax({
            url: '/Pedido/UpdateQuantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers
        })
            //Altera o valor do input de quatidade de livros
            .done(
                function (response) {
                    let itemPedido = response.itemPedido;
                    let linhaDoItem = $('[item-id=' + itemPedido.id + ']')
                    linhaDoItem.find('input').val(itemPedido.quantidade);
                } 
            );
    }

}

var carrinho = new Carrinho();