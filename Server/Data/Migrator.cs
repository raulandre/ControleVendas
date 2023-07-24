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
                
                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Usuarios_Raul'))
                BEGIN
                CREATE TABLE Usuarios_Raul (
                    Nome VARCHAR(255) NOT NULL,
                    Hash VARBINARY(max),
                    Salt VARBINARY(max),
                    {ModelBaseFields()}
                );                
                END

                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Produtos_Raul'))
                BEGIN
                CREATE TABLE Produtos_Raul (
                    Descricao VARCHAR(255) NOT NULL,
                    Preco DECIMAL(10, 2) NOT NULL,
                    Quantidade INT NOT NULL,
                    {ModelBaseFields()}
                );                
                END

                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Clientes_Raul'))
                BEGIN
                CREATE TABLE Clientes_Raul (
                    Nome VARCHAR(255) NOT NULL,
                    Nascimento DATE NOT NULL,
                    CPF VARCHAR(11) NULL,
                    CNPJ VARCHAR(14) NULL,
                    TipoPessoa INT NOT NULL,
                    Endereco VARCHAR(255) NOT NULL,
                    {ModelBaseFields()}
                );                
                END

                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Vendas_Raul'))
                BEGIN
                CREATE TABLE Vendas_Raul (
                    VendedorId INT NOT NULL,
                    ClienteId INT NOT NULL,

                    CONSTRAINT FK_VendaUsuario_Raul FOREIGN KEY (VendedorId) REFERENCES Usuarios_Raul(Id),
                    CONSTRAINT FK_VendaCliente_Raul FOREIGN KEY (ClienteId) REFERENCES Clientes_Raul(Id),
                    {ModelBaseFields()}
                );                
                END

                IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'VendaItens_Raul'))
                BEGIN
                CREATE TABLE VendaItens_Raul (
                    VendaId INT NOT NULL,
                    ProdutoId INT NOT NULL,
                    Quantidade INT NOT NULL,
                    ValorUnitario DECIMAL(10, 2) NOT NULL,
                    Desconto DECIMAL(10, 2) NOT NULL,

                    CONSTRAINT FK_VendaItemVenda_Raul FOREIGN KEY (VendaId) REFERENCES Vendas_Raul(Id),
                    CONSTRAINT FK_VendaItemProduto_Raul FOREIGN KEY (ProdutoId) REFERENCES Produtos_Raul(Id),
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
