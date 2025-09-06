# API de Gerenciamento de Produtos

Esta é uma API RESTful simples construída com .NET 8 e Entity Framework Core, usando um banco de dados SQLite. Ela fornece operações CRUD (Create, Read, Update, Delete) para gerenciar uma lista de produtos.

## Tecnologias Utilizadas

*   **.NET 8**: Plataforma de desenvolvimento.
*   **ASP.NET Core**: Framework para construir APIs web.
*   **Entity Framework Core**: ORM para interação com o banco de dados.
*   **SQLite**: Banco de dados relacional baseado em arquivo.
*   **Swagger (OpenAPI)**: Para documentação e teste interativo da API.

## Estrutura do Projeto

*   `/Models/Product.cs`: Define o modelo de dados do produto.
*   `/Data/AppDbContext.cs`: Configura o contexto do banco de dados e a tabela de produtos.
*   `/Controllers/ProductsController.cs`: Contém os endpoints da API para as operações CRUD.
*   `appsettings.json`: Armazena a string de conexão do banco de dados.
*   `Program.cs`: Ponto de entrada da aplicação, onde os serviços e o pipeline de requisições são configurados.
*   `products.db`: O arquivo do banco de dados SQLite (criado após a primeira execução da migração).

## Endpoints da API

A URL base para esta API é `api/products`.

| Método HTTP | URL                     | Descrição                                   |
|-------------|-------------------------|---------------------------------------------|
| `GET`       | `/api/products`         | Retorna uma lista de todos os produtos.     |
| `GET`       | `/api/products/{id}`    | Retorna um produto específico pelo seu ID.  |
| `POST`      | `/api/products`         | Cria um novo produto.                       |
| `PUT`       | `/api/products/{id}`    | Atualiza um produto existente.              |
| `DELETE`    | `/api/products/{id}`    | Deleta um produto específico pelo seu ID.   |

### Exemplo de Corpo de Requisição (POST / PUT)

```json
{
  "name": "Notebook Gamer",
  "price": 7500.50,
  "description": "Notebook com placa de vídeo dedicada e 16GB de RAM."
}
```

## Como Executar o Projeto

1.  **Clone o repositório** (ou use os arquivos criados).
2.  **Navegue até o diretório raiz do projeto** (`ProductApi`).
3.  **Execute o comando:** `dotnet run`
4.  A API estará em execução. Você pode acessar a interface do Swagger (que permite testar os endpoints visualmente) em um endereço como `https://localhost:7123/swagger` (a porta pode variar).
