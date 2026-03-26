#region ensamblado Abstracciones, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\HP\source\repos\SC-701\pc06-practica-en-clase-06-sandfall74\Vehiculo.API\Abstracciones\obj\Debug\net8.0\ref\Abstracciones.dll
#endregion
#nullable enable
using Abstracciones.Interfaces.Servicios;
using System.Collections.Generic;


namespace Abstracciones.Modelos.Servicios
{
    public class APIEndPoint
    {
        public APIEndPoint() { }

        public string? UrlBase { get; set; }
        public IEnumerable<Metodo> Metodos { get; set; }
    }
}
