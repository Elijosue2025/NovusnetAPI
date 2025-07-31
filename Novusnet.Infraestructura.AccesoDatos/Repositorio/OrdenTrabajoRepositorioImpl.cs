using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class OrdenTrabajoRepositorioImpl : RepositorioImpl<Orden_Trabajo>, IOrdenTrabajoRepositorio
    {
        private readonly NovusnetPROContext _context;

        public OrdenTrabajoRepositorioImpl(NovusnetPROContext context) : base(context)
        {
            _context = context;
        }

        // ===============================
        // MÉTODOS CRUD BÁSICOS (ACTUALIZADOS)
        // ===============================

        public async Task CrearOrdenTrabajoAsync(Orden_Trabajo orden)
        {
            try
            {
                orden.otra_fecha_registro = DateTime.Now; // Fecha automática
                orden.otra_estado = orden.otra_estado ?? "Pendiente"; // Estado por defecto
                await _context.Orden_Trabajo.AddAsync(orden);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear orden de trabajo", ex);
            }
        }

        public async Task<List<Orden_Trabajo>> ObtenerTodasOrdenesAsync()
        {
            try
            {
                return await _context.Orden_Trabajo
                    .Where(ot => ot.otra_estado != "Cancelado") // Excluir canceladas
                    .OrderByDescending(ot => ot.otra_fecha_registro)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener órdenes", ex);
            }
        }

        public async Task<Orden_Trabajo> ObtenerOrdenPorIdAsync(int id)
        {
            try
            {
                return await _context.Orden_Trabajo.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener orden ID {id}", ex);
            }
        }

        public async Task ActualizarOrdenTrabajoAsync(Orden_Trabajo orden)
        {
            try
            {
                var existente = await _context.Orden_Trabajo.FindAsync(orden.pk_orden_trabajo);
                if (existente == null) throw new Exception("Orden no encontrada");

                // Campos actualizables (excepto fecha_registro)
                existente.otra_codigo = orden.otra_codigo;
                existente.otra_descripcion = orden.otra_descripcion;
                existente.otra_fecha_programada = orden.otra_fecha_programada;
                existente.otra_tiempo_estimado = orden.otra_tiempo_estimado;
                existente.otra_estado = orden.otra_estado;
                existente.otra_prioridad = orden.otra_prioridad;
                existente.fk_Empleado = orden.fk_Empleado;
                existente.fk_servicio = orden.fk_servicio;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar orden", ex);
            }
        }

        // MÉTODO DELETE MODIFICADO: CANCELAR EN LUGAR DE ELIMINAR
        public async Task EliminarOrdenTrabajoAsync(int id)
        {
            try
            {
                var orden = await _context.Orden_Trabajo.FindAsync(id);
                if (orden != null)
                {
                    // Cambiar estado a "Cancelado" en lugar de eliminar físicamente
                    orden.otra_estado = "Cancelado";
                    _context.Orden_Trabajo.Update(orden);
                    await _context.SaveChangesAsync();
                }
                else throw new Exception("Orden no encontrada");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar orden", ex);
            }
        }

        // ===============================
        // MÉTODO PRINCIPAL: LISTAR CON DATOS COMPLETOS (DTO)
        // ===============================

        public async Task<List<OrdenTrabajo>> ObtenerOrdenesTrabajoCompletasAsync()
        {
            try
            {
                return await _context.Orden_Trabajo
                    .Include(ot => ot.fk_servicioNavigation)
                        .ThenInclude(s => s.fk_clienteNavigation)
                    .Include(ot => ot.fk_EmpleadoNavigation)
                    .Where(ot => ot.otra_estado != "Cancelado") // Excluir canceladas
                    .Select(ot => new OrdenTrabajo
                    {
                        // Datos del servicio
                        pk_servicio = ot.fk_servicioNavigation.pk_servicio,
                        ser_nombre = ot.fk_servicioNavigation.ser_nombre,
                        ser_precio = ot.fk_servicioNavigation.ser_precio,
                        ser_descripcion = ot.fk_servicioNavigation.ser_descripcion,
                        ser_requiere_material = ot.fk_servicioNavigation.ser_requiere_material,
                        ser_tipo_factura = ot.fk_servicioNavigation.ser_tipo_factura,
                        fk_cliente = ot.fk_servicioNavigation.fk_cliente,

                        // Datos del cliente
                        pk_cliente = ot.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                        cli_cedula = ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula,
                        cli_nombre = ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre,
                        cli_apellido = ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido,

                        // Datos de la orden de trabajo
                        pk_orden_trabajo = ot.pk_orden_trabajo,
                        otra_codigo = ot.otra_codigo,
                        otra_descripcion = ot.otra_descripcion,
                        otra_fecha_registro = ot.otra_fecha_registro,
                        otra_fecha_programada = ot.otra_fecha_programada,
                        otra_tiempo_estimado = ot.otra_tiempo_estimado,
                        otra_estado = ot.otra_estado,
                        otra_prioridad = ot.otra_prioridad,
                        fk_Empleado = ot.fk_Empleado,
                        fk_servicio = ot.fk_servicio,

                        // Datos del empleado
                        pk_Empleado = ot.fk_EmpleadoNavigation.pk_Empleado,
                        emp_roll = ot.fk_EmpleadoNavigation.emp_roll,
                        emp_nombre = ot.fk_EmpleadoNavigation.emp_nombre,
                        emp_apellido = ot.fk_EmpleadoNavigation.emp_apellido,
                        emp_cedula = ot.fk_EmpleadoNavigation.emp_cedula
                    })
                    .OrderByDescending(ot => ot.otra_fecha_registro)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener órdenes de trabajo completas", ex);
            }
        }

        // ===============================
        // MÉTODO DE BÚSQUEDA POR CRITERIOS MÚLTIPLES
        // ===============================

        public async Task<List<OrdenTrabajo>> BuscarOrdenesTrabajosPorCriterioAsync(string criterio, string busqueda)
        {
            try
            {
                var query = _context.Orden_Trabajo
                    .Include(ot => ot.fk_servicioNavigation)
                        .ThenInclude(s => s.fk_clienteNavigation)
                    .Include(ot => ot.fk_EmpleadoNavigation)
                    .Where(ot => ot.otra_estado != "Cancelado") // Excluir canceladas
                    .AsQueryable();

                string criterioLower = criterio?.ToLower();
                string busquedaLower = busqueda?.ToLower();

                switch (criterioLower)
                {
                    // Búsqueda por datos del cliente
                    case "cedula":
                    case "cedulacliente":
                    case "clientecedula":
                        query = query.Where(ot => ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula.ToLower().Contains(busquedaLower));
                        break;

                    case "nombrecliente":
                    case "clientenombre":
                        query = query.Where(ot => ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre.ToLower().Contains(busquedaLower));
                        break;

                    case "apellidocliente":
                    case "clienteapellido":
                        query = query.Where(ot => ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido.ToLower().Contains(busquedaLower));
                        break;

                    // Búsqueda por datos del empleado
                    case "nombre":
                    case "nombreempleado":
                    case "empleadonombre":
                        query = query.Where(ot => ot.fk_EmpleadoNavigation.emp_nombre.ToLower().Contains(busquedaLower));
                        break;

                    case "apellido":
                    case "apellidoempleado":
                    case "empleadoapellido":
                        query = query.Where(ot => ot.fk_EmpleadoNavigation.emp_apellido.ToLower().Contains(busquedaLower));
                        break;

                    case "cedulaempleado":
                    case "empleadocedula":
                        query = query.Where(ot => ot.fk_EmpleadoNavigation.emp_cedula.ToLower().Contains(busquedaLower));
                        break;

                    case "empleado":
                    case "pkempleado":
                    case "idempleado":
                        if (int.TryParse(busqueda, out int empleadoId))
                        {
                            query = query.Where(ot => ot.fk_Empleado == empleadoId);
                        }
                        break;

                    // Búsqueda por datos de la orden
                    case "codigo":
                    case "codigoot":
                    case "otra_codigo":
                        query = query.Where(ot => ot.otra_codigo.ToLower().Contains(busquedaLower));
                        break;

                    case "estado":
                    case "estadoot":
                        query = query.Where(ot => ot.otra_estado.ToLower().Contains(busquedaLower));
                        break;

                    case "prioridad":
                    case "prioridadot":
                        query = query.Where(ot => ot.otra_prioridad.ToLower().Contains(busquedaLower));
                        break;

                    // Búsqueda por fechas
                    case "fechaprogramada":
                    case "fechaprog":
                        if (DateTime.TryParse(busqueda, out DateTime fechaProg))
                        {
                            query = query.Where(ot => ot.otra_fecha_programada.HasValue &&
                                                     ot.otra_fecha_programada.Value.Date == fechaProg.Date);
                        }
                        break;

                    case "fecharegistro":
                    case "fechareg":
                        if (DateTime.TryParse(busqueda, out DateTime fechaReg))
                        {
                            query = query.Where(ot => ot.otra_fecha_registro.HasValue &&
                                                     ot.otra_fecha_registro.Value.Date == fechaReg.Date);
                        }
                        break;

                    // Búsqueda por servicio
                    case "servicio":
                    case "nombreservicio":
                        query = query.Where(ot => ot.fk_servicioNavigation.ser_nombre.ToLower().Contains(busquedaLower));
                        break;

                    case "pkservicio":
                    case "idservicio":
                        if (int.TryParse(busqueda, out int servicioId))
                        {
                            query = query.Where(ot => ot.fk_servicio == servicioId);
                        }
                        break;

                    // Búsqueda por ID de orden
                    case "id":
                    case "pkorden":
                    case "ordenid":
                        if (int.TryParse(busqueda, out int ordenId))
                        {
                            query = query.Where(ot => ot.pk_orden_trabajo == ordenId);
                        }
                        break;

                    default:
                        // Búsqueda general en múltiples campos
                        query = query.Where(ot =>
                            ot.otra_codigo.ToLower().Contains(busquedaLower) ||
                            ot.otra_descripcion.ToLower().Contains(busquedaLower) ||
                            ot.fk_servicioNavigation.ser_nombre.ToLower().Contains(busquedaLower) ||
                            ot.fk_EmpleadoNavigation.emp_nombre.ToLower().Contains(busquedaLower) ||
                            ot.fk_EmpleadoNavigation.emp_apellido.ToLower().Contains(busquedaLower) ||
                            ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre.ToLower().Contains(busquedaLower) ||
                            ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido.ToLower().Contains(busquedaLower)
                        );
                        break;
                }

                var lista = await query.ToListAsync();

                return lista.Select(ot => new OrdenTrabajo
                {
                    // Datos del servicio
                    pk_servicio = ot.fk_servicioNavigation.pk_servicio,
                    ser_nombre = ot.fk_servicioNavigation.ser_nombre,
                    ser_precio = ot.fk_servicioNavigation.ser_precio,
                    ser_descripcion = ot.fk_servicioNavigation.ser_descripcion,
                    ser_requiere_material = ot.fk_servicioNavigation.ser_requiere_material,
                    ser_tipo_factura = ot.fk_servicioNavigation.ser_tipo_factura,
                    fk_cliente = ot.fk_servicioNavigation.fk_cliente,

                    // Datos del cliente
                    pk_cliente = ot.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                    cli_cedula = ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula,
                    cli_nombre = ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre,
                    cli_apellido = ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido,

                    // Datos de la orden de trabajo
                    pk_orden_trabajo = ot.pk_orden_trabajo,
                    otra_codigo = ot.otra_codigo,
                    otra_descripcion = ot.otra_descripcion,
                    otra_fecha_registro = ot.otra_fecha_registro,
                    otra_fecha_programada = ot.otra_fecha_programada,
                    otra_tiempo_estimado = ot.otra_tiempo_estimado,
                    otra_estado = ot.otra_estado,
                    otra_prioridad = ot.otra_prioridad,
                    fk_Empleado = ot.fk_Empleado,
                    fk_servicio = ot.fk_servicio,

                    // Datos del empleado
                    pk_Empleado = ot.fk_EmpleadoNavigation.pk_Empleado,
                    emp_roll = ot.fk_EmpleadoNavigation.emp_roll,
                    emp_nombre = ot.fk_EmpleadoNavigation.emp_nombre,
                    emp_apellido = ot.fk_EmpleadoNavigation.emp_apellido,
                    emp_cedula = ot.fk_EmpleadoNavigation.emp_cedula

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar órdenes de trabajo", ex);
            }
        }

        // ===============================
        // MÉTODOS AUXILIARES PARA FORMULARIOS
        // ===============================

        /// <summary>
        /// Obtiene lista simplificada de empleados activos para llenar combos/selects
        /// </summary>
        public async Task<List<object>> ObtenerEmpleadosParaFormularioAsync()
        {
            try
            {
                return await _context.Empleado
                    .Where(e => e.emp_activo == 1)
                    .Select(e => new
                    {
                        pk_Empleado = e.pk_Empleado,
                        emp_nombre = e.emp_nombre,
                        emp_apellido = e.emp_apellido,
                        emp_roll = e.emp_roll,
                        NombreCompleto = e.emp_nombre + " " + e.emp_apellido + " (" + e.emp_roll + ")"
                    })
                    .Cast<object>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener empleados para formulario", ex);
            }
        }

        /// <summary>
        /// Obtiene lista simplificada de servicios disponibles para llenar combos/selects
        /// </summary>
        public async Task<List<object>> ObtenerServiciosParaFormularioAsync()
        {
            try
            {
                return await _context.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .Where(s => s.ser_tipo_factura != "Cancelado")
                    .Select(s => new
                    {
                        pk_servicio = s.pk_servicio,
                        ser_nombre = s.ser_nombre,
                        ser_precio = s.ser_precio,
                        ClienteNombre = s.fk_clienteNavigation.cli_nombre + " " + s.fk_clienteNavigation.cli_apellido,
                        DescripcionCompleta = s.ser_nombre + " - " + s.fk_clienteNavigation.cli_nombre + " " + s.fk_clienteNavigation.cli_apellido + " ($" + s.ser_precio + ")"
                    })
                    .Cast<object>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener servicios para formulario", ex);
            }
        }

        // ===============================
        // MÉTODO DE ESTADÍSTICAS OPCIONAL
        // ===============================

        public async Task<Dictionary<string, int>> ObtenerEstadisticasOrdenesAsync()
        {
            try
            {
                var estadisticas = await _context.Orden_Trabajo
                    .Where(ot => ot.otra_estado != "Cancelado")
                    .GroupBy(ot => ot.otra_estado)
                    .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                    .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);

                return estadisticas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener estadísticas de órdenes", ex);
            }
        }

        // ===============================
        // MÉTODO PARA OBTENER UNA ORDEN CON DATOS COMPLETOS
        // ===============================

        public async Task<OrdenTrabajo> ObtenerOrdenCompletaPorIdAsync(int id)
        {
            try
            {
                var ordenTrabajo = await _context.Orden_Trabajo
                    .Include(ot => ot.fk_servicioNavigation)
                        .ThenInclude(s => s.fk_clienteNavigation)
                    .Include(ot => ot.fk_EmpleadoNavigation)
                    .FirstOrDefaultAsync(ot => ot.pk_orden_trabajo == id);

                if (ordenTrabajo == null) return null;

                // Mapear a DTO completo
                return new OrdenTrabajo
                {
                    // Datos del servicio
                    pk_servicio = ordenTrabajo.fk_servicioNavigation.pk_servicio,
                    ser_nombre = ordenTrabajo.fk_servicioNavigation.ser_nombre,
                    ser_precio = ordenTrabajo.fk_servicioNavigation.ser_precio,
                    ser_descripcion = ordenTrabajo.fk_servicioNavigation.ser_descripcion,
                    ser_requiere_material = ordenTrabajo.fk_servicioNavigation.ser_requiere_material,
                    ser_tipo_factura = ordenTrabajo.fk_servicioNavigation.ser_tipo_factura,
                    fk_cliente = ordenTrabajo.fk_servicioNavigation.fk_cliente,

                    // Datos del cliente
                    pk_cliente = ordenTrabajo.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                    cli_cedula = ordenTrabajo.fk_servicioNavigation.fk_clienteNavigation.cli_cedula,
                    cli_nombre = ordenTrabajo.fk_servicioNavigation.fk_clienteNavigation.cli_nombre,
                    cli_apellido = ordenTrabajo.fk_servicioNavigation.fk_clienteNavigation.cli_apellido,

                    // Datos de la orden de trabajo
                    pk_orden_trabajo = ordenTrabajo.pk_orden_trabajo,
                    otra_codigo = ordenTrabajo.otra_codigo,
                    otra_descripcion = ordenTrabajo.otra_descripcion,
                    otra_fecha_registro = ordenTrabajo.otra_fecha_registro,
                    otra_fecha_programada = ordenTrabajo.otra_fecha_programada,
                    otra_tiempo_estimado = ordenTrabajo.otra_tiempo_estimado,
                    otra_estado = ordenTrabajo.otra_estado,
                    otra_prioridad = ordenTrabajo.otra_prioridad,
                    fk_Empleado = ordenTrabajo.fk_Empleado,
                    fk_servicio = ordenTrabajo.fk_servicio,

                    // Datos del empleado
                    pk_Empleado = ordenTrabajo.fk_EmpleadoNavigation.pk_Empleado,
                    emp_roll = ordenTrabajo.fk_EmpleadoNavigation.emp_roll,
                    emp_nombre = ordenTrabajo.fk_EmpleadoNavigation.emp_nombre,
                    emp_apellido = ordenTrabajo.fk_EmpleadoNavigation.emp_apellido,
                    emp_cedula = ordenTrabajo.fk_EmpleadoNavigation.emp_cedula
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener orden completa con ID {id}", ex);
            }
        }
    }
}