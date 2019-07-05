using DesIntegrados.Persistence;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DesIntegrados.Models;
using Bogus;

namespace DesIntegrados.API.Extensions
{
    public static class DesIntegradosContextExtension
    {
        public static async Task SeedData(this DesIntegradosContext context, int qtdeRegistros = 10, CancellationToken ct = default)
        {
            if (context.Usuarios.Any())
                return;     // Caso já existam registros, não gerar novos

            var usuarioFake = new Faker<Usuario>()
                .RuleFor(p => p.Nome, f => f.Name.FullName())

                .RuleFor(p => p.Email, (f, p) => f.Internet.Email(firstName: p.Nome))
                .RuleFor(p => p.Senha, f => f.Internet.Password(10))
                .RuleFor(p => p.dataFim, f => f.Date.Past(25))
                .RuleFor(p => p.dataFim, f => f.Date.Past(25))
                .RuleFor(p => p.iteracoes, f => f.Random.Int())
                .RuleFor(p => p.tamanho, f => f.Random.Int().ToString())
                .Generate(qtdeRegistros);

            context.Usuarios.AddRange(usuarioFake);
            await context.SaveChangesAsync(ct);
        }
    }
}
