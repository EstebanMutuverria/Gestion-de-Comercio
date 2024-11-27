using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public bool Estado {  get; set; }
        public Proveedor Proveedor { get; set; }
        public DetalleCompra DetalleCompra { get; set; }

    }
}
