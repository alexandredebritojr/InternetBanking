# 🏦 Sistema de Caixa de Banco

Sistema bancário desenvolvido em .NET 8 seguindo os princípios da Clean Architecture, permitindo o cadastro de contas e transferências entre contas bancárias.

## 🎯 Objetivo do Projeto

Desenvolver a API do Sistema de Caixa de Banco da Vindi, permitindo:
- ✅ Cadastro de contas bancárias
- ✅ Consulta de contas cadastradas
- ✅ Inativação de contas
- ✅ Transferências entre contas

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (incluído no Visual Studio)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) ou [VS Code](https://code.visualstudio.com/)

### 1. Clone o Repositório
```bash
git clone https://github.com/alexandredebritojr/InternetBanking.git
cd InternetBanking
```

### 2. Restaure as Dependências
```bash
dotnet restore
```

### 3. Execute o Projeto
```bash
# Navegue até a pasta da API
cd InternetBanking.Api

# Execute a aplicação
dotnet run
```

### 4. Acesse a Aplicação
- **API**: `https://localhost:7000` ou `http://localhost:5000`
- **Swagger/OpenAPI**: `https://localhost:7000` (documentação interativa)

## 📋 Funcionalidades Implementadas

### 1. Cadastro de Contas Bancárias
- **Endpoint**: `POST /api/accounts`
- **Funcionalidades**:
  - Nome e documento obrigatórios
  - Documento único (não permite duplicatas)
  - Saldo inicial de R$ 1.000,00 (bonificação)
  - Data de abertura registrada automaticamente
  - Status inicial como "Ativa"

### 2. Consulta de Contas
- **Endpoint**: `GET /api/accounts`
- **Funcionalidades**:
  - Listagem de todas as contas
  - Filtros por nome (parcial), documento ou status
  - Retorna: nome, documento, saldo, data de abertura e status

### 3. Inativação de Conta
- **Endpoint**: `PUT /api/accounts/document/{document}/deactivate`
- **Funcionalidades**:
  - Desativa conta por documento
  - Preserva dados históricos
  - Registra log de auditoria

### 4. Transferências entre Contas
- **Endpoint**: `POST /api/transfers`
- **Funcionalidades**:
  - Transferência entre contas ativas
  - Validação de saldo suficiente
  - Transação atômica (débito/crédito)
  - Log de auditoria

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

## 📊 Exemplos de Uso

### 1. Cadastrar uma Conta
```bash
curl -X POST "https://localhost:7000/api/accounts" \
  -H "Content-Type: application/json" \
  -d '{
    "clientName": "João Silva",
    "document": "12345678901"
  }'
```

### 2. Listar Contas
```bash
curl -X GET "https://localhost:7000/api/accounts"
```

### 3. Realizar Transferência
```bash
curl -X POST "https://localhost:7000/api/transfers" \
  -H "Content-Type: application/json" \
  -d '{
    "fromDocument": "12345678901",
    "toDocument": "98765432100",
    "amount": 500.00,
    "description": "Transferência de teste"
  }'
```

### 4. Desativar Conta
```bash
curl -X PUT "https://localhost:7000/api/accounts/document/12345678901/deactivate"
```

## 🏗️ Arquitetura do Projeto

```
InternetBanking/
├── InternetBanking.Api/           # Camada de apresentação (Controllers)
├── InternetBanking.Application/   # Camada de aplicação (Services, DTOs)
├── InternetBanking.Domain/        # Camada de domínio (Entities, Interfaces)
├── InternetBanking.Infrastructure/ # Camada de infraestrutura (Repositories, DB)
├── InternetBanking.UnitTests/     # Testes unitários
└── InternetBanking.IntegrationTests/ # Testes de integração
```

### Tecnologias Utilizadas
- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **SQL Server**: Banco de dados principal
- **FluentValidation**: Validação de dados
- **Swagger/OpenAPI**: Documentação da API
- **xUnit**: Framework de testes
- **FluentAssertions**: Assertions mais legíveis
- **Moq**: Mocking para testes

## 🔧 Configuração do Banco de Dados

### SQL Server LocalDB (Desenvolvimento)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InternetBankingDb_Dev;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
}
```

### SQLite (Alternativa)
Se preferir usar SQLite, a aplicação detectará automaticamente e criará um arquivo `banking.db`.

## 📈 Cobertura de Testes

O projeto inclui:
- **Testes Unitários**: Entidades, serviços, validadores
- **Testes de Integração**: Endpoints da API
- **Mocks**: Para isolamento de dependências

### Executar Testes com Cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 🛡️ Segurança e Validações

### Validações Implementadas
- ✅ Nome do cliente obrigatório (máx. 200 caracteres)
- ✅ Documento obrigatório, único e apenas números
- ✅ Valores de transferência positivos
- ✅ Contas devem estar ativas para operações
- ✅ Saldo suficiente para transferências

### Logs de Auditoria
Todas as ações importantes são registradas:
- Criação de contas
- Desativação de contas
- Transferências realizadas

## 🔍 Troubleshooting

### Problema: Erro de Conexão com Banco
**Solução**: Verifique se o SQL Server LocalDB está instalado e funcionando:
```bash
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### Problema: Porta em Uso
**Solução**: Altere as portas em `launchSettings.json` ou mate o processo:
```bash
# Windows
netstat -ano | findstr :7000
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:7000 | xargs kill -9
```

### Problema: Testes Falhando
**Solução**: Execute os testes individualmente:
```bash
dotnet test InternetBanking.UnitTests --verbosity normal
dotnet test InternetBanking.IntegrationTests --verbosity normal
```

## 📚 Documentação Adicional

- [Documentação da API](./InternetBanking.Api/README.md)
- [Swagger/OpenAPI](https://localhost:7000) (quando a aplicação estiver rodando)

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

**Desenvolvido para o Desafio Técnico da Vindi** 🚀
