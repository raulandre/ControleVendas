using ControleVendas.Server.Data;
using ControleVendas.Shared.Models;
using Dapper;
using System.Data;

namespace ControleVendas.Server.Repositories
{
    public class UsuarioRepository
    {
        private DbSession _session;

        public UsuarioRepository(DbSession session)
        {
            _session = session;
        }

        public Task<Usuario> GetByNome(string nome)
        {
            return _session.Connection.QueryFirstOrDefaultAsync<Usuario>(@"
                SELECT * FROM Usuarios WHERE Nome = @Nome
            ", new { Nome = nome });
        }

        public Task Save(Usuario usuario)
        {
            var dp = new DynamicParameters();

            dp.Add("Nome", usuario.Nome);
            dp.Add("Hash", usuario.Hash, DbType.Binary);
            dp.Add("Salt", usuario.Salt, DbType.Binary);

            return _session.Connection.ExecuteAsync(@"
                INSERT INTO dbo.Usuarios (Nome, Hash, Salt) VALUES 
                (@Nome, @Hash, @Salt);
            ", dp);
        }
    }
}
