using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using Microsoft.Extensions.Configuration;
using Reglas;
using System.Net.Http.Json;

namespace Servicios
{
    public class TipoCambioServicio:ITipoCambioServicio
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguracion _iConfiguracion;

        public TipoCambioServicio(IHttpClientFactory httpClientFactory, IConfiguracion iConfiguracion)
        {
            _httpClientFactory = httpClientFactory;
            _iConfiguracion = iConfiguracion;
        }

        public async Task<List<TipoCambio>> ObtenerTipoCambioAsync(DateTime fechaInicio, DateTime fechaFin)
        {
         
            

            TipoCambioReglas.ValidarFechas(fechaInicio, fechaFin);
            

            var metodo = _iConfiguracion.ObtenerMetodo("ApiEndPointsRevision", "ObtenerRevision");

            string url = string.Format(
            metodo,
            fechaInicio.ToString("yyyy/MM/dd"),
            fechaFin.ToString("yyyy/MM/dd")
            );
            var cliente = _httpClientFactory.CreateClient("ServicioRevision");
            var token = _iConfiguracion.ObtenerValor("ApiEndPointsRevision:Token");
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await cliente.GetAsync(url);
            respuesta.EnsureSuccessStatusCode();

            var apiResponse = await respuesta.Content.ReadFromJsonAsync<ApiResponse>();

            if (apiResponse == null || !apiResponse.Estado)
                throw new Exception("Error al consultar el tipo de cambio.");

            var resultado = apiResponse.Datos
            .SelectMany(d => d.Indicadores)
            .SelectMany(i => i.Series)
            .Select(s => new TipoCambio
            {
                Fecha = s.Fecha,
                Valor = s.ValorDatoPorPeriodo
            })
            .ToList();

            return resultado;

        }

        public async Task<decimal> ObtenerTipoCambioHoyAsync()
        {
            var hoy = DateTime.Today;

            var lista = await ObtenerTipoCambioAsync(hoy, hoy);

            var tipoCambioHoy = lista.FirstOrDefault();

            if (tipoCambioHoy == null)
                throw new Exception("No se pudo obtener el tipo de cambio del día.");

            return tipoCambioHoy.Valor;
        }
    }
}
