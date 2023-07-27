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

        public async Task<IEnumerable<Venda>> GetAll(int vendedorId, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"
                 SELECT V.Id, V.VendedorId, V.ClienteId, U.Nome AS Vendedor, C.Nome AS Cliente,
                ' ' as split, 
                VI.*,
                ' ' as split, 
                P.*
                FROM dbo.Vendas_Raul V
                INNER JOIN dbo.VendaItens_Raul VI ON VI.VendaId = V.Id
                INNER JOIN dbo.Usuarios_Raul U ON U.Id = V.VendedorId
                INNER JOIN dbo.Clientes_Raul C ON C.Id = V.ClienteId
                INNER JOIN dbo.Produtos_Raul P ON P.Id = VI.ProdutoId
                WHERE V.VendedorId = @vendedorId AND V.Timestamp BETWEEN @dataInicio AND @dataFim
                ORDER BY V.Timestamp DESC
            ";

            var lookup = new Dictionary<int, Venda>();

            await _session.Connection.QueryAsync<Venda, VendaItem, Produto, Venda>(query, (venda, vendaItem, produto) =>
            {
                if (!lookup.TryGetValue(venda.Id, out var v))
                {
                    v = venda;
                    v.Itens = new();
                    lookup.Add(venda.Id, v);
                }

                vendaItem.Produto = produto;
                v.Itens.Add(vendaItem);
                return v;
            }, splitOn: "split", 
            param: new
            {
                vendedorId,
                dataInicio = dataInicio.Date,
                dataFim = dataFim.Date
            });

            var vendas = lookup.Values;

            var vendasMapeadas = vendas.GroupBy(v => v.Id).Select(g => new Venda
            {
                Id = g.Key,
                Vendedor = g.First().Vendedor,
                Cliente = g.First().Cliente,
                Itens = g.SelectMany(i => i.Itens).ToList(),
            }).ToList();

            vendasMapeadas.ForEach(v =>
            {
                v.Total = v.Itens.Sum(i => i.Quantidade * i.ValorUnitario);
                v.Desconto = v.Itens.Sum(i => i.Quantidade * i.ValorUnitario * i.Desconto / 100f);
            });

            return vendasMapeadas;
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
                    INSERT INTO dbo.VendaItens_Raul (VendaId, ProdutoId, Quantidade, Desconto, ValorUnitario)
                    VALUES (@VendaId, @ProdutoId, @Quantidade, @Desconto, @ValorUnitario)
                ";

                await _session.Connection.ExecuteAsync(query, v.Itens);
            }

            return vendaId;

        }
    }
}
