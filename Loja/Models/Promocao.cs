using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loja.Models
{
    public class Promocao
    {
        [Key]
        public int PromocaoID { get; set; }
        
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Data de validade")]
        public DateTime DataValidade { get; set; }
        
        [Display(Name = "Produto")]
        public Produto produto { get; set; }

        [Required]
        [Display(Name = "Quantidade Mínima")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade mínima é 1 produto")]
        public int Quantidade { get; set; }

        [Required]
        [Display(Name = "Desconto aplicado")]
        [Range(1, int.MaxValue, ErrorMessage = "O desconto mínimo é 1%")]
        public int Desconto { get; set; }
}
}