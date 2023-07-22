using ControleVendas.Server.Data;
using ControleVendas.Shared.Models;
using Dapper;

namespace ControleVendas.Server.Repositories
{
    public class ProdutoRepository
    {
        private DbSession _session;

        public ProdutoRepository(DbSession session)
        {
            _session = session;
        }

        public Task<IEnumerable<Produto>> GetAll()
        {
            return _session.Connection.QueryAsync<Produto>("SELECT * FROM dbo.Produtos");
        }

        public Task Save(Produto produto)
        {
            return _session.Connection.ExecuteAsync(@"
                INSERT INTO dbo.Produtos (Descricao, Preco, Quantidade)
                VALUES (@Descricao, @Preco, @Quantidade);
            ", new { produto.Descricao, produto.Preco, produto.Quantidade });
        }

        public Task Update(Produto produto)
        {
            return _session.Connection.ExecuteAsync(@"
                UPDATE dbo.Produtos SET
                    Descricao = @Descricao,
                    Preco = @Preco,
                    Quantidade = @Quantidade,
                    Ativo = @Ativo
                WHERE Id = @Id;
            ", new { produto.Id, produto.Descricao, produto.Preco, produto.Quantidade, produto.Ativo });
        }
    }
}
