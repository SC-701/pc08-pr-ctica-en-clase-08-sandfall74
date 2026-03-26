using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Servicios
{
    public class ApiEndPointsRevision
    {
        
            public string UrlBase { get; set; }
        public string Token { get; set; }
        public List<Metodo> Metodos { get; set; }
       

        
    }

    public class Metodo
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
