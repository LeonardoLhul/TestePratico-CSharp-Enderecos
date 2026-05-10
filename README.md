# Teste Prático - Desenvolvedor C#

Aplicação web desenvolvida em ASP.NET Core MVC para autenticação de usuários e gerenciamento de endereços.

O sistema permite que o usuário realize login, cadastre endereços manualmente ou por meio de busca automática via CEP utilizando a API do ViaCEP, além de editar, excluir, listar e exportar seus endereços para um arquivo CSV.

---

# Funcionalidades

- Autenticação de usuário
- Validação de credenciais
- Redirecionamento após login
- Listagem de endereços por usuário
- Cadastro manual de endereços
- Busca automática de endereço por CEP via ViaCEP
- Edição de endereços
- Exclusão de endereços
- Exportação de endereços para CSV

---

# Tecnologias utilizadas

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML
- CSS
- JavaScript

---

# Estrutura do projeto

```txt
TestePratico-CSharp-Enderecos
│
├── TesteEndereco.Web          # Projeto principal ASP.NET Core MVC
│
├── scripts
│   ├── create_tables.sql      # Script de criação das tabelas
│   └── seed_data.sql          # Script opcional de carga inicial
│
└── README.md
```

---

# Requisitos

Antes de executar o projeto, certifique-se de possuir instalado:

- .NET SDK
- SQL Server ou SQL Server LocalDB
- Visual Studio 2022 ou superior

---

# Configuração do banco de dados

A aplicação utiliza SQL Server com conexão configurada no arquivo `appsettings.json`.

Exemplo de connection string utilizando LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TesteEnderecoDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

---

# Como executar o projeto

## 1. Clonar o repositório
HTTPS
```powershell
git clone https://github.com/LeonardoLhul/TestePratico-CSharp-Enderecos
```
SSH
```powershell
git clone git@github.com:LeonardoLhul/TestePratico-CSharp-Enderecos.git
```
---

## 2. Acessar a pasta do projeto

```powershell
cd .\TestePratico-CSharp-Enderecos
```

---

## 3. Criar o banco de dados

Executar:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB"
```

Após abrir o prompt do SQL Server, executar:

```sql
CREATE DATABASE TesteEnderecoDb;
GO
```

Para sair do prompt:

```sql
EXIT
```

---

## 4. Executar o script de criação das tabelas

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "TesteEnderecoDb" -i ".\scripts\create_tables.sql"
```

---

## 5. (Opcional) Executar o script de carga inicial

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "TesteEnderecoDb" -i ".\scripts\seed_data.sql"
```

---

## 6. Configurar a connection string

Abrir o arquivo:

```txt
TesteEndereco.Web/appsettings.json
```

Verificar se a connection string está configurada corretamente:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TesteEnderecoDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

---

## 7. Acessar a pasta do projeto web

```powershell
cd .\TesteEndereco.Web
```

---

## 8. Restaurar dependências e compilar o projeto

```powershell
dotnet build
```

---

## 9. Executar a aplicação

```powershell
dotnet run
```

---

## 10. Acessar a aplicação

Após iniciar a aplicação, acessar a URL exibida no terminal, por exemplo:

```txt
https://localhost:xxxx
```

---

# Usuários de teste

Caso utilize o script de carga inicial (`seed_data.sql`), você pode acessar utilizando os usuários abaixo:

## Administrador

- Usuário: `admin`
- Senha: `Admin@2026`

## Maria

- Usuário: `maria`
- Senha: `Maria@2026`

## João

- Usuário: `joao`
- Senha: `Joao@2026`

---

# Estrutura do banco de dados

## Tabela `Usuarios`

| Campo | Descrição |
|---|---|
| Id | Identificador do usuário |
| Nome | Nome do usuário |
| UserName | Nome de login |
| SenhaHash | Senha do usuário |

---

## Tabela `Enderecos`

| Campo | Descrição |
|---|---|
| Id | Identificador do endereço |
| Cep | CEP |
| Logradouro | Rua/Avenida |
| Complemento | Complemento |
| Bairro | Bairro |
| Cidade | Cidade |
| Uf | Estado |
| Numero | Número |
| UsuarioId | Relacionamento com usuário |

---

# Integração com ViaCEP

A busca automática de endereço é realizada por integração com a API pública do ViaCEP.

[https://viacep.com.br/](https://viacep.com.br/)

---

# Exportação CSV

O sistema permite exportar os endereços do usuário autenticado para um arquivo `.csv`.

---

# Observações

- Cada usuário visualiza apenas os seus próprios endereços
- A aplicação utiliza Entity Framework Core para acesso e manipulação dos dados
- A estrutura inicial do banco foi disponibilizada por scripts SQL, conforme solicitado no teste
- O cadastro de usuário e o logout foram implementados como melhoria adicional ao escopo solicitado

---

# Autor

Leonardo Lhul Aguiar
