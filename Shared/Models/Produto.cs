namespace ControleVendas.Shared.Models
{
    public class Produto : ModelBase, ICloneable
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
