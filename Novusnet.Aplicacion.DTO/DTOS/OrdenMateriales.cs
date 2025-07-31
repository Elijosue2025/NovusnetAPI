using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class OrdenMateriales

    {
        /// <summary>
        /// Empleado
        /// </summary>
        public int pk_Empleado { get; set; }

        public string emp_roll { get; set; }

        public string emp_nombre { get; set; }

        public string emp_apellido { get; set; }

        public string emp_cedula { get; set; }
        /// <summary>
        /// Orden material
        /// </summary>

        public int pk_orden_material { get; set; }

        public string orma_codigo { get; set; }

        public int? orma_cantidad { get; set; }

        public string orma_estado { get; set; }

        public string orma_observaciones { get; set; }

        public DateTime? orma_fecha_uso { get; set; }

        public int fk_material { get; set; }
        ////
        ///Servicio

    }
}
