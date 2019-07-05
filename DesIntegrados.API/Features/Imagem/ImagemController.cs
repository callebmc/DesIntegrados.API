using DesIntegrados.Models;
using DesIntegrados.Persistence;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesIntegrados.API.Features
{
    [Route("imagem")]
    [ApiController]
    [EnableCors]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ImagemController : ControllerBase
    {
        private readonly DesIntegradosContext context;

        public ImagemController(DesIntegradosContext context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Imagem>>> GetImagem()
        {
            List<Imagem> imagem;

            try
            {
                imagem = await context.Imagens.ToListAsync();
                if (imagem == null)
                    return new NotFoundResult();
                return new OkObjectResult(imagem);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
