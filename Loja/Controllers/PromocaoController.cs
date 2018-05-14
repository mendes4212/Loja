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
    public class PromocaoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Promocao
        public ActionResult Index(string erro = "")
        {
            if(!erro.Equals(""))
            {
                ModelState.AddModelError("", erro);
            }
            return View(db.Promocaos.ToList());
        }

        // GET: Promocao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promocao promocao = db.Promocaos.Find(id);
            if (promocao == null)
            {
                return HttpNotFound();
            }
            return View(promocao);
        }

        // GET: Promocao/Create
        public ActionResult Create()
        {
            var prod = PegaProdutos();
            if(prod.Count == 0)
            {
                var erro = "Não é possível cadastrar uma promoção sem produtos";
                return RedirectToAction("Index", new RouteValueDictionary(
                            new { controller = "Promocao", action = "Index", erro = erro }));
            }
            TempData["Produtos"] = PegaProdutos();
            TempData.Keep();
            return View();
        }

        public List<Produto> PegaProdutos()
        {
            var prod = db.Produtos.ToList();
            return prod;
        }

        // POST: Promocao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PromocaoID,Descricao,produtoid,DataValidade,Quantidade,Desconto")] Promocao promocao)
        {
            TempData["Produtos"] = PegaProdutos();
            TempData.Keep();


            

            var idprod = Request.Form[name: "produtoid"];
            
            try
            {
                int id = Convert.ToInt32(idprod);
                if (VerificaExistenciaPromocao(id))
                {
                    ModelState.AddModelError("", "Somente pode existir uma promoção por produto");
                    return View(promocao);
                }
                var prod = db.Produtos.FirstOrDefault(p => p.ProdutosID == id);
                promocao.produto = prod;
            }
            catch
            {
                ModelState.AddModelError("","Selecione um produto");
            }

            if(promocao.DataValidade < DateTime.Now)
            {
                ModelState.AddModelError("DataValidade", "A data da promoção deve ser maior que a data atual");
                return View(promocao);
            }
            
            if (ModelState.IsValid)
            {
                db.Promocaos.Add(promocao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(promocao);
        }

        public bool VerificaExistenciaPromocao(int idproduto,int idpromo = 0)
        {
            var promos = db.Promocaos.FirstOrDefault(p => p.produto.ProdutosID == idproduto);
            if (promos != null)
            {
                if (idpromo > 0 || promos.PromocaoID == idpromo)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        // GET: Promocao/Edit/5
        public ActionResult Edit(int? id)
        {
            TempData["Produtos"] = PegaProdutos();
            TempData.Keep();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promocao promocao = db.Promocaos.Find(id);
            if (promocao == null)
            {
                return HttpNotFound();
            }
            return View(promocao);
        }

        // POST: Promocao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PromocaoID,Descricao,DataValidade,Quantidade,Desconto")] Promocao promocao)
        {
            TempData["Produtos"] = PegaProdutos();
            TempData.Keep();

            var idprod = Request.Form[name: "produtoid"];
            try
            {
                int id = Convert.ToInt32(idprod);
                if (VerificaExistenciaPromocao(id,promocao.PromocaoID))
                {
                    ModelState.AddModelError("", "Somente pode existir uma promoção por produto");
                    return View(promocao);
                }
                var prod = db.Produtos.FirstOrDefault(p => p.ProdutosID == id);
                promocao.produto = prod;
            }
            catch
            {
                ModelState.AddModelError("", "Selecione um produto");
            }

            if (promocao.DataValidade < DateTime.Now)
            {
                ModelState.AddModelError("DataValidade", "A data da promoção deve ser maior que a data atual");
                return View(promocao);
            }

            if (ModelState.IsValid)
            {
                var promo = db.Promocaos.FirstOrDefault(p => p.PromocaoID == promocao.PromocaoID);
                db.Entry(promo).CurrentValues.SetValues(promocao);
                //db.Entry(promocao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(promocao);
        }

        // GET: Promocao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promocao promocao = db.Promocaos.Find(id);
            if (promocao == null)
            {
                return HttpNotFound();
            }
            return View(promocao);
        }

        // POST: Promocao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Promocao promocao = db.Promocaos.Find(id);
            db.Promocaos.Remove(promocao);
            db.SaveChanges();
            return RedirectToAction("Index");
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
