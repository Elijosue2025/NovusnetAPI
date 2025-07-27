using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class LoggingDTO
    {
        public int pk_logging { get; set; }

        public string log_user { get; set; }

        public string log_password { get; set; }

        public int fk_Empleado { get; set; }

    }
}
