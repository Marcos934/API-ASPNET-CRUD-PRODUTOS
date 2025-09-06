// Importa os namespaces necessários para configurar e executar a aplicação.
using Microsoft.EntityFrameworkCore; // Necessário para usar o método UseSqlite e outras funcionalidades do EF Core.
using ProductApi.Data; // Namespace do nosso DbContext para que possamos registrá-lo.

// Cria uma instância do WebApplicationBuilder. O builder é usado para configurar a aplicação.
var builder = WebApplication.CreateBuilder(args);

// --- Início da Seção de Configuração de Serviços ---
// A coleção 'builder.Services' é onde registramos os serviços para injeção de dependência (DI).

// Adiciona o serviço do DbContext (AppDbContext) ao contêiner de DI.
builder.Services.AddDbContext<AppDbContext>(options =>
    // Configura o DbContext para usar o provedor SQLite.
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    // 'builder.Configuration.GetConnectionString("DefaultConnection")' lê a string de conexão "DefaultConnection"
    // que definimos no arquivo 'appsettings.json'.
);

// Adiciona os serviços dos controllers da API ao contêiner de DI.
// Isso permite que o .NET encontre e use nossas classes de controller para rotear requisições HTTP.
builder.Services.AddControllers();

// Adiciona os serviços do gerador de documentação da API (Swagger/OpenAPI).
// É uma boa prática para documentar e testar APIs.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Fim da Seção de Configuração de Serviços ---

// Constrói a aplicação web com base na configuração definida acima.
var app = builder.Build();

// --- Início da Seção de Configuração do Pipeline de Requisições HTTP ---
// Esta seção define como a aplicação responderá às requisições HTTP (middleware).

// Verifica se o ambiente de desenvolvimento é o atual.
if (app.Environment.IsDevelopment())
{
    // Se for desenvolvimento, habilita o middleware do Swagger.
    app.UseSwagger();
    // Habilita o middleware do Swagger UI, que fornece uma interface gráfica para a documentação da API.
    app.UseSwaggerUI();
}

// Habilita o middleware para redirecionar requisições HTTP para HTTPS.
app.UseHttpsRedirection();

// Habilita o middleware de autorização. (Não estamos usando neste exemplo, mas é uma boa prática).
app.UseAuthorization();

// Mapeia as rotas para os controllers da API.
// Isso conecta as URLs das requisições aos métodos correspondentes nos nossos controllers.
app.MapControllers();

// Executa a aplicação, fazendo-a ouvir por requisições HTTP.
app.Run();