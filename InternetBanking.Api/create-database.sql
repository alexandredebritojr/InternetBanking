-- Script para criar o banco de dados InternetBankingDb no SQL Server
-- Execute este script no SQL Server Management Studio ou via sqlcmd

-- Criar o banco de dados
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InternetBankingDb')
BEGIN
    CREATE DATABASE InternetBankingDb;
    PRINT 'Banco de dados InternetBankingDb criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Banco de dados InternetBankingDb já existe.';
END

-- Usar o banco criado
USE InternetBankingDb;

-- Verificar se as tabelas existem
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Accounts')
BEGIN
    PRINT 'Tabelas serão criadas automaticamente pela aplicação via Entity Framework.';
END
ELSE
BEGIN
    PRINT 'Tabelas já existem no banco de dados.';
END

-- Mostrar informações do banco
SELECT 
    name as 'Nome do Banco',
    database_id as 'ID do Banco',
    create_date as 'Data de Criação',
    collation_name as 'Collation'
FROM sys.databases 
WHERE name = 'InternetBankingDb';