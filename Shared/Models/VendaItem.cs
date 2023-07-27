using System.ComponentModel.DataAnnotations.Schema;

namespace ControleVendas.Shared.Models
{
    public class VendaItem : ModelBase
    {
        public int VendaId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public double Desconto { get; set; }
        public double ValorUnitario { get; set; }

        [NotMapped]
        public Produto Produto { get; set; }
    }
}
