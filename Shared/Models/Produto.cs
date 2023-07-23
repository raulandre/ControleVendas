using System.ComponentModel.DataAnnotations;

namespace ControleVendas.Shared.Models
{
    public class Produto : ModelBase, ICloneable
    {
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Descrição precisa ter pelo menos 3 caracteres e no máximo 20 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Preço é obrigatório")]
        [Range(0.01d, double.MaxValue, ErrorMessage = "Preço precisa ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade precisa ser maior que zero")]
        public int Quantidade { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
