using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoResponse productoResponse { get; set; } = default!;

        [BindProperty]
        public ProductoRequest productoRequest { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaSeleccionada { get; set; }

        [BindProperty]
        public Guid subCategoriaSeleccionada { get; set; }

        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                await ObtenerCategorias();
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                productoResponse = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones);

                if (productoResponse != null)
                {
                    categoriaSeleccionada = Guid.Parse(
                        categorias.Where(c => c.Text == productoResponse.Categoria).First().Value
                    );

                    subCategorias = (await ObtenerSubCategoriasAsync(categoriaSeleccionada)).Select(a =>
                        new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Nombre,
                            Selected = a.Nombre == productoResponse.SubCategoria
                        }).ToList();

                    subCategoriaSeleccionada = Guid.Parse(
                        subCategorias.Where(s => s.Text == productoResponse.SubCategoria).First().Value
                    );
                }
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (productoResponse.Id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");

            var cliente = new HttpClient();

            var respuesta = await cliente.PutAsJsonAsync<ProductoRequest>(
                string.Format(endpoint, productoResponse.Id.ToString()),
                new ProductoRequest
                {
                    IdSubCategoria = subCategoriaSeleccionada,
                    Nombre = productoResponse.Nombre,
                    Descripcion = productoResponse.Descripcion,
                    Precio = productoResponse.Precio,
                    Stock = productoResponse.Stock,
                    CodigoBarras = productoResponse.CodigoBarras
                });

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

            categorias = resultadoDeserializado.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre
                }).ToList();
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaID)
        {
            var subCategorias = await ObtenerSubCategoriasAsync(categoriaID);
            return new JsonResult(subCategorias);
        }

        private async Task<List<SubCategoria>> ObtenerSubCategoriasAsync(Guid categoriaID)
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
    }
}
