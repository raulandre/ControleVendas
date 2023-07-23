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
            return _session.Connection.QueryAsync<Produto>("SELECT * FROM dbo.Produtos_Raul");
        }

        public Task Save(Produto produto)
        {
            return _session.Connection.ExecuteAsync(@"
                INSERT INTO dbo.Produtos_Raul (Descricao, Preco, Quantidade)
                VALUES (@Descricao, @Preco, @Quantidade);
            ", new { produto.Descricao, produto.Preco, produto.Quantidade });
        }

        public Task Update(Produto produto)
        {
            return _session.Connection.ExecuteAsync(@"
                UPDATE dbo.Produtos_Raul SET
                    Descricao = @Descricao,
                    Preco = @Preco,
                    Quantidade = @Quantidade,
                    Ativo = @Ativo
                WHERE Id = @Id;
            ", new { produto.Id, produto.Descricao, produto.Preco, produto.Quantidade, produto.Ativo });
        }

        public Task UpdateEstoque(int vendaId)
        {
            return _session.Connection.ExecuteAsync(@"
                UPDATE P SET P.Quantidade = P.Quantidade - V.Quantidade
                FROM dbo.Produtos_Raul P
                INNER JOIN dbo.VendaItens_Raul V ON P.Id = V.ProdutoId
                WHERE V.VendaId = @VendaId
            ", new { VendaId = vendaId });
        }

        public Task<IEnumerable<Produto>> VerificaEstoque(List<VendaItem> itens)
        {
            _session.Connection.Execute("CREATE TABLE #ProdutoQuantidades (ProdutoId INT PRIMARY KEY, Quantidade INT)");
            _session.Connection.Execute("INSERT INTO #ProdutoQuantidades (ProdutoId, Quantidade) VALUES (@ProdutoId, @Quantidade)", itens);

            return _session.Connection.QueryAsync<Produto>(@" 
                SELECT P.* FROM dbo.Produtos_Raul P
                INNER JOIN #ProdutoQuantidades PQ ON P.Id = PQ.ProdutoId
                WHERE P.Quantidade < PQ.Quantidade
            ");
        }
    }
}
