using Loja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loja.Controllers
{
    public class CarrinhoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carrinho
        public ActionResult Index(Carrinho carrinho)
        {
            
            return View(carrinho);
        }

        [HttpPost]
        public ActionResult AdicionaItemCarrinho(Produto prod)
        {
            var cart = InicializaCarrinho();
            cart = ValidaExistenciaItemCarrinho(prod,cart);
            cart = AplicaPromocoes(cart);
            TempData["carrinho"] = cart;
            TempData.Keep();
            Console.Write("Aloooa");
            return View("Index",cart);
        }

        public Carrinho AplicaPromocoes(Carrinho cart)
        {
            foreach (var item in cart.produtos)
            { 
                var promocao = PegaPromocao(item.ProdutosID);
                if(promocao.DataValidade > DateTime.Now && item.Quantidade >= promocao.Quantidade)
                {
                    var subvalor = item.PrecoVenda - (item.PrecoVenda * (Convert.ToDouble(promocao.Desconto) / 100));
                    cart.ValorTotal += subvalor;
                }
                else
                    cart.ValorTotal += item.PrecoVenda * item.Quantidade;
            }
            return cart;
        }

        public Promocao PegaPromocao(int idproduto)
        {
            var promos = db.Promocaos.FirstOrDefault(p => p.produto.ProdutosID == idproduto);
            return promos;
        }

        public Carrinho ValidaExistenciaItemCarrinho(Produto prod,Carrinho cart)
        {
            if (cart.produtos == null)
            {
                cart.produtos = new List<Produto>();
                cart.produtos.Add(prod);
            }
            else
            {
                var item = cart.produtos.FirstOrDefault(p => p.ProdutosID == prod.ProdutosID);

                if (item != null)
                    item.Quantidade += prod.ProdutosID;
                else
                    cart.produtos.Add(prod);
            }
            return cart;
                
        }

        public Carrinho InicializaCarrinho()
        {
            return new Carrinho();
        }
    }
}