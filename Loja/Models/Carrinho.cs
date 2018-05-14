using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loja.Models
{
    public class Carrinho
    {
        public int CarrinhoID { get; set; }
        public List<Produto> produtos { get; set; }
        public double ValorTotal { get; set; }
    }
}