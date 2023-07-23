using ControleVendas.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleVendas.Shared.Models
{
    public class Cliente : ModelBase, ICloneable
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres e no máximo 20 caracteres")]
        public string Nome { get; set; }

        [Required]
        [Range(typeof(DateTime), "1-1-1900", "1-1-3000", ErrorMessage = "Nascimento precisa ser uma data válida")]
        public DateTime Nascimento { get; set; }
       
        public string CPF { get; set; }

        public string CNPJ { get; set; }

        public TipoPessoa TipoPessoa { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório")]
        public string Endereco { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
