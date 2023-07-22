namespace ControleVendas.Shared.Models
{
    public class Usuario : ModelBase
    {
        public string Nome { get; set; }
        
        //Senha
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }

        public Usuario()
        {
            
        }

        public Usuario(string nome, byte[] hash, byte[] salt)
        {
            Nome = nome;
            Hash = hash;
            Salt = salt;
        }
    }
}
