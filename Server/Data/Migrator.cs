using Dapper;
using Microsoft.Data.SqlClient;

namespace ControleVendas.Server.Data
{
    public static class Migrator
    {
        public static void Migrate(string connectiongString)
        {
            using var conn = new SqlConnection(connectiongString);

            conn.Execute(@$"
                
                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Usuarios'))
                BEGIN
                CREATE TABLE Usuarios (
                    Nome VARCHAR(255) NOT NULL,
                    Hash VARBINARY(max),
                    Salt VARBINARY(max),
                    {ModelBaseFields()}
                );                
                END

                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Produtos'))
                BEGIN
                CREATE TABLE Produtos (
                    Descricao VARCHAR(255) NOT NULL,
                    Preco DECIMAL NOT NULL,
                    Quantidade INT NOT NULL,
                    {ModelBaseFields()}
                );                
                END
            ");
        }

        public static string ModelBaseFields()
            => @"
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
                Ativo BIT NOT NULL DEFAULT 1
            ";
    }
}
