using DesIntegrados.Models;
using DesIntegrados.Persistence;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesIntegrados.API.Features;

namespace DesIntegrados.API.Features
{
    [Route("usuario")]
    [ApiController]
    [EnableCors]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsuarioController : ControllerBase
    {
        private readonly DesIntegradosContext context;

        public UsuarioController(DesIntegradosContext context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            List<Usuario> usuario;

            try
            {
                usuario = await context.Usuarios.ToListAsync();
                if (usuario == null)
                    return new NotFoundResult();
                return new OkObjectResult(usuario);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            Usuario resultado;

            try
            {
                resultado = await context.Usuarios.FindAsync(id);
                if (resultado == null)
                    return new NotFoundResult();

                return new OkObjectResult(resultado);
            }
            catch (Exception)
            {
                // Realizar log do erro
                // Ver qual o internal server error result e retornar
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Usuario>> CreateUsuario([FromBody] Usuario novoUsuario)
        {
            Usuario usuario;

            try
            {
                if (novoUsuario.Nome.Length >= 255)
                    return new BadRequestResult();

                usuario = new Usuario()
                {
                    Nome = novoUsuario.Nome
                };

                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();

                return CreatedAtRoute("GetUsuario", new { id = usuario.Id }, usuario);
            }
            catch (InvalidCastException)
            {
                return new BadRequestResult();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteUsuario([FromRoute] int id)
        {
            Usuario usuario;
            try
            {
                usuario = await context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return new NotFoundResult();

                context.Usuarios.Remove(usuario);
                await context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception)
            {
                // Realizar log do erro
                // Ver qual o internal server error result e retornar
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUsuario([FromRoute]int id, [FromBody] Usuario novoUsuario)
        {
            Usuario usuario;
            try
            {
                if (novoUsuario.Nome.Length >= 255)
                    return new BadRequestResult();

                usuario = await context.Usuarios.FindAsync(id);

                if (usuario == null)
                    return new NotFoundResult();

                usuario.Nome = novoUsuario.Nome;

                await context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception)
            {
                // Realizar log do erro
                // Ver qual o internal server error result e retornar
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{email}/{senha}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChecaSenha([FromBody] string email, string senha)
        {
            Usuario usuario;
            try
            {
                usuario = await context.Usuarios.FindAsync(JsonConvert.DeserializeObject(email));
                if (usuario == null)
                    return new NotFoundResult();

                if (usuario.Senha == JsonConvert.DeserializeObject(senha).ToString())
                    return new OkObjectResult(1);
                else
                    return new BadRequestResult();
            }
            catch (Exception)
            {
                // Realizar log do erro
                // Ver qual o internal server error result e retornar
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{email}/{ganho}/{arquivo}/{senha}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CalculaImagem([FromRoute] string email, float ganho, string arquivo, string senha)
        {
            Usuario usuario;
            try
            {
                /*usuario = await context.Usuarios.FindAsync((email));
                if (usuario == null)
                    return new NotFoundResult();
                if (usuario.Senha == senha)
                    return new BadRequestResult();*/

                Features.Reconstrucao.IniciaReconstrucao(ganho, arquivo);
                return new OkResult();
            }
            catch (Exception)
            {
                // Realizar log do erro
                // Ver qual o internal server error result e retornar
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
