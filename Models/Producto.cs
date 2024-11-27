using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
        public int StockMinimo { get; set; }
        public int StockActual { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public DateTime? FechaVencimiento { get; set; } // ? = Significa que es nulleable
        public decimal? Precio { get; set; }
    }
}
