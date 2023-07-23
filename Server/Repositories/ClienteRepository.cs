using ControleVendas.Server.Data;
using ControleVendas.Shared.Models;
using Dapper;

namespace ControleVendas.Server.Repositories
{
    public class ClienteRepository
    {
        private DbSession _session;

        public ClienteRepository(DbSession session)
        {
            _session = session;
        }

        public Task<IEnumerable<Cliente>> GetAll()
        {
            return _session.Connection.QueryAsync<Cliente>("SELECT * FROM dbo.Clientes_Raul");
        }

        public Task Save(Cliente cliente)
        {
            return _session.Connection.ExecuteAsync(@"
                INSERT INTO dbo.Clientes_Raul (Nome, Nascimento, CPF, CNPJ, TipoPessoa, Endereco)
                VALUES (@Nome, @Nascimento, @CPF, @CNPJ, @TipoPessoa, @Endereco);
            ", new { cliente.Nome, cliente.Nascimento, cliente.CPF, cliente.CNPJ, cliente.TipoPessoa, cliente.Endereco });
        }

        public Task Update(Cliente cliente)
        {
            return _session.Connection.ExecuteAsync(@"
                UPDATE dbo.Clientes_Raul SET
                    Nome = @Nome,
                    Nascimento = @Nascimento,
                    CPF = @CPF,
                    CNPJ = @CNPJ,
                    TipoPessoa = @TipoPessoa,
                    Endereco = @Endereco,
                    Ativo = @Ativo
                WHERE Id = @Id;
            ", new { cliente.Nome, cliente.Nascimento, cliente.CPF, cliente.CNPJ, cliente.TipoPessoa, cliente.Endereco, cliente.Ativo, cliente.Id });
        }
    }
}
