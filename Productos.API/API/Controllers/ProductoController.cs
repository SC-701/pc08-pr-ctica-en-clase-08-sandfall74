using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using DA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/producto")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase,IProductoController
    {
        private IProductoFlujo _productoFlujo;
        private ILogger<ProductoController> _logger;
        private readonly ITipoCambioServicio _tipoCambioServicio;

        public ProductoController(IProductoFlujo productoFlujo, ILogger<ProductoController> logger, ITipoCambioServicio tipoCambioServicio)
        {
            _productoFlujo = productoFlujo;
            _logger = logger;
            _tipoCambioServicio = tipoCambioServicio;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Agregar(ProductoRequest producto)
        {
            var resultado = await _productoFlujo.Agregar(producto);
            return CreatedAtAction("ObtenerPorId",
                                   new { Id = resultado },
                                   resultado);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Editar(Guid Id, ProductoRequest producto)
        {
            var resultado = await _productoFlujo.Editar(Id, producto);
            return Ok(resultado);

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Eliminar(Guid Id)
        {
            var resultado = await _productoFlujo.Eliminar(Id);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _productoFlujo.Obtener();
            var tipoCambioHoy = await _tipoCambioServicio.ObtenerTipoCambioHoyAsync();
            if (!resultado.Any())
            {
                return NoContent();
            }
            foreach (var producto in resultado)
            {
                producto.Precio = Math.Round(producto.Precio / tipoCambioHoy, 2);
            }
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> ObtenerPorId(Guid Id)
        {
            var tipoCambioHoy = await _tipoCambioServicio.ObtenerTipoCambioHoyAsync();
            var resultado = await _productoFlujo.ObtenerPorId(Id);
            resultado.Precio = Math.Round(resultado.Precio / tipoCambioHoy, 2);
            if (resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }
    }
}
