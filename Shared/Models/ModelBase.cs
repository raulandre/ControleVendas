using System.ComponentModel.DataAnnotations;

namespace ControleVendas.Shared.Models
{
    public class ModelBase
    {
        [Key]
        public int Id { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
    }
}
