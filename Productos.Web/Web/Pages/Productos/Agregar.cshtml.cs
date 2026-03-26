using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private IConfiguracion _configuracion;

        [BindProperty]
        public ProductoRequest producto { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaSeleccionada { get; set; }

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<PageResult> OnGet()
        {
            await ObtenerCategorias();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await ObtenerCategorias();
                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProducto");
            var cliente = new HttpClient();
            var respuesta = await cliente.PostAsJsonAsync(endpoint, producto);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        private async Task ObtenerCategorias()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(endpoint);
            respuesta.EnsureSuccessStatusCode();
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadoDeserializado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);

            categorias = resultadoDeserializado.Select(categoria => new SelectListItem
            {
                Value = categoria.Id.ToString(),
                Text = categoria.Nombre
            }).ToList();
        }

        private async Task<List<SubCategoria>?> ObtenerSubCategorias(Guid categoriaID)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");
            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(string.Format(endpoint, categoriaID));
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SubCategoria>>(resultado, opciones);
            }

            return new List<SubCategoria>();
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaID)
        {
            var subCategorias = await ObtenerSubCategorias(categoriaID);
            return new JsonResult(subCategorias);
        }
    }
}