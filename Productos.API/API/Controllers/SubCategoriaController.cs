using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/SubCategoria")]
    [ApiController]
    [Authorize]
    public class SubCategoriaController : ControllerBase, ISubCategoriaController
    {
        private readonly ISubCategoriaFlujo _subCategoria;

        public SubCategoriaController(ISubCategoriaFlujo subCategoria)
        {
            _subCategoria = subCategoria;
        }

        [HttpGet("{categoriaID}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Obtener(Guid CategoriaID)
        {
            var resultado = await _subCategoria.ObtenerSubCategoria(CategoriaID);
            if (!resultado.Any())
            {
                return NoContent();
            }
            return Ok(resultado);
        }
    }
}
