using System.ComponentModel.DataAnnotations.Schema;

namespace ControleVendas.Shared.Models
{
    public class Venda : ModelBase
    {
        public int VendedorId { get; set; }
        public int ClienteId { get; set; }
        
        [NotMapped]
        public List<VendaItem> Itens { get; set; }
    }
}
