using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class ClienteDTO
    {
        public int pk_cliente { get; set; }
        public string cli_cedula { get; set; }
        public string cli_nombre { get; set; }
        public string cli_apellido { get; set; }
        public string cli_telefono { get; set; }
        public string cli_email { get; set; }
        public string cli_direccion { get; set; }
        public string cli_referencia_ubicacion { get; set; }
        public DateTime? cli_fecha_registro { get; set; }
        public int? cli_activo { get; set; }
    }
}
