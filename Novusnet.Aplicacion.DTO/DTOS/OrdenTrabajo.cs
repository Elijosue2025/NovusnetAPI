using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class OrdenTrabajo
    {
        /// <summary>
        /// Servicio
        /// </summary>
        public int pk_servicio { get; set; }

        public string ser_nombre { get; set; }

        public decimal? ser_precio { get; set; }

        public string ser_descripcion { get; set; }

        public int? ser_requiere_material { get; set; }

        public string ser_tipo_factura { get; set; }

        public int fk_cliente { get; set; }
        //
        //Cliente
        //
        public int pk_cliente { get; set; }

        public string cli_cedula { get; set; }

        public string cli_nombre { get; set; }

        public string cli_apellido { get; set; }

      /// <summary>
      /// orden trabajo
      /// </summary>



        public int pk_orden_trabajo { get; set; }

        public string otra_codigo { get; set; }

        public string otra_descripcion { get; set; }

        public DateTime? otra_fecha_registro { get; set; }

        public DateTime? otra_fecha_programada { get; set; }

        public string otra_tiempo_estimado { get; set; }

        public string otra_estado { get; set; }

        public string otra_prioridad { get; set; }

        public int fk_Empleado { get; set; }

        public int fk_servicio { get; set; }

        //// Empleado
        ///
        public int pk_Empleado { get; set; }

        public string emp_roll { get; set; }

        public string emp_nombre { get; set; }

        public string emp_apellido { get; set; }

        public string emp_cedula { get; set; }
    }
}
