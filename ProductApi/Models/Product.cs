// Define o namespace para as classes de modelo. Namespaces são usados para organizar o código e evitar conflitos de nomes.
namespace ProductApi.Models
{
    // Declara a classe 'Product'. Esta classe é um modelo de dados (POCO - Plain Old CLR Object).
    // Ela representa a estrutura de um produto no nosso sistema.
    public class Product
    {
        // Declara a propriedade 'Id'. Por convenção do Entity Framework Core, uma propriedade chamada 'Id' ou 'ProductId' será automaticamente a chave primária da tabela.
        // A chave primária é um identificador único para cada registro na tabela do banco de dados.
        public int Id { get; set; }

        // Declara a propriedade 'Name'. O 'string?' indica que a propriedade pode ser nula (nullable).
        // Armazena o nome do produto.
        public string? Name { get; set; }

        // Declara a propriedade 'Price'. 'decimal' é usado para valores monetários para garantir a precisão.
        // Armazena o preço do produto.
        public decimal Price { get; set; }

        // Declara a propriedade 'Description'. Também é uma string que pode ser nula.
        // Armazena a descrição do produto.
        public string? Description { get; set; }
    }
}
