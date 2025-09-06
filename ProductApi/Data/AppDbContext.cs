// Importa os namespaces necessários.
using Microsoft.EntityFrameworkCore; // Namespace principal do Entity Framework Core.
using ProductApi.Models; // Namespace onde nossa classe de modelo 'Product' está localizada.

// Define o namespace para as classes de acesso a dados.
namespace ProductApi.Data
{
    // Declara a classe 'AppDbContext', que herda de 'DbContext'.
    // DbContext é a classe base do EF Core que representa uma sessão com o banco de dados.
    public class AppDbContext : DbContext
    {
        // Este é o construtor da classe. Ele aceita um objeto 'DbContextOptions<AppDbContext>' como parâmetro.
        // As opções (options) contêm a configuração para o contexto, como a string de conexão do banco de dados.
        // A palavra-chave 'base(options)' chama o construtor da classe base (DbContext) e passa as opções para ele.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Declara uma propriedade 'DbSet<Product>' chamada 'Products'.
        // Um DbSet representa uma coleção de entidades (neste caso, 'Product') no contexto.
        // O EF Core mapeará esta propriedade para uma tabela chamada 'Products' no banco de dados.
        // O 'get; set;' permite que obtenhamos e definamos esta propriedade.
        public DbSet<Product> Products { get; set; }
    }
}
