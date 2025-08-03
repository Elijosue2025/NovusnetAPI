using System;
using System.Collections.Generic;

namespace Novusnet.Aplicacion.DTO.DTOS
{
    public class OrdenMaterialDTO
    {
        // Orden Trabajo
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
        public int fk_cliente { get; set; }

        // Cliente
        public int pk_cliente { get; set; }
        public string cli_cedula { get; set; }
        public string cli_nombre { get; set; }
        public string cli_apellido { get; set; }

        // Empleado
        public int pk_empleado { get; set; }
        public string emp_cedula { get; set; }
        public string emp_nombre { get; set; }
        public string emp_apellido { get; set; }
        public string emp_roll { get; set; }

        // Servicio
        public int pk_servicio { get; set; }
        public string ser_codigo { get; set; }

        public string ser_nombre { get; set; }
        public decimal? ser_precio { get; set; }
        public string ser_descripcion { get; set; }
        public int? ser_requiere_material { get; set; }
        public string ser_tipo_factura { get; set; }

        // Materiales utilizados

        public int pk_orden_material { get; set; }
        public int fk_material { get; set; }
        public int fk_orden_trabajo { get; set; }
        public int orma_cantidad { get; set; }
        public string orma_estado { get; set; }

        // Datos del material
        public int pk_material { get; set; }
        public string mat_nombre { get; set; }
        public decimal mat_precio_unitario { get; set; }

    }
}
