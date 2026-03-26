using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class TipoCambioReglas
    {
        public static void ValidarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha inicio no puede ser mayor que la fecha fin.");

            if (fechaFin > DateTime.Now)
                throw new ArgumentException("La fecha fin no puede ser mayor a la fecha actual.");
        }

        public static decimal ConvertirADolares(decimal precioColones, decimal tipoCambio)
        {
            if (tipoCambio <= 0)
                throw new ArgumentException("El tipo de cambio debe ser mayor que cero.");

            return Math.Round(precioColones / tipoCambio, 2);
        }
    }
}
