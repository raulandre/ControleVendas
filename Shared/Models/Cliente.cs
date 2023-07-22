using ControleVendas.Shared.Enums;

namespace ControleVendas.Shared.Models
{
    public class Cliente : ModelBase, ICloneable
    {
        public string Nome { get; set; }
        public DateOnly Nascimento { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public string Endereco { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
