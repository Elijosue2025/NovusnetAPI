using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class SServicoDTO
    {
        public int pk_servicio { get; set; }

        public string ser_nombre { get; set; }

        public decimal? ser_precio { get; set; }

        public string ser_descripcion { get; set; }

        public int? ser_requiere_material { get; set; }

        public string ser_tipo_factura { get; set; }

        public int fk_cliente { get; set; }

        public int ClienteId { get; set; }
        public string CedulaCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string TelefonoCliente { get; set; }

    }
}
