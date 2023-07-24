using System.ComponentModel.DataAnnotations.Schema;

namespace ControleVendas.Shared.Models
{
    public class Venda : ModelBase
    {
        public int VendedorId { get; set; }
        public string Vendedor { get; set; }

        public int ClienteId { get; set; }
        public string Cliente { get; set; }

        [NotMapped]
        public List<VendaItem> Itens { get; set; }

        public double Total { get; set; }
        public double Desconto { get; set; }
    }
}
