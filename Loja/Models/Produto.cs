using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loja.Models
{
    public class Produto
    {
        [Key]
        public int ProdutosID { get; set; }
        [Display(Name = "Nome")]
        [Required]
        public string Nome { get; set; }
        [Display(Name = "Descrição")]
        [Required]
        public string Descricao { get; set; }
        [Display(Name = "Preço de venda")]
        [Required]
        public double PrecoVenda { get; set; }

        [Display(Name = "Quantidade em estoque")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade mínima é 1 produto")]
        public int Quantidade { get; set; }
    }
}