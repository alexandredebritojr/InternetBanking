# Internet Banking API

API do Sistema de Caixa de Banco - Desenvolvida em .NET 8 com Clean Architecture.

## 📋 Funcionalidades

### 1. Cadastro de Contas Bancárias
- Cadastro de novas contas para clientes
- Validação de dados obrigatórios (nome e documento)
- Verificação de unicidade do documento
- Saldo inicial de R$ 1.000,00 (bonificação)
- Registro automático da data de abertura

### 2. Consulta de Contas
- Listagem de contas cadastradas
- Filtros por nome, documento ou status
- Retorna: nome, documento, saldo, data de abertura e status

### 3. Inativação de Conta
- Desativação de contas por documento
- Preservação de dados históricos
- Log de auditoria

### 4. Transferências
- Transferências entre contas ativas
- Validação de saldo suficiente
- Transações atômicas (débito/crédito)
- Log de auditoria

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server LocalDB (ou SQLite para desenvolvimento)

### Passos
1. Clone o repositório
2. Navegue até a pasta do projeto
3. Restaure as dependências:
   ```bash
   dotnet restore
   ```
4. Execute a aplicação:
   ```bash
   dotnet run --project InternetBanking.Api
   ```
5. Acesse a documentação da API em: `https://localhost:7000` (Swagger)

## 📚 Documentação da API

### Endpoints Principais

#### Contas
- `POST /api/accounts` - Cadastrar nova conta
- `GET /api/accounts` - Listar contas (com filtros)
- `GET /api/accounts/{id}` - Buscar conta por ID
- `GET /api/accounts/document/{document}` - Buscar conta por documento
- `PUT /api/accounts/document/{document}/deactivate` - Desativar conta

#### Transferências
- `POST /api/transfers` - Realizar transferência
- `GET /api/transfers/account/{document}` - Listar transações da conta

#### Auditoria
- `GET /api/audit` - Listar logs de auditoria (com filtros)

### Exemplos de Uso

#### Cadastrar Conta
```json
POST /api/accounts
{
  "clientName": "João Silva",
  "document": "12345678901"
}
```

#### Realizar Transferência
```json
POST /api/transfers
{
  "fromDocument": "12345678901",
  "toDocument": "98765432100",
  "amount": 500.00,
  "description": "Transferência de teste"
}
```

#### Filtrar Contas
```
GET /api/accounts?name=João&status=1
GET /api/accounts?document=123
```

## 🧪 Testes

### Executar Testes Unitários
```bash
dotnet test InternetBanking.UnitTests
```

### Executar Testes de Integração
```bash
dotnet test InternetBanking.IntegrationTests
```

### Executar Todos os Testes
```bash
dotnet test
```

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture:

- **Domain**: Entidades, enums e interfaces
- **Application**: Serviços, DTOs e validadores
- **Infrastructure**: Repositórios, banco de dados e configurações
- **API**: Controllers e configuração da aplicação

## 🔧 Configuração

### Banco de Dados
- **Produção**: SQL Server
- **Desenvolvimento**: SQL Server
- **Testes**: SQLite em memória

### Connection Strings
Configuradas em `appsettings.json` e `appsettings.Development.json`

## 📊 Cobertura de Testes
- Testes unitários para entidades, serviços e validadores
- Testes de integração para endpoints da API
- Mocks para isolamento de dependências

## 🛡️ Segurança
- Validação rigorosa de entrada
- Tratamento de exceções
- Logs de auditoria para ações importantes
- Transações atômicas para operações críticas

## 📝 Logs de Auditoria
Todas as ações importantes são registradas:
- Criação de contas
- Desativação de contas
- Transferências realizadas

## 🔍 Validações
- Nome do cliente obrigatório (máx. 200 caracteres)
- Documento obrigatório, único e apenas números
- Valores de transferência positivos
- Contas devem estar ativas para operações