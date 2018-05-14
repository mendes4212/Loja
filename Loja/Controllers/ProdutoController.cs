using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Loja.Models;

namespace Loja.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProdutoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Produto
        public ActionResult Index()
        {
            return View(db.Produtos.ToList());
        }

        [AllowAnonymous]
        // GET: Produto/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData.Remove("promos");
            Produto produto = db.Produtos.Find(id);
            var promocoes = PegaPromocao(id);
            if (promocoes.PromocaoID != 0)
            { 
                TempData["promos"] = promocoes;
                TempData.Keep();
            }
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        public Promocao PegaPromocao(int idproduto)
        {
            var promos = db.Promocaos.FirstOrDefault(p => p.produto.ProdutosID == idproduto);
            return promos;
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProdutosID,Nome,Descricao,PrecoVenda,Quantidade")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produtos.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produtos.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProdutosID,Nome,Descricao,PrecoVenda,Quantidade")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(int? id,string erro = "")
        {
            if (!erro.Equals(""))
                ModelState.AddModelError("",erro);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produtos.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if(ValidaPromocao(id))
            {
                var erro = "Não é possível apagar um produto vinculado a uma promoção";
                return RedirectToAction("Delete", new RouteValueDictionary(
                            new { controller = "Produto", action = "Delete", id = id ,erro = erro }));
            }
            else
            { 
                Produto produto = db.Produtos.Find(id);
                db.Produtos.Remove(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public bool ValidaPromocao(int id)
        {
            var produtoempromocao = db.Promocaos.FirstOrDefault(p => p.produto.ProdutosID == id);
            if (produtoempromocao != null)
                return true;
            else
                return false;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
