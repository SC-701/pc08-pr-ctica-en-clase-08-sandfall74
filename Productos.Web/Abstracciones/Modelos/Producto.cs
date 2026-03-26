using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    
        public class ProductoBase
        {
            
            [Required(ErrorMessage = "La propiedad nombre es requerida")]
            [StringLength(100,ErrorMessage ="El producto no puede tener un nombre mayor a 100", MinimumLength =5)]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La propiedad descripcion es requerida")]
            [StringLength(100, ErrorMessage = "El producto no puede tener un nombre mayor a 100", MinimumLength = 5)]
            public string Descripcion { get; set; }


            [Required(ErrorMessage = "La propiedad Precio es requerido")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
            public decimal Precio { get; set; }

            [Required(ErrorMessage = "La propiedad Stock es requerido")]
            [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
            public int Stock { get; set; }

            [Required(ErrorMessage = "La propiedad Codigo de Barras es requerido")]
            [StringLength(13, ErrorMessage = "El codigo de barras debe tener 13 caracteres", MinimumLength = 13)]
            public string CodigoBarras { get; set; } 
        }

        public class ProductoRequest : ProductoBase
        {
            public Guid IdSubCategoria { get; set; }
        }

        public class ProductoResponse : ProductoBase
        {
            public Guid Id { get; set; }
            public string SubCategoria { get; set; }
            public string Categoria { get; set; }
        }
    
}
