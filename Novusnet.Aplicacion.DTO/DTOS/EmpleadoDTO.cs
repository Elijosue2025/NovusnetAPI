using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class EmpleadoDTO
    {
        public int IdEmpleado { get; set; }
        public string Roll { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Propiedades adicionales para consultas específicas

        public int CantidadEmpleados { get; set; }
        public List<string> EmpleadosDelRoll { get; set; }

    }
}
