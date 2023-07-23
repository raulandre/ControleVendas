using ControleVendas.Server.Data;
using ControleVendas.Shared.Models;
using Dapper;

namespace ControleVendas.Server.Repositories
{
    public class VendaRepository
    {
        private DbSession _session;

        public VendaRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Venda>> GetAll()
        {
            var query = @"
                SELECT V.*, ' ' as split, VI.* FROM dbo.Vendas_Raul V
                LEFT JOIN dbo.VendaItens_Raul VI ON VI.VendaId = V.Id
            ";

            var lookup = new Dictionary<int, Venda>();

            await _session.Connection.QueryAsync<Venda, VendaItem, Venda>(query, (venda, vendaItem) =>
            {
                if (!lookup.TryGetValue(venda.Id, out var v))
                {
                    v = venda;
                    v.Itens = new();
                    lookup.Add(venda.Id, v);
                }

                v.Itens.Add(vendaItem);
                return v;
            }, splitOn: "split");

            return lookup.Values;
        }

        public async Task<int> Save(Venda v)
        {
            var query = @"
                    INSERT INTO dbo.Vendas_Raul (VendedorId, ClienteId)
                    OUTPUT INSERTED.Id
                    VALUES (@VendedorId, @ClienteId)
                ";

            var vendaId = await _session.Connection.QuerySingleOrDefaultAsync<int>(query, new
            {
                v.VendedorId,
                v.ClienteId
            });

            v.Itens.ForEach(i => i.VendaId = vendaId);

            if (vendaId != default)
            {
                query = @"
                    INSERT INTO dbo.VendaItens_Raul (VendaId, ProdutoId, Quantidade, Desconto)
                    VALUES (@VendaId, @ProdutoId, @Quantidade, @Desconto)
                ";

                await _session.Connection.ExecuteAsync(query, v.Itens);
            }

            return vendaId;

        }
    }
}
