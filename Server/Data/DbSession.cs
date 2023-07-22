using Microsoft.Data.SqlClient;
using System.Data;

namespace ControleVendas.Server.Data
{
    public sealed class DbSession : IDisposable
    {
        private readonly Guid _id = Guid.NewGuid();
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration.GetConnectionString("SQL Server"));
            Connection.Open();
        }

        public void Dispose()
            => Connection?.Dispose();
    }
}
