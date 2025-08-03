using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class OrdenMaterialRepositorioImpl : RepositorioImpl<Orden_Material>, IOrdenMaterialRepositorio
    {
        private readonly NovusnetPROContext _context;

        public OrdenMaterialRepositorioImpl(NovusnetPROContext context) : base(context)
        {
            _context = context;
        }

        public async Task OrdenMaterialAddAsync(Orden_Material entidad)
        {
            await _context.Orden_Material.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Orden_Material>> OrdenMaterialGetAllAsync()
        {
            return await _context.Orden_Material
                .Include(x => x.fk_materialNavigation)
                .Include(x => x.fk_orden_trabajoNavigation)
                .ToListAsync();
        }

        public async Task<Orden_Material> OrdenMaterialGetByIdAsync(int pk_orden_material)
        {
            return await _context.Orden_Material
                .Include(x => x.fk_materialNavigation)
                .Include(x => x.fk_orden_trabajoNavigation)
                .FirstOrDefaultAsync(x => x.pk_orden_material == pk_orden_material);
        }

        public async Task OrdenMaterialUpdateAsync(Orden_Material entidad)
        {
            var existente = await _context.Orden_Material.FindAsync(entidad.pk_orden_material);

            if (existente == null)
                throw new Exception("Orden_Material no encontrada.");

            existente.orma_codigo = entidad.orma_codigo;
            existente.orma_cantidad = entidad.orma_cantidad;
            existente.orma_estado = entidad.orma_estado;
            existente.orma_observaciones = entidad.orma_observaciones;
            existente.orma_fecha_uso = entidad.orma_fecha_uso;
            existente.fk_material = entidad.fk_material;
            existente.fk_orden_trabajo = entidad.fk_orden_trabajo;

            await _context.SaveChangesAsync();
        }

        public async Task OrdenMaterialDeleteAsync(int pk_orden_material)
        {
            var entidad = await _context.Orden_Material.FindAsync(pk_orden_material);
            if (entidad != null)
            {
                entidad.orma_estado = "cancelado/devuelto";
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Orden_Material no encontrada.");
            }
        }

       

        public async Task<List<OrdenMaterialDTO>> ObtenerOrdenesTrabajoCompletasConMaterialesAsync()
        {
            return await _context.Orden_Trabajo
                .Include(ot => ot.fk_servicioNavigation)
                    .ThenInclude(s => s.fk_clienteNavigation)
                .Include(ot => ot.fk_EmpleadoNavigation)
                .Include(ot => ot.Orden_Material)
                    .ThenInclude(om => om.fk_materialNavigation)
                .Where(ot => ot.otra_estado != "Cancelado")
                .SelectMany(ot => ot.Orden_Material.DefaultIfEmpty(), (ot, om) => new OrdenMaterialDTO
                {
                    // Datos de Orden Trabajo
                    pk_orden_trabajo = ot.pk_orden_trabajo,
                    otra_codigo = ot.otra_codigo ?? string.Empty,
                    otra_descripcion = ot.otra_descripcion ?? string.Empty,
                    otra_fecha_registro = ot.otra_fecha_registro,
                    otra_fecha_programada = ot.otra_fecha_programada,
                    otra_tiempo_estimado = ot.otra_tiempo_estimado ?? string.Empty,
                    otra_estado = ot.otra_estado ?? string.Empty,
                    otra_prioridad = ot.otra_prioridad ?? string.Empty,
                    fk_Empleado = ot.fk_Empleado,
                    fk_servicio = ot.fk_servicio,
                    fk_cliente = ot.fk_servicioNavigation.fk_cliente, // Agregado según tu DTO

                    // Datos de Cliente
                    pk_cliente = ot.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                    cli_cedula = ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula ?? string.Empty,
                    cli_nombre = ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre ?? string.Empty,
                    cli_apellido = ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido ?? string.Empty,

                    // Datos de Empleado
                    pk_empleado = ot.fk_EmpleadoNavigation.pk_Empleado,
                    emp_cedula = ot.fk_EmpleadoNavigation.emp_cedula ?? string.Empty,
                    emp_nombre = ot.fk_EmpleadoNavigation.emp_nombre ?? string.Empty,
                    emp_apellido = ot.fk_EmpleadoNavigation.emp_apellido ?? string.Empty,
                    emp_roll = ot.fk_EmpleadoNavigation.emp_roll ?? string.Empty,

                    // Datos de Servicio
                    pk_servicio = ot.fk_servicioNavigation.pk_servicio,
                    ser_codigo = string.Empty, // No existe en la DB pero está en el DTO
                    ser_nombre = ot.fk_servicioNavigation.ser_nombre ?? string.Empty,
                    ser_precio = ot.fk_servicioNavigation.ser_precio,
                    ser_descripcion = ot.fk_servicioNavigation.ser_descripcion ?? string.Empty,
                    ser_requiere_material = ot.fk_servicioNavigation.ser_requiere_material,
                    ser_tipo_factura = ot.fk_servicioNavigation.ser_tipo_factura ?? string.Empty,

                    // Datos de Orden Material - CON MANEJO SEGURO DE NULLS
                    pk_orden_material = om != null ? om.pk_orden_material : 0,
                    fk_material = om != null ? om.fk_material : 0,
                    fk_orden_trabajo = om != null ? om.fk_orden_trabajo : 0,
                    orma_cantidad = om != null ? (om.orma_cantidad ?? 0) : 0,
                    orma_estado = om != null ? (om.orma_estado ?? string.Empty) : string.Empty,

                    // Datos de Material - SOLO LOS CAMPOS QUE TIENES EN TU DTO
                    pk_material = om != null && om.fk_materialNavigation != null ? om.fk_materialNavigation.pk_material : 0,
                    mat_nombre = om != null && om.fk_materialNavigation != null ? (om.fk_materialNavigation.ma_nombre ?? string.Empty) : string.Empty,
                    mat_precio_unitario = om != null && om.fk_materialNavigation != null ? (om.fk_materialNavigation.ma_precio_unitario ?? 0) : 0
                })
                .OrderBy(dto => dto.pk_servicio)        // Primero por servicio
                .ThenBy(dto => dto.otra_codigo)         // Luego por código de orden
                .ThenBy(dto => dto.pk_orden_trabajo)    // Finalmente por ID de orden
                .ToListAsync();
        }

        public async Task<List<OrdenMaterialDTO>> FiltrarOrdenesTrabajoPorCriteriosAsync(string criterio, string valor)
        {
            var query = _context.Orden_Trabajo
                .Include(ot => ot.fk_servicioNavigation)
                    .ThenInclude(s => s.fk_clienteNavigation)
                .Include(ot => ot.fk_EmpleadoNavigation)
                .Include(ot => ot.Orden_Material)
                    .ThenInclude(om => om.fk_materialNavigation)
                .Where(ot => ot.otra_estado != "Cancelado")
                .AsQueryable();

            string filtro = valor?.ToLower() ?? "";
            criterio = criterio?.ToLower();

            switch (criterio)
            {
                // Filtros por Cliente
                case "cliente_nombre":
                case "nombre_cliente":
                    query = query.Where(o => o.fk_servicioNavigation.fk_clienteNavigation.cli_nombre.ToLower().Contains(filtro));
                    break;

                case "cliente_apellido":
                case "apellido_cliente":
                    query = query.Where(o => o.fk_servicioNavigation.fk_clienteNavigation.cli_apellido.ToLower().Contains(filtro));
                    break;

                case "cliente_cedula":
                case "cedula_cliente":
                    query = query.Where(o => o.fk_servicioNavigation.fk_clienteNavigation.cli_cedula.ToLower().Contains(filtro));
                    break;

                // Filtros por Empleado
                case "empleado_nombre":
                case "nombre_empleado":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_nombre.ToLower().Contains(filtro));
                    break;

                case "empleado_apellido":
                case "apellido_empleado":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_apellido.ToLower().Contains(filtro));
                    break;

                case "empleado_cedula":
                case "cedula_empleado":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_cedula.ToLower().Contains(filtro));
                    break;

                case "empleado_roll":
                case "roll_empleado":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_roll.ToLower().Contains(filtro));
                    break;

                // Filtros por Servicio
                case "servicio_nombre":
                case "nombre_servicio":
                    query = query.Where(o => o.fk_servicioNavigation.ser_nombre.ToLower().Contains(filtro));
                    break;

                case "servicio_descripcion":
                case "descripcion_servicio":
                    query = query.Where(o => o.fk_servicioNavigation.ser_descripcion.ToLower().Contains(filtro));
                    break;

                case "servicio_tipo_factura":
                case "tipo_factura":
                    query = query.Where(o => o.fk_servicioNavigation.ser_tipo_factura.ToLower().Contains(filtro));
                    break;

                // Filtros por Orden de Trabajo
                case "codigo":
                case "orden_codigo":
                case "otra_codigo":
                    query = query.Where(o => o.otra_codigo.ToLower().Contains(filtro));
                    break;

                case "orden_descripcion":
                case "otra_descripcion":
                    query = query.Where(o => o.otra_descripcion.ToLower().Contains(filtro));
                    break;

                case "orden_estado":
                case "otra_estado":
                case "estado":
                    query = query.Where(o => o.otra_estado.ToLower().Contains(filtro));
                    break;

                case "orden_prioridad":
                case "otra_prioridad":
                case "prioridad":
                    query = query.Where(o => o.otra_prioridad.ToLower().Contains(filtro));
                    break;

                // Filtros por Material
                case "material_nombre":
                case "nombre_material":
                    query = query.Where(o => o.Orden_Material.Any(om => om.fk_materialNavigation.ma_nombre.ToLower().Contains(filtro)));
                    break;

                case "material_codigo":
                case "codigo_material":
                    query = query.Where(o => o.Orden_Material.Any(om => om.fk_materialNavigation.ma_codigo.ToLower().Contains(filtro)));
                    break;

                case "estado_material":
                case "orma_estado":
                    query = query.Where(o => o.Orden_Material.Any(om => om.orma_estado.ToLower().Contains(filtro)));
                    break;

                // Filtros por IDs (útiles para búsquedas exactas)
                case "pk_servicio":
                case "id_servicio":
                    if (int.TryParse(valor, out int servicioId))
                    {
                        query = query.Where(o => o.fk_servicio == servicioId);
                    }
                    break;

                case "pk_cliente":
                case "id_cliente":
                    if (int.TryParse(valor, out int clienteId))
                    {
                        query = query.Where(o => o.fk_servicioNavigation.fk_cliente == clienteId);
                    }
                    break;

                case "pk_empleado":
                case "id_empleado":
                    if (int.TryParse(valor, out int empleadoId))
                    {
                        query = query.Where(o => o.fk_Empleado == empleadoId);
                    }
                    break;

                case "pk_orden_trabajo":
                case "id_orden_trabajo":
                    if (int.TryParse(valor, out int ordenId))
                    {
                        query = query.Where(o => o.pk_orden_trabajo == ordenId);
                    }
                    break;

                // Filtros por fechas
                case "fecha_registro":
                case "otra_fecha_registro":
                    if (DateTime.TryParse(valor, out DateTime fechaRegistro))
                    {
                        query = query.Where(o => o.otra_fecha_registro.HasValue &&
                                               o.otra_fecha_registro.Value.Date == fechaRegistro.Date);
                    }
                    break;

                case "fecha_programada":
                case "otra_fecha_programada":
                    if (DateTime.TryParse(valor, out DateTime fechaProgramada))
                    {
                        query = query.Where(o => o.otra_fecha_programada.HasValue &&
                                               o.otra_fecha_programada.Value.Date == fechaProgramada.Date);
                    }
                    break;

                // Filtros por precio
                case "servicio_precio":
                case "precio_servicio":
                    if (decimal.TryParse(valor, out decimal precio))
                    {
                        query = query.Where(o => o.fk_servicioNavigation.ser_precio == precio);
                    }
                    break;

                default:
                    // Si no se reconoce el criterio, no aplicar filtro adicional
                    break;
            }

            // Ejecutar la consulta y mapear a DTO según tu estructura exacta
            return await query
                .SelectMany(ot => ot.Orden_Material.DefaultIfEmpty(), (ot, om) => new OrdenMaterialDTO
                {
                    // Datos de Orden Trabajo
                    pk_orden_trabajo = ot.pk_orden_trabajo,
                    otra_codigo = ot.otra_codigo ?? string.Empty,
                    otra_descripcion = ot.otra_descripcion ?? string.Empty,
                    otra_fecha_registro = ot.otra_fecha_registro,
                    otra_fecha_programada = ot.otra_fecha_programada,
                    otra_tiempo_estimado = ot.otra_tiempo_estimado ?? string.Empty,
                    otra_estado = ot.otra_estado ?? string.Empty,
                    otra_prioridad = ot.otra_prioridad ?? string.Empty,
                    fk_Empleado = ot.fk_Empleado,
                    fk_servicio = ot.fk_servicio,
                    fk_cliente = ot.fk_servicioNavigation.fk_cliente, // Agregado según tu DTO

                    // Datos de Cliente
                    pk_cliente = ot.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                    cli_cedula = ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula ?? string.Empty,
                    cli_nombre = ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre ?? string.Empty,
                    cli_apellido = ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido ?? string.Empty,

                    // Datos de Empleado
                    pk_empleado = ot.fk_EmpleadoNavigation.pk_Empleado,
                    emp_cedula = ot.fk_EmpleadoNavigation.emp_cedula ?? string.Empty,
                    emp_nombre = ot.fk_EmpleadoNavigation.emp_nombre ?? string.Empty,
                    emp_apellido = ot.fk_EmpleadoNavigation.emp_apellido ?? string.Empty,
                    emp_roll = ot.fk_EmpleadoNavigation.emp_roll ?? string.Empty,

                    // Datos de Servicio
                    pk_servicio = ot.fk_servicioNavigation.pk_servicio,
                    ser_codigo = string.Empty, // No existe en tu DB, pero está en el DTO
                    ser_nombre = ot.fk_servicioNavigation.ser_nombre ?? string.Empty,
                    ser_precio = ot.fk_servicioNavigation.ser_precio,
                    ser_descripcion = ot.fk_servicioNavigation.ser_descripcion ?? string.Empty,
                    ser_requiere_material = ot.fk_servicioNavigation.ser_requiere_material,
                    ser_tipo_factura = ot.fk_servicioNavigation.ser_tipo_factura ?? string.Empty,

                    // Datos de Orden Material - CON MANEJO SEGURO DE NULLS
                    pk_orden_material = om != null ? om.pk_orden_material : 0,
                    fk_material = om != null ? om.fk_material : 0,
                    fk_orden_trabajo = om != null ? om.fk_orden_trabajo : 0,
                    orma_cantidad = om != null ? (om.orma_cantidad ?? 0) : 0,
                    orma_estado = om != null ? (om.orma_estado ?? string.Empty) : string.Empty,

                    // Datos de Material - SOLO LOS CAMPOS QUE TIENES EN TU DTO
                    pk_material = om != null && om.fk_materialNavigation != null ? om.fk_materialNavigation.pk_material : 0,
                    mat_nombre = om != null && om.fk_materialNavigation != null ? (om.fk_materialNavigation.ma_nombre ?? string.Empty) : string.Empty,
                    mat_precio_unitario = om != null && om.fk_materialNavigation != null ? (om.fk_materialNavigation.ma_precio_unitario ?? 0) : 0
                })
                .OrderBy(dto => dto.pk_servicio)        // Primero por servicio
                .ThenBy(dto => dto.otra_codigo)         // Luego por código de orden
                .ThenBy(dto => dto.pk_orden_trabajo)    // Finalmente por ID de orden
                .ToListAsync();
        }
    }
}
