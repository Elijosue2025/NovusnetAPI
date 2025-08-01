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
                .Select(ot => new OrdenMaterialDTO
                {
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

                    pk_cliente = ot.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                    cli_cedula = ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula,
                    cli_nombre = ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre,
                    cli_apellido = ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido,

                    pk_empleado = ot.fk_EmpleadoNavigation.pk_Empleado,
                    emp_cedula = ot.fk_EmpleadoNavigation.emp_cedula,
                    emp_nombre = ot.fk_EmpleadoNavigation.emp_nombre,
                    emp_apellido = ot.fk_EmpleadoNavigation.emp_apellido,
                    emp_roll = ot.fk_EmpleadoNavigation.emp_roll,

                    pk_servicio = ot.fk_servicioNavigation.pk_servicio,
                    ser_nombre = ot.fk_servicioNavigation.ser_nombre,
                    ser_precio = ot.fk_servicioNavigation.ser_precio,
                    ser_descripcion = ot.fk_servicioNavigation.ser_descripcion,
                    ser_requiere_material = ot.fk_servicioNavigation.ser_requiere_material,
                    ser_tipo_factura = ot.fk_servicioNavigation.ser_tipo_factura
                })
                .ToListAsync();
        }

        public async Task<List<OrdenMaterialDTO>> FiltrarOrdenesTrabajoPorCriteriosAsync(string criterio, string valor)
        {
            var query = _context.Orden_Trabajo
                .Include(ot => ot.fk_servicioNavigation).ThenInclude(s => s.fk_clienteNavigation)
                .Include(ot => ot.fk_EmpleadoNavigation)
                .Include(ot => ot.Orden_Material)
                .AsQueryable();

            string filtro = valor?.ToLower() ?? "";
            criterio = criterio?.ToLower();

            switch (criterio)
            {
                case "cliente_nombre":
                    query = query.Where(o => o.fk_servicioNavigation.fk_clienteNavigation.cli_nombre.ToLower().Contains(filtro));
                    break;
                case "cliente_apellido":
                    query = query.Where(o => o.fk_servicioNavigation.fk_clienteNavigation.cli_apellido.ToLower().Contains(filtro));
                    break;
                case "cliente_cedula":
                    query = query.Where(o => o.fk_servicioNavigation.fk_clienteNavigation.cli_cedula.ToLower().Contains(filtro));
                    break;
                case "empleado_nombre":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_nombre.ToLower().Contains(filtro));
                    break;
                case "empleado_apellido":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_apellido.ToLower().Contains(filtro));
                    break;
                case "empleado_cedula":
                    query = query.Where(o => o.fk_EmpleadoNavigation.emp_cedula.ToLower().Contains(filtro));
                    break;
                case "servicio_nombre":
                    query = query.Where(o => o.fk_servicioNavigation.ser_nombre.ToLower().Contains(filtro));
                    break;
                case "estado_material":
                    query = query.Where(o => o.Orden_Material.Any(m => m.orma_estado.ToLower().Contains(filtro)));
                    break;
            }

            var resultado = await query.ToListAsync();

            return resultado.Select(ot => new OrdenMaterialDTO
            {
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

                pk_cliente = ot.fk_servicioNavigation.fk_clienteNavigation.pk_cliente,
                cli_cedula = ot.fk_servicioNavigation.fk_clienteNavigation.cli_cedula,
                cli_nombre = ot.fk_servicioNavigation.fk_clienteNavigation.cli_nombre,
                cli_apellido = ot.fk_servicioNavigation.fk_clienteNavigation.cli_apellido,

                pk_empleado = ot.fk_EmpleadoNavigation.pk_Empleado,
                emp_cedula = ot.fk_EmpleadoNavigation.emp_cedula,
                emp_nombre = ot.fk_EmpleadoNavigation.emp_nombre,
                emp_apellido = ot.fk_EmpleadoNavigation.emp_apellido,
                emp_roll = ot.fk_EmpleadoNavigation.emp_roll,

                pk_servicio = ot.fk_servicioNavigation.pk_servicio,
                ser_nombre = ot.fk_servicioNavigation.ser_nombre,
                ser_precio = ot.fk_servicioNavigation.ser_precio,
                ser_descripcion = ot.fk_servicioNavigation.ser_descripcion,
                ser_requiere_material = ot.fk_servicioNavigation.ser_requiere_material,
                ser_tipo_factura = ot.fk_servicioNavigation.ser_tipo_factura
            }).ToList();
        }
    }
}
