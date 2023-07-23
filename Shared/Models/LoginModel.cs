using System.ComponentModel.DataAnnotations;

namespace ControleVendas.Shared.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres e no máximo 20 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Senha precisa ter pelo menos 3 caracteres e no máximo 20 caracteres")]
        public string Senha { get; set; }
    }
}
