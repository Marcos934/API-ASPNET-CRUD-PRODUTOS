// Importa os namespaces necessários.
using Microsoft.AspNetCore.Mvc; // Fornece as classes para criar APIs web, como [ApiController], [Route], etc.
using Microsoft.EntityFrameworkCore; // Necessário para usar ToListAsync, FindAsync, etc.
using ProductApi.Data; // Onde nosso AppDbContext está localizado.
using ProductApi.Models; // Onde nosso modelo Product está localizado.

// Define o namespace para os controllers.
namespace ProductApi.Controllers
{
    // [ApiController] é um atributo que habilita comportamentos específicos para controllers de API, como validação automática do corpo da requisição.
    [ApiController]
    // [Route("api/[controller]")] define o padrão da URL para este controller.
    // "api/[controller]" será resolvido para "api/Products", onde "[controller]" é substituído pelo nome da classe do controller sem o sufixo "Controller".
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Declara um campo privado e somente leitura para armazenar a instância do nosso DbContext.
        // 'readonly' significa que o valor só pode ser atribuído no construtor.
        private readonly AppDbContext _context;

        // Este é o construtor do controller.
        // O .NET usará injeção de dependência para passar uma instância de AppDbContext aqui.
        // Nós armazenamos essa instância no campo _context para usá-la nos métodos do controller.
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODO READ (TODOS) ---
        // [HttpGet] é um atributo que marca este método para responder a requisições HTTP GET.
        // A URL será a base do controller: GET /api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Usa o _context para acessar a tabela 'Products'.
            // 'ToListAsync()' é um método do Entity Framework Core que consulta todos os registros da tabela de forma assíncrona.
            // 'async' e 'await' são usados para não bloquear a thread principal enquanto a consulta ao banco de dados está em andamento.
            var products = await _context.Products.ToListAsync();
            // Retorna um código de status 200 OK com a lista de produtos no corpo da resposta.
            return Ok(products);
        }

        // --- MÉTODO READ (POR ID) ---
        // [HttpGet("{id}")] marca este método para responder a requisições HTTP GET com um parâmetro de rota 'id'.
        // A URL será, por exemplo: GET /api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // 'FindAsync(id)' é um método otimizado do EF Core para encontrar uma entidade pela sua chave primária.
            // A consulta é feita de forma assíncrona.
            var product = await _context.Products.FindAsync(id);

            // Verifica se o produto foi encontrado no banco de dados.
            if (product == null)
            {
                // Se não for encontrado, retorna um código de status 404 Not Found.
                return NotFound();
            }

            // Se o produto for encontrado, retorna um código de status 200 OK com o produto no corpo da resposta.
            return Ok(product);
        }

        // --- MÉTODO CREATE ---
        // [HttpPost] marca este método para responder a requisições HTTP POST.
        // A URL será a base do controller: POST /api/products
        // O corpo da requisição deve conter um JSON com os dados do novo produto.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // 'Add(product)' adiciona a nova entidade de produto ao contexto do EF Core.
            // Neste ponto, o produto ainda não foi salvo no banco de dados, está apenas sendo "rastreado" pelo EF Core.
            _context.Products.Add(product);
            // 'SaveChangesAsync()' salva todas as alterações feitas no contexto (neste caso, a adição do novo produto) no banco de dados.
            // A operação é assíncrona.
            await _context.SaveChangesAsync();

            // 'CreatedAtAction' é um método auxiliar que retorna um código de status 201 Created.
            // Este é o status padrão para indicar que um novo recurso foi criado com sucesso.
            // O primeiro argumento é o nome da ação (método) que pode ser usada para buscar o novo recurso (o método GetProduct).
            // O segundo argumento é um objeto com os parâmetros de rota para a ação de busca (o 'id' do novo produto).
            // O terceiro argumento é o próprio objeto criado, que será incluído no corpo da resposta.
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // --- MÉTODO UPDATE ---
        // [HttpPut("{id}")] marca este método para responder a requisições HTTP PUT com um parâmetro de rota 'id'.
        // A URL será, por exemplo: PUT /api/products/5
        // O corpo da requisição deve conter um JSON com os dados atualizados do produto.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            // Verifica se o 'id' passado na URL é o mesmo do 'id' do produto no corpo da requisição.
            // Isso é uma medida de segurança para evitar inconsistências.
            if (id != product.Id)
            {
                // Se os IDs não baterem, retorna um 400 Bad Request, pois a requisição é malformada.
                return BadRequest();
            }

            // Informa ao EF Core que a entidade 'product' que recebemos está em um estado modificado.
            // O EF Core irá então gerar um comando SQL UPDATE para todas as propriedades desta entidade quando SaveChangesAsync for chamado.
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                // Tenta salvar as alterações no banco de dados.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Esta exceção pode ocorrer se, por exemplo, o produto foi deletado por outra requisição
                // enquanto estávamos tentando atualizá-lo.
                if (!ProductExists(id))
                {
                    // Se o produto não existe mais, retornamos 404 Not Found.
                    return NotFound();
                }
                else
                {
                    // Se a exceção foi por outro motivo de concorrência, nós a relançamos.
                    throw;
                }
            }

            // Se a atualização for bem-sucedida, retorna um código de status 204 No Content.
            // Este é o status padrão para uma atualização bem-sucedida, pois não precisamos retornar nenhum conteúdo no corpo da resposta.
            return NoContent();
        }

        // --- MÉTODO DELETE ---
        // [HttpDelete("{id}")] marca este método para responder a requisições HTTP DELETE com um parâmetro de rota 'id'.
        // A URL será, por exemplo: DELETE /api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Procura o produto no banco de dados pela sua chave primária.
            var product = await _context.Products.FindAsync(id);
            // Verifica se o produto foi encontrado.
            if (product == null)
            {
                // Se não for encontrado, retorna 404 Not Found.
                return NotFound();
            }

            // 'Remove(product)' marca a entidade para ser deletada pelo EF Core.
            _context.Products.Remove(product);
            // Salva as alterações no banco de dados, efetivamente deletando o registro.
            await _context.SaveChangesAsync();

            // Retorna 204 No Content para indicar que a operação foi bem-sucedida.
            return NoContent();
        }

        // Método privado auxiliar para verificar se um produto existe.
        private bool ProductExists(int id)
        {
            // 'Any(e => e.Id == id)' verifica se existe algum produto na tabela que satisfaça a condição.
            // É uma forma eficiente de fazer essa checagem sem precisar carregar a entidade inteira.
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
